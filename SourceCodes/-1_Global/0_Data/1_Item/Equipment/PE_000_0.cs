using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PE_000_0", menuName = "SO/Equipment/000_0")]
public class PE_000_0 : EquipmentItemSO
{
    protected override string id_detail => "000_0";

    public override string dataName => "하급 생명력의 서";

    public override string description 
        => $"최대 체력 {amount} 증가";

    // protected override int ParamCount => 5;
    public int amount = 50;
    //

    public override void OnEquip(int stackCount)
    {
        Player player = Player.Instance;
        EntityStatusField targetField = player.status.maxHp;
        List<EntityStatusModifier> equipmentEffect = new(){ new(targetField, amount * stackCount) };
        foreach(EntityStatusModifier modifier in equipmentEffect)
        {
            modifier.Adjust();
        }

        player.GetHealed(amount);
    }

    public override void OnUnEquip(int stackCount)
    {
        Player player = Player.Instance;
        EntityStatusField targetField = player.status.maxHp;
        List<EntityStatusModifier> equipmentEffect =  new(){ new(targetField, amount * stackCount) };
        foreach(EntityStatusModifier modifier in equipmentEffect)
        {
            modifier.Undo();
        }
    }
}
