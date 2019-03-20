using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
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

    public Camera _camera;
    public Camera cameraAttack;

    private static readonly float PanSpeed = 20f;
    private static readonly float ZoomSpeedTouch = 0.1f;
    private static readonly float ZoomSpeedMouse = 0.5f;

    private static readonly float[] BoundsX = new float[] { -43.5f, 42f };
    private static readonly float[] BoundsY = new float[] { -36.5f, 40f };
    private static readonly float[] ZoomBounds = new float[] { 10f, 35f };

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private bool wasZoomingLastFrame; // Touch mode only
    private Vector2[] lastZoomPositions; // Touch mode only

    private Vector3 pos1;

    void Update()
    {
        if (GameManager.Instance.isPlay)
        {
            if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                HandleTouch();
            }
            else
            {
                HandleMouse();
            }
        }
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                wasZoomingLastFrame = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    pos1 = _camera.ScreenToWorldPoint(lastPanPosition);
                    panFingerId = touch.fingerId;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    if (!IsPointerOverGameObject())
                    {
                        PanCamera(touch.position);
                    }
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Ended)
                {
                    if (!GameManager.Instance.castlePlayer.CheckCastle() && !IsPointerOverGameObject() && GameManager.Instance.castlePlayer.lsHouseRelease.Count > 0)
                    {
                        Vector3 pos2 = _camera.ScreenToWorldPoint(touch.position);
                        if (Vector3.Distance(pos1, pos2) <= 0.05f)
                        {
                            GameManager.Instance.castlePlayer.MoveCastle(touch.position);
                        }
                    }
                }
                break;

            case 2: // Zooming
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
                    float offset = oldDistance / newDistance;
                    if (!IsPointerOverGameObject())
                    {
                        ZoomCamera(offset, ZoomSpeedTouch);
                    }
                    lastZoomPositions = newPositions;
                }
                break;

            default:
                wasZoomingLastFrame = false;
                break;
        }
    }

    void HandleMouse()
    {
        // On mouse down, capture it's position.
        // Otherwise, if the mouse is still down, pan the camera.
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
            pos1 = _camera.ScreenToWorldPoint(lastPanPosition);
        }
        else if (Input.GetMouseButton(0))
        {
            if (!IsPointerOverGameObject())
            {
                PanCamera(Input.mousePosition);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (!GameManager.Instance.castlePlayer.CheckCastle() && !IsPointerOverGameObject() && GameManager.Instance.castlePlayer.lsHouseRelease.Count > 0 && GameManager.Instance.castlePlayer.lsHouseRelease[0].countHero > 0)
            {
                Vector3 pos2 = _camera.ScreenToWorldPoint(Input.mousePosition);
                if (Vector3.Distance(pos1, pos2) <= 0.05f)
                {
                    GameManager.Instance.castlePlayer.MoveCastle(Input.mousePosition);
                }
            }
        }

        // Check for scrolling to zoom the camera
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (!IsPointerOverGameObject())
        {
            ZoomCamera(scroll, ZoomSpeedMouse);
        }
    }
    public bool IsPointerOverGameObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, list);
        return list.Count > 0;
    }

    void PanCamera(Vector3 newPanPosition)
    {
        // Determine how much to move the camera
        Vector3 offset = _camera.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x * PanSpeed * (_camera.orthographicSize / 10f), offset.y * PanSpeed * (_camera.orthographicSize / 10f));

        // Perform the movement
        transform.Translate(move, Space.World);

        // Ensure the camera remains within bounds.
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(transform.position.x, BoundsX[0], BoundsX[1]);
        pos.y = Mathf.Clamp(transform.position.y, BoundsY[0], BoundsY[1]);
        transform.position = pos;

        // Cache the position
        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (offset == 0)
        {
            return;
        }
#if !UNITY_EDITER && (IOS || ANDROID)
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize * offset, ZoomBounds[0], ZoomBounds[1]);
#else
        _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
#endif
    }
}