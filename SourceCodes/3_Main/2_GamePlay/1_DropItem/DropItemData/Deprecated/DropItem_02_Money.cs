using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem_02_Money", menuName = "SO/DropItem/02", order = int.MaxValue)]
public class DropItem_02_Money : DropItemDataSO
{
    public override string id => "002";

    public override string dataName => "금화";
    
    public override void PickUp(DropItem di, float value)
    {
        Player.Instance.status.GetGold((int)value);
    }
}
