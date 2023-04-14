using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Prospector : MonoBehaviour
{
    public Menu ProspectorMenu;
    public List<Vector2> UpgradeLevels;
    public int CurrentLevel = 0;
    public StockManager Stock;
    public Button ProspectorButton;
    public Button CollectButton;
    public Button UpgradeButton;
    public Button HireButton;
    public TextMeshProUGUI UpgradeText;
    public TextMeshProUGUI ExpeditionText;
    public Image ProspectorSprite;
    public GameObject NewIcon;
    public GameObject HireMenu;
    public GameObject CollectMenu;
    public Slider TimeSlider;
    public float ProspectTime = 300;
    public float Timer;
    public bool IsHired = false;
    public bool Mining = false;
    public bool ReturnedFromMining = false;
    public InnManager Inn;
    public SaveManager Save;
    public ProspectorData Data;


    private void Start()
    {
        ProspectorButton = gameObject.GetComponent<Button>();
        if (PlayerPrefs.GetInt("ProspectorUnlocked") == 1)
        {
            gameObject.SetActive(true);
            if (IsHired)
            {
                HireMenu.SetActive(false);
            }
            else
            {
                HireMenu.SetActive(true);
            }

        }
        else
        {
            gameObject.SetActive(false);
        }
        if (ReturnedFromMining)
        {
            ReturnFromMining();
        }
        CheckTimePassed();

    }

    public void Update()
    {
        if (Mining)
        {
            Timer += Time.deltaTime;
            TimeSlider.value = Timer / ProspectTime;
            if(Timer > ProspectTime)
            {
                ReturnFromMining();
            }
        }
        //if(CurrentLevel < UpgradeLevels.Count - 1)
        //{
        //    if(Stock.Gold >= UpgradeLevels[CurrentLevel].x)
        //    {
        //        UpgradeButton.interactable = true;
        //    }
        //    else
        //    {
        //        UpgradeButton.interactable = false;
        //    }            
        //}
        //else
        //{
        //    UpgradeButton.interactable = false;
        //}


    }

    public void CheckTimePassed()
    {
        var lastDateChecked = System.DateTime.Parse(PlayerPrefs.GetString("DateTime"));
        var elapsedSeconds = (int)System.DateTime.Now.Subtract(lastDateChecked).TotalSeconds;
        Debug.Log("Last Date: " + lastDateChecked + "\nCurrent Date: " + System.DateTime.Now + "\nSeconds Passed = " + elapsedSeconds);
        Timer += elapsedSeconds;
        if (!IsHired)
        {
            HireMenu.SetActive(true);
            UpdateUI();
            return;
        }
        else
        {
            HireMenu.SetActive(false);
        }
        if(Timer > ProspectTime)
        {
            ReturnFromMining();
        }
        if (Mining)
        {
            NewIcon.SetActive(false);
            ProspectorButton.interactable = false;
            ProspectorSprite.color = new Color(1, 1, 1, 0.5f);
            TimeSlider.gameObject.SetActive(true);
        }
        UpdateUI();

    }
    public void ReturnFromMining()
    {
        Mining = false;
        ReturnedFromMining = true;
        NewIcon.SetActive(true);
        ProspectorButton.interactable = true;
        ProspectorSprite.color = new Color(1, 1, 1, 1);
        TimeSlider.gameObject.SetActive(false);
        Inn.BottomContent.NewHeroIcon();
        CollectMenu.SetActive(true);
        CollectButton.GetComponentInChildren<TextMeshProUGUI>().text = "COLLECT (" + UpgradeLevels[CurrentLevel].y + "G)";
        if (IsHired)
        {
            HireMenu.SetActive(false);
        }
        else
        {
            HireMenu.SetActive(true);
        }
        UpdateUI();
    }

    public void SendToMine()
    {
        Mining = true;
        Timer = 0;
        ReturnedFromMining = false;
        NewIcon.SetActive(false);
        ProspectorButton.interactable = false;
        ProspectorSprite.color = new Color(1, 1, 1, 0.5f);
        TimeSlider.gameObject.SetActive(true);
        ProspectorMenu.DeActivate();
        Save.SaveGame();

    }

    public void StoreData()
    {
        var newProspectorData = new ProspectorData();
        newProspectorData.LastDateChecked = System.DateTime.Now;
        newProspectorData.Timer = Timer;
        newProspectorData.ProspectTime = ProspectTime;
        newProspectorData.Mining = Mining;
        newProspectorData.ReturnedFromMining = ReturnedFromMining;
        newProspectorData.IsHired = IsHired;
        newProspectorData.CurrentLevel = CurrentLevel;
        Data = newProspectorData;
    }

    public void HireProspector()
    {
        Stock.CollectGold(-5000);
        IsHired = true;
        HireMenu.SetActive(false);
        PlayerPrefs.SetFloat("ProspectorHired", 1);
        UpdateUI();
        Save.SaveGame();
    }

    public void CollectGold()
    {
        Stock.CollectGold(UpgradeLevels[CurrentLevel].y);
        Stock.PlayCollectSound();
        Timer = 0;
        CollectMenu.SetActive(false);
        ReturnedFromMining = false;
        Mining = false;
        UpdateUI();
        Save.SaveGame();

    }

    public void UpgradeMiner()
    {
        Stock.CollectGold(-UpgradeLevels[CurrentLevel].x);
        CurrentLevel++;
        ProspectTime += 300;
        UpdateUI();
        
    }

    public void UpdateUI()
    {
        if (IsHired)
        {
            HireMenu.SetActive(false);
            if (Mining)
            {
                CollectMenu.SetActive(false);
            }
        }
        else
        {
            HireMenu.SetActive(true);
            if(Stock.Gold < 5000)
            {
                HireButton.interactable = false;
            }
            else
            {
                HireButton.interactable = true;
            }
        }
        Debug.Log(CurrentLevel + " - " + UpgradeLevels.Count);
        if (CurrentLevel < UpgradeLevels.Count-1)
        {
            if (Stock.Gold >= UpgradeLevels[CurrentLevel].x)
            {
                UpgradeButton.interactable = true;
            }
            else
            {
                UpgradeButton.interactable = false;
            }
            UpgradeText.gameObject.SetActive(true);
            UpgradeText.text = UpgradeLevels[CurrentLevel].y + "G > " + UpgradeLevels[CurrentLevel+1].y + "G";
            UpgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "UPGRADE (" + UpgradeLevels[CurrentLevel].x + "G)";
            
        }
        else
        {
            UpgradeButton.GetComponentInChildren<TextMeshProUGUI>().text = "FULLY UPGRADED";
            UpgradeButton.interactable = false;
            UpgradeText.gameObject.SetActive(false);
        }

        ExpeditionText.text = Mathf.CeilToInt(ProspectTime / 60) + "m - " + UpgradeLevels[CurrentLevel].y + "G";

        Save.SaveGame();
        
        
        
    }

    public void OpenProspectorMenu()
    {

        ProspectorMenu.Activate();
        if(PlayerPrefs.GetFloat("ProspectorHired") == 1)
        {
            IsHired = true;
        }
        UpdateUI();
        NewIcon.SetActive(false);
    }
}
