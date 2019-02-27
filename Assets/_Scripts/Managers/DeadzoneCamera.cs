using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Camera))]
public class DeadzoneCamera : MonoBehaviour
{
    public static DeadzoneCamera Instance;
    void Awake()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
    }

    public Renderer target;
    public Camera _camera;
    public Camera cameraMap;
    public Camera cameraAttack;

    public Rect deadzone;
    public Vector3 smoothPos;
    public Vector3 smoothPosMap;

    public Rect[] limits;

    protected Vector3 _currentVelocity;

    float minFov = 5f;
    float maxFov = 25f;

    float speed = 2.0f;
    int boundary = 10;
    int width;
    int height;

    public void Start()
    {
        smoothPos = target.transform.position;
        smoothPos.z = transform.position.z;
        _currentVelocity = Vector3.zero;

        _camera = GetComponent<Camera>();
        if (!_camera.orthographic)
        {
            Debug.LogError("deadzone script require an orthographic camera!");
            Destroy(this);
        }

        width = Screen.width;
        height = Screen.height;
    }

    public void Update()
    {
        #region CameraMain
        if (Input.touchCount == 1 && UIManager.Instance.isBinoculars)
        {
            wasZoomingLastFrame = false;

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                panFingerId = touch.fingerId;
            }
            else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
            {
                PanCamera(touch.position);
            }
        }
        else if (Input.touchCount == 2)
        {
            Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
            if (!wasZoomingLastFrame)
            {
                lastZoomPositions = newPositions;
                wasZoomingLastFrame = true;
            }
            else
            {
                // Zoom based on the distance between the new positions compared to the 
                // distance between the previous positions.
                float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                float offset = newDistance - oldDistance;

                ZoomCamera(offset, ZoomSpeedTouch);

                lastZoomPositions = newPositions;
            }
        }
        else
        {
            wasZoomingLastFrame = false;
        }

        float localX = target.transform.position.x - transform.position.x;
        float localY = target.transform.position.y - transform.position.y;

        if (localX < deadzone.xMin)
        {
            smoothPos.x += localX - deadzone.xMin;
        }
        else if (localX > deadzone.xMax)
        {
            smoothPos.x += localX - deadzone.xMax;
        }

        if (localY < deadzone.yMin)
        {
            smoothPos.y += localY - deadzone.yMin;
        }
        else if (localY > deadzone.yMax)
        {
            smoothPos.y += localY - deadzone.yMax;
        }

        Rect camWorldRect = new Rect();
        camWorldRect.min = new Vector2(smoothPos.x - _camera.aspect * _camera.orthographicSize, smoothPos.y - _camera.orthographicSize);
        camWorldRect.max = new Vector2(smoothPos.x + _camera.aspect * _camera.orthographicSize, smoothPos.y + _camera.orthographicSize);

        for (int i = 0; i < limits.Length; ++i)
        {
            if (limits[i].Contains(target.transform.position))
            {
                Vector3 localOffsetMin = limits[i].min + camWorldRect.size * 0.5f;
                Vector3 localOffsetMax = limits[i].max - camWorldRect.size * 0.5f;

                localOffsetMin.z = localOffsetMax.z = smoothPos.z;

                smoothPos = Vector3.Max(smoothPos, localOffsetMin);
                smoothPos = Vector3.Min(smoothPos, localOffsetMax);

                break;
            }
        }

        Vector3 current = transform.position;
        smoothPos.z = -10;
        current = smoothPos;
        transform.position = Vector3.SmoothDamp(current, smoothPos, ref _currentVelocity, 0.1f);
        #endregion

    }
    private static readonly float PanSpeed = 20f;
    private static readonly float ZoomSpeedTouch = 0.1f;
    private static readonly float ZoomSpeedMouse = 0.5f;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            PanCamera(Input.mousePosition);
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        ZoomCamera(scroll, ZoomSpeedMouse);
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = cameraMap.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed, offset.y * PanSpeed);

        // Perform the movement
        cameraMap.transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 targetMap = cameraMap.transform.position;
        if (Input.mousePosition.x > width - boundary)
        {
            targetMap += new Vector3(Time.deltaTime * speed, 0.0f);
        }

        if (Input.mousePosition.x < 0 + boundary)
        {
            targetMap -= new Vector3(Time.deltaTime * speed, 0.0f);
        }

        if (Input.mousePosition.y > height - boundary)
        {
            targetMap += new Vector3(0.0f, Time.deltaTime * speed);
        }

        if (Input.mousePosition.y < 0 + boundary)
        {
            targetMap -= new Vector3(0.0f, Time.deltaTime * speed);
        }

        smoothPosMap = targetMap;

        Rect camWorldRectMap = new Rect();
        camWorldRectMap.min = new Vector2(smoothPosMap.x - cameraMap.aspect * cameraMap.orthographicSize, smoothPosMap.y - cameraMap.orthographicSize);
        camWorldRectMap.max = new Vector2(smoothPosMap.x + cameraMap.aspect * cameraMap.orthographicSize, smoothPosMap.y + cameraMap.orthographicSize);


        for (int i = 0; i < limits.Length; ++i)
        {
            if (limits[i].Contains(targetMap))
            {
                Vector3 localOffsetMin = limits[i].min + camWorldRectMap.size * 0.5f;
                Vector3 localOffsetMax = limits[i].max - camWorldRectMap.size * 0.5f;

                localOffsetMin.z = localOffsetMax.z = smoothPosMap.z;

                smoothPosMap = Vector3.Max(smoothPosMap, localOffsetMin);
                smoothPosMap = Vector3.Min(smoothPosMap, localOffsetMax);

                break;
            }
        }

        smoothPosMap.z = -10;
        cameraMap.transform.position = smoothPosMap;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }

        _camera.orthographicSize = Mathf.Clamp(cameraMap.orthographicSize - (offset * speed), minFov, maxFov);
        cameraMap.orthographicSize = _camera.orthographicSize;
    }

}

#if UNITY_EDITOR

[CustomEditor(typeof(DeadzoneCamera))]
public class DeadZonEditor : Editor
{
    public void OnSceneGUI()
    {
        DeadzoneCamera cam = target as DeadzoneCamera;

        Vector3[] vert =
        {
            cam.transform.position + new Vector3(cam.deadzone.xMin, cam.deadzone.yMin, 0),
            cam.transform.position + new Vector3(cam.deadzone.xMax, cam.deadzone.yMin, 0),
            cam.transform.position + new Vector3(cam.deadzone.xMax, cam.deadzone.yMax, 0),
            cam.transform.position + new Vector3(cam.deadzone.xMin, cam.deadzone.yMax, 0)
        };

        Color transp = new Color(0, 0, 0, 0);
        Handles.DrawSolidRectangleWithOutline(vert, transp, Color.red);

        for (int i = 0; i < cam.limits.Length; ++i)
        {
            Vector3[] vertLimit =
           {
                new Vector3(cam.limits[i].xMin, cam.limits[i].yMin, 0),
                new Vector3(cam.limits[i].xMax, cam.limits[i].yMin, 0),
                new Vector3(cam.limits[i].xMax, cam.limits[i].yMax, 0),
                new Vector3(cam.limits[i].xMin, cam.limits[i].yMax, 0)
            };

            Handles.DrawSolidRectangleWithOutline(vertLimit, transp, Color.blue);

        }
    }
}
#endif