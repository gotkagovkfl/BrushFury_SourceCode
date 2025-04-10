using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem_01_HpUp", menuName = "SO/DropItem/001", order = int.MaxValue)]
public class DropItem_01_HpUp : DropItemDataSO
{
    public override string id => "001";

    public override string dataName => "회복";
    
    public override void PickUp(DropItem di, float value)
    {
        Player.Instance.GetHealed(value);
    }
}
