using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestManager : MonoBehaviour
{

    public static TestManager Instance;

    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
    }

    public List<Hero> lsPrefabsHero;
    public List<Hero> lsPrefabsEnemy;

    [Header("Charater")]
    public List<Hero> lsHero;
    public List<Hero> lsEnemy;

    [Header("UI")]
    public GameObject pannelSelect;
    public Dropdown dropdownHero;
    public Dropdown dropdownEnemy;
    public InputField txtCountHero;
    public InputField txtCountEnemy;
    public Button btnPlay;

    public Transform posHero;
    public Transform posEnemy;

    [Header("OTHER")]
    public Camera cameraMain;

    private void Start()
    {
        List<string> lsNameHero = new List<string>();
        foreach (Hero hero in lsPrefabsHero)
        {
            lsNameHero.Add(hero.name);
        }
        dropdownHero.AddOptions(lsNameHero);

        List<string> lsNameEnemy = new List<string>();
        foreach (Hero hero in lsPrefabsEnemy)
        {
            lsNameEnemy.Add(hero.name);
        }
        dropdownEnemy.AddOptions(lsNameEnemy);

    }

    public void PlayGame()
    {
        int typeHero = dropdownHero.value;
        int typeEnemy = dropdownEnemy.value;
        int numberHero;
        int numberEnemy;
        try
        {
            numberHero = int.Parse(txtCountHero.text);
            numberEnemy = int.Parse(txtCountEnemy.text);
        }
        catch
        {
            numberHero = 100;
            numberEnemy = 100;
        }

        StartCoroutine(IEInstantiate(lsPrefabsHero[typeHero], posHero, numberHero, "Hero"));

        StartCoroutine(IEInstantiate(lsPrefabsEnemy[typeEnemy], posEnemy, numberEnemy, "Enemy"));

        pannelSelect.SetActive(false);

    }


    public IEnumerator IEInstantiate(Hero prafabs, Transform posIns, int countHero, string name)
    {
        Hero hero = Instantiate<Hero>(prafabs, posIns);
        hero.gameObject.name = name;
        if (name == "Hero")
            lsHero.Add(hero);
        else
        {
            lsEnemy.Add(hero);
            hero.transform.eulerAngles = new Vector3(0f, 0f, 180f);
        }
        yield return new WaitForEndOfFrame();
        hero.infoHero.numberHero = countHero;
        hero.txtCountHero.text = UIManager.Instance.ConvertNumber(hero.infoHero.numberHero);
        hero.infoHero.healthAll = hero.infoHero.health * hero.infoHero.numberHero;
        hero.isInGoldMine = true;

    }

    public void BackMenu()
    {
        if (lsEnemy.Count > 0)
        {
            Destroy(lsEnemy[0].gameObject);
        }
        if (lsHero.Count > 0)
        {
            Destroy(lsHero[0].gameObject);
        }
        lsHero.Clear();
        lsEnemy.Clear();
        pannelSelect.SetActive(true);
    }

}
