using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DropItem_000_Exp", menuName = "SO/DropItem/000", order = int.MaxValue)] 
public class DropItem_00_Exp : DropItemDataSO
{
    public override string id => "000";

    public override string dataName => "경험치";

    public override void PickUp(DropItem di,float value)
    {
        Player.Instance.GetExp(value);
    }
}
