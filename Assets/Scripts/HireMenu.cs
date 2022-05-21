using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HireMenu : MonoBehaviour
{
    public float HeroCost;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI HeroText;
    public TextMeshProUGUI PriceText;
    public Menu HireScreen;
    public Transform HeroParent;
    public Transform CurrentHero;
    public StockManager Stock;
    public GameObject HeroButton;
    public HeroGenerator Generator;
    public ScrollingWindow DungeonContent;

    private void Start()
    {
        NewHero();
    }
    public void HireHero()
    {
        if(HeroCost <= Stock.Gold)
        {
            Stock.CollectGold(-HeroCost);
            CurrentHero.SetParent(HeroParent);
            CurrentHero.GetComponent<Stats>().State = Stats.HeroState.Idle;
            CurrentHero = null;
            HireScreen.DeActivate();
            HeroButton.SetActive(false);
            DungeonContent.SetWindow(2);
        }
        else
        {
            HeroText.text = "YOU CAN'T AFFORD ME!";
        }
        

    }

    public void UpdateHeroInfo(Transform hero)
    {
        CurrentHero = hero;
        var s = CurrentHero.GetComponent<Stats>();
        HeroCost = s.Level * 100;
        PriceText.text = HeroCost + "G";
        NameText.text = s.HeroName;
        HeroText.text = "Level " + s.Level + " " + s.Class + "\n HP:" + s.MaxHP + "\n XP:" + s.XP + " / " + s.MaxXP + "\n Damage:" + s.Damage + "\n Range:" + Mathf.FloorToInt(s.Range) + "\n Gold Drop: x" + s.Discovery;


    }

    public void NewHero()
    {
        HeroButton.SetActive(true);
        HireScreen.DeActivate();
        if(CurrentHero != null)
        {
            Destroy(CurrentHero.gameObject);
        }
        Generator.CreateHero();
    }

}
