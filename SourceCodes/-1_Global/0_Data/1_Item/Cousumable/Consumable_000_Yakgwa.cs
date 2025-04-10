using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PI_Yakgwa", menuName = "SO/Consumable/000")]
public class Consumable_000_Yakgwa : ConsumableItemSO
{
    protected override string id_detail => "000_Yakgwa";
    public override string dataName => "약과";
    // protected override int ParamCount => 1;
    public override string description 
        => $"체력 {amount} 회복";
    public int amount = 30;

    //
    // public override void EquipEvent( bool isEquip)
    // {
    //     if (isEquip)
    //     {
    //         // Debug.Log("약과 get");
    //         Player.Instance.GetHealed(amount);
    //     }   
    // }


    // public override List<EntityStatusModifier> GetEquipmentEffect(PlayerStatus playerStatus)
    // {
    //     List<EntityStatusModifier> ret = new();

    //     return ret;
    // }


    public override void Consume(float value)
    {
        Player.Instance.GetHealed(amount);
    }
}
