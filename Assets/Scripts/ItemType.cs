using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemType : MonoBehaviour
{
    public enum ItemTypes
    {
        Sword,
        Club,
        Bow,
        Wand,
        Helm,
        Armour,
        Boots,
        Consumable,
        Ring,
    }
    public ItemTypes Type;
   
}
