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

        Hero hero = Instantiate<Hero>(lsPrefabsHero[typeHero], posHero);
        hero.gameObject.name = "Hero";
        lsHero.Add(hero);
        hero.infoHero.numberHero = numberHero;

        Hero enemy = Instantiate<Hero>(lsPrefabsEnemy[typeEnemy], posEnemy);
        hero.gameObject.name = "Enemy";
        lsEnemy.Add(enemy);
        enemy.infoHero.numberHero = numberEnemy;
        enemy.transform.eulerAngles = new Vector3(0f, 0f, 180f);

        pannelSelect.SetActive(false);
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

    public void Update()
    {
        if (lsEnemy.Count <= 0 || lsHero.Count <= 0)
        {
            if (!pannelSelect.activeSelf)
            {
                BackMenu();
            }
        }
    }
}
