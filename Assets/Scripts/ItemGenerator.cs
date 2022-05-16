using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public StockManager Stock;
    public Transform StockList;
    public List<GameObject> SwordPrefabs;
    public List<GameObject> ClubPrefabs;
    public List<GameObject> BowPrefabs;
    public List<GameObject> WandPrefabs;
    public List<GameObject> ArmourPrefabs;
    public List<GameObject> HelmPrefabs;
    public List<GameObject> BootsPrefabs;
    public List<GameObject> ConsumablePrefabs;

    public void GenerateWeapon(int l)
    {
        var pick = Random.Range(0, 4);
        if(pick == 0)
        {
            CreateSword(l);
        }
        else if(pick == 1)
        {
            CreateClub(l);
        }
        else if (pick == 2)
        {
            CreateBow(l);
        }
        else if (pick == 3)
        {
            CreateWand(l);
        }

    }

    public void GenerateArmour(int l)
    {

    }

    public void GenerateConsumable()
    {

    }

    public void CreateSword(int l)
    {
        Debug.Log("Created New Sword");
        var pick = Random.Range(0, SwordPrefabs.Count);
        var newSword = Instantiate(SwordPrefabs[pick], StockList);
        newSword.GetComponent<Weapon>().Level = l;

    }

    public void CreateClub(int l)
    {
        Debug.Log("Created New Club");
        var pick = Random.Range(0, ClubPrefabs.Count);
        var newClub = Instantiate(ClubPrefabs[pick], StockList);
        newClub.GetComponent<Weapon>().Level = l;

    }
    public void CreateBow(int l)
    {
        Debug.Log("Created New Bow");
        var pick = Random.Range(0, BowPrefabs.Count);
        var newBow = Instantiate(BowPrefabs[pick], StockList);
        newBow.GetComponent<Weapon>().Level = l;
    }
    public void CreateWand(int l)
    {
        Debug.Log("Created New Wand");
        var pick = Random.Range(0, WandPrefabs.Count);
        var newWand = Instantiate(WandPrefabs[pick], StockList);
        newWand.GetComponent<Weapon>().Level = l;
    }

}
