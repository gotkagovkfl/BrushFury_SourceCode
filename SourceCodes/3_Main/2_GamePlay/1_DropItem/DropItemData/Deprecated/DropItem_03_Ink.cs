using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem_03_Ink", menuName = "SO/DropItem/03", order = int.MaxValue)]
public class DropItem_03_Ink : DropItemDataSO
{
    public override string id => "003";

    public override string dataName => "잉크";
    
    public override void PickUp(DropItem di,float value)
    {
        Player.Instance.GetInk(value);
    }
}
