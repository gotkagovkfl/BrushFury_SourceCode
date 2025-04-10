using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuyableItem", menuName = "SO/BuyableItem", order = int.MaxValue)]
public class BuyableItemSO : ScriptableObject
{
    public ItemDataSO baseData;
    
    public string id => baseData.id;
    public string dataName => baseData.dataName;
    public Sprite sprite => baseData.sprite;    

    public string description=>baseData.description;    

    //===========================================
    
    public int cost;

    //===========================================
}
