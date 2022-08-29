using UnityEngine;
using System;
[Serializable]
public class ItemData
{
    public string ItemName;
    public int SpriteIndex;
    public float BasePrice;
    public int TypeIndex;
    public float DamageType;
    public float Level;
    public float StatPoint;
    public int ConsumableType;
    public bool Merchant;
    public bool Equipped;
    public bool Selling;
    public float SellTimer;
}