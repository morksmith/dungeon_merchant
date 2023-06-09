using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Item : MonoBehaviour
{
    public Color NormalColour;
    public Color SpecialColour;
    public string ItemName;    
    public Image ItemSprite;
    public int SpriteIndex;
    public Image DemandIcon;
    public Sprite DownSprite;
    public Sprite UpSprite;
    public Sprite DoubleUpSprite;
    public float PriceScale = 1;
    public float BasePrice;
    public float Price;
    public ItemType Type;
    public int TypeIndex;
    public GameObject CollectButton;
    public GameObject PriceInfo;
    public TextMeshProUGUI PriceText;
    public float DamageType;
    public float SellTime;
    public bool Locked;
    public bool Equipped;
    public bool Selling;
    public bool Sold;
    public bool Destroyed = false;
    public bool Merchant;
    public bool Special;
    public string BonusString = " ";
    public enum WeaponBonusType
    {
        XP,
        Range,
        HP,
        Gold
    }
    public WeaponBonusType WeaponBonus;
    public float XPBonus;
    public float RangeBonus;
    public float HPBonus;
    public float GoldBonus;
    public enum ArmourBonusType
    {
        Wand,
        Sword,
        Bow,
        Club
    }
    public ArmourBonusType ArmourBonus;
    public float WandBonus;
    public float SwordBonus;
    public float ClubBonus;
    public float BowBonus;
    public float BonusStat;
    public Slider SellSlider;
    public TextMeshProUGUI LevelText;
    public ItemData Data;

    private StockManager stockMan;
    private MerchantMenu merch;
    public float SellTimer;
    // Start is called before the first frame update
    private void Start()
    {
        if(stockMan == null)
        {
            stockMan = GameObject.FindObjectOfType<StockManager>();
        }
        if(merch == null)
        {
            merch = GameObject.FindObjectOfType<MerchantMenu>();
        }

        StoreData();



        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Selling)
        {
            SellSlider.gameObject.SetActive(true);
            GetComponent<Button>().interactable = false;
            if(SellTimer < SellTime)
            {
                SellTimer += Time.deltaTime * stockMan.SellSpeed;
            }
            else
            {
                if (stockMan == null)
                {
                    stockMan = GameObject.FindObjectOfType<StockManager>();
                }
                ReadyToSell();
            }
            SellSlider.value = SellTimer / SellTime;
        }
        else
        {
            SellSlider.gameObject.SetActive(false);
        }
    }

    public void SelectItem()
    {
        if (Merchant)
        {
            merch.SelectItem(this);
        }
        else
        {
            stockMan.SelectItem(this);
        }

    }
    public void CollectGold()
    {
        Destroyed = true;
        stockMan.CollectGold(Price);
        stockMan.PlayCollectSound();
        if (!stockMan.Tutorial)
        {
            stockMan.Save.SaveGame();
        }
        Destroy(gameObject);
    }

    public void EquipItem()
    {
        PriceInfo.SetActive(false);
        Equipped = true;
    }

    public void StockItem()
    {
        PriceInfo.SetActive(true);
        Equipped = false;
    }

    public void ReturnToStock()
    {
        transform.SetParent(stockMan.StockList);
        transform.position = new Vector3(0, 0, 0);
        StockItem();
    }

    public void UpdatePrice(float x)
    {
        PriceScale = x;
        if(x == 0.5f)
        {
            DemandIcon.sprite = DownSprite;
            DemandIcon.color = Color.blue;
            DemandIcon.gameObject.SetActive(true);
        }
        else if (x == 1)
        {
            DemandIcon.gameObject.SetActive(false);
        }
        else if (x == 1.5f)
        {
            DemandIcon.sprite = UpSprite;
            DemandIcon.color = Color.yellow;
            DemandIcon.gameObject.SetActive(true);
        }
        else if (x == 2)
        {
            DemandIcon.sprite = DoubleUpSprite;
            DemandIcon.color = Color.yellow;
            DemandIcon.gameObject.SetActive(true);
        }
        Price = Mathf.CeilToInt(BasePrice * PriceScale);
        if (Merchant)
        {
            if(stockMan!= null)
            {
                Price *= stockMan.MerchantDiscount;
            }
            Price = Mathf.CeilToInt(Price);
        }
        PriceText.text = Price + "G";        
        SellTime = Price;
    }

    public void SetStats()
    {
        if (gameObject.GetComponent<Weapon>())
        {
            gameObject.GetComponent<Weapon>().Damage += (gameObject.GetComponent<Weapon>().Level * 2);
            BasePrice = gameObject.GetComponent<Weapon>().Damage * 3;
            LevelText.text = gameObject.GetComponent<Weapon>().Level.ToString();
            var specialPick = Random.Range(0, 8);
            if(specialPick == 1)
            {
                Special = true;
                var specialStat = Random.Range(1, 10) * 0.1f;
                BonusStat = 1 + specialStat;
                var bonusPick = Random.Range(0, 3);
                if(bonusPick == 0)
                {
                    WeaponBonus = WeaponBonusType.XP;
                    XPBonus = BonusStat;
                    BonusString = "XP x" + XPBonus.ToString("F1");
                }
                else if(bonusPick == 1)
                {
                    WeaponBonus = WeaponBonusType.HP;
                    HPBonus = BonusStat * 20;
                    BonusString = "+ " + HPBonus.ToString("F1") + "HP";
                }
                else if (bonusPick == 2)
                {
                    WeaponBonus = WeaponBonusType.Range;
                    RangeBonus = BonusStat - 1;
                    BonusString = "+ " + RangeBonus.ToString("F1") + " Range";
                }
                else if (bonusPick == 3)
                {
                    WeaponBonus = WeaponBonusType.Gold;
                    GoldBonus = BonusStat;
                    BonusString = "Gold x" + GoldBonus.ToString("F1");
                }
            }
        }
        else if (gameObject.GetComponent<Armour>())
        {
            gameObject.GetComponent<Armour>().HP += (gameObject.GetComponent<Armour>().Level * 2);
            BasePrice = gameObject.GetComponent<Armour>().HP * 3;
            LevelText.text = gameObject.GetComponent<Armour>().Level.ToString();
            var specialPick = Random.Range(0, 10);
            if (specialPick == 1)
            {
                Special = true;
                var specialStat = Random.Range(1, 10) * 0.1f;
                BonusStat = 1 + specialStat;
                var bonusPick = Random.Range(0, 4);
                if (bonusPick == 0)
                {
                    ArmourBonus = ArmourBonusType.Wand;
                    WandBonus = BonusStat;
                    BonusString = "Wand DMG x" + BonusStat.ToString("F1");
                }
                else if (bonusPick == 1)
                {
                    ArmourBonus = ArmourBonusType.Bow;
                    BowBonus = BonusStat;
                    BonusString = "Bow DMG x" + BonusStat.ToString("F1");
                }
                else if (bonusPick == 2)
                {
                    ArmourBonus = ArmourBonusType.Sword;
                    SwordBonus = BonusStat;
                    BonusString = "Sword DMG x" + BonusStat.ToString("F1");
                }
                else if (bonusPick == 3)
                {
                    ArmourBonus = ArmourBonusType.Club;
                    ClubBonus = BonusStat;
                    BonusString = "Club DMG x" + BonusStat.ToString("F1");
                }
            }
        }
        else if (gameObject.GetComponent<Consumable>())
        {
            var con = gameObject.GetComponent<Consumable>();
            if (con.Type == Consumable.ConsumableType.Potion)
            {
                con.Value = 20 + con.Level * 2;
                BasePrice = con.Value / 2;
                LevelText.text = con.Value.ToString();

            }
            else if (con.Type == Consumable.ConsumableType.Portal)
            {
                BasePrice = 100;
                LevelText.text = "";
            }
            else if (con.Type == Consumable.ConsumableType.Damage)
            {
                con.Value = con.Level + 1;
                BasePrice = 20 * con.Value;
                LevelText.text = con.Value.ToString();
            }
        }
        else
        {
            LevelText.text = " ";
        }
        stockMan = GameObject.FindObjectOfType<StockManager>();
        merch = GameObject.FindObjectOfType<MerchantMenu>();
        Price = Mathf.CeilToInt(BasePrice * PriceScale);
        PriceText.text = Price + "G";
        SellTime = Price;
        stockMan.UpdatePrices();
        var button = GetComponent<Button>();
        ColorBlock cb = button.colors;
        if (!Special)
        {
            cb.normalColor = NormalColour;
        }
        else
        {
            cb.normalColor = SpecialColour;
        }
        button.colors = cb;

        
    
}
    public void ReadyToSell()
    {
        Selling = false;
        Sold = true;
        CollectButton.SetActive(true);
        CollectButton.GetComponentInChildren<TextMeshProUGUI>().text = "SOLD\n" + Price + "G";
        if (stockMan == null)
        {
            stockMan = GameObject.FindObjectOfType<StockManager>();
        }
        stockMan.NewSale();
        StoreData();
    }

    public void PriceUI()
    {
        PriceText.text = Price + "G";
        if (gameObject.GetComponent<Weapon>())
        {
            LevelText.text = gameObject.GetComponent<Weapon>().Level.ToString();
        }
        else if (gameObject.GetComponent<Armour>())
        {
            LevelText.text = gameObject.GetComponent<Armour>().Level.ToString();
        }
        else if (gameObject.GetComponent<Consumable>())
        {
            var con = gameObject.GetComponent<Consumable>();
            if(con.Type == Consumable.ConsumableType.Potion)
            {
                LevelText.text = con.Value.ToString();
            }
            else if(con.Type == Consumable.ConsumableType.Portal)
            {
                LevelText.text = " ";
            }
            else if (con.Type == Consumable.ConsumableType.Damage)
            {
                LevelText.text = con.Value.ToString();
            }

        }
        var button = GetComponent<Button>();
        ColorBlock cb = button.colors;
        if (!Special)
        {
            cb.normalColor = NormalColour;
        }
        else
        {
            cb.normalColor = SpecialColour;
        }
        button.colors = cb;

    }

    public void StoreData()
    {
        var newItemData = new ItemData();
        newItemData.ItemName = ItemName;
        newItemData.BonusString = BonusString;
        newItemData.SpriteIndex = SpriteIndex;
        newItemData.TypeIndex = TypeIndex;
        newItemData.DamageType = DamageType;
        newItemData.Merchant = Merchant;
        newItemData.Equipped = Equipped;
        newItemData.Selling = Selling;
        newItemData.Sold = Sold;
        newItemData.Destroyed = Destroyed;
        newItemData.SellTimer = SellTimer;
        newItemData.Special = Special;
        newItemData.XPBonus = XPBonus;
        newItemData.RangeBonus = RangeBonus;
        newItemData.HPBonus = HPBonus;
        newItemData.GoldBonus = GoldBonus;
        newItemData.WandBonus = WandBonus;
        newItemData.SwordBonus = SwordBonus;
        newItemData.ClubBonus = ClubBonus;
        newItemData.BowBonus = BowBonus;
        newItemData.BonusStat = BonusStat;
        newItemData.ArmourBonus = (int)ArmourBonus;
        newItemData.WeaponBonus = (int)WeaponBonus;

        if(GetComponent<Weapon>() != null)
        {
            newItemData.ItemName = GetComponent<Weapon>().WeaponName;
            newItemData.Level = GetComponent<Weapon>().Level;
            newItemData.StatPoint = GetComponent<Weapon>().Damage;
            newItemData.ConsumableType = 0;
        }
        else if(GetComponent<Armour>() != null)
        {
            newItemData.Level = GetComponent<Armour>().Level;
            newItemData.StatPoint = GetComponent<Armour>().HP;
            newItemData.ConsumableType = 0;
        }
        else if (GetComponent<Consumable>() != null)
        {
            newItemData.Level = GetComponent<Consumable>().Level;
            newItemData.StatPoint = GetComponent<Consumable>().Value;
            if(GetComponent<Consumable>().Type == Consumable.ConsumableType.Potion)
            {
                newItemData.ConsumableType = 1;
            }
            else if (GetComponent<Consumable>().Type == Consumable.ConsumableType.Portal)
            {
                newItemData.ConsumableType = 2;
            }
            else if (GetComponent<Consumable>().Type == Consumable.ConsumableType.Damage)
            {
                newItemData.ConsumableType = 3;
            }
        }
        newItemData.BasePrice = BasePrice;
        if (Selling)
        {
            newItemData.BasePrice = Price;
        }
        Data = newItemData;
    }

    public void SetBonusTypes(int w, int a)
    {
        ArmourBonus = (ArmourBonusType)a;
        WeaponBonus = (WeaponBonusType)w;
    }

    
}
