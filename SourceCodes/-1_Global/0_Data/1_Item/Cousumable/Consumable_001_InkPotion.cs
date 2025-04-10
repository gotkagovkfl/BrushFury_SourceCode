using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable_001_InkPotion : ConsumableItemSO
{
    protected override string id_detail => "001_InkPotion";
    public override string dataName => "RestoreHp";

    public override string description 
        => $"";
    public override void Consume(float value)
    {
        Player.Instance.GetHealed(value);
    }

    

}
