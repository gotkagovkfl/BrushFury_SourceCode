using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public abstract class EquipmentItemSO : ItemDataSO
{
    // protected abstract int ParamCount {get;}  // 아이템 효과에 사용되는 
    public int maxStackCount = 5;
    public override string typeKor => "능력치";


    public EquipmentItemSO()
    {
        type = ItemType.Equipment;
    }

    protected override bool CanGet(out CantGetReason reason)
    {        
        reason = CantGetReason.None;
        
        // if (Player.Instance.equipments.HasEmptySpace()==false)
        // {
        //     reason = CantGetReason.NoSpace;
        // }


        return reason == CantGetReason.None;
    }

    protected override void Get()
    {
        
        Player.Instance.skills.Equip( this );
    }

    protected override void OnCantGet(CantGetReason reason)
    {
        // switch(reason)
        // {
            // case CantGetReason.NoSpace:
                // GamePlayManager.Instance.OnInventoryFull(this);
                // break;
        // }
    }

    //=====================================
    /// <summary>
    /// 장착 효과 없이 장비 장착
    /// </summary>
    // public void InitEquip()
    // {
    //     OnEquip();
        
    // }

    // public void Equip()
    // {
    //     OnEquip();  // 장착효과
    //     EquipEvent(true);
    // }


    // public void UnEquip()
    // {
    //     //플레이어 장비창에서 사라짐. 획득 능력치는삭제
    //     OnUnEquip(); // 장착효과 해제
    //     EquipEvent(false);
    // }
    //=====================================

    public abstract void OnEquip(int stackCount);
    public abstract void OnUnEquip(int stackCount);

    // public abstract void EquipEvent(bool isEquip);

    // public abstract List<EntityStatusModifier> GetEquipmentEffect(PlayerStatus playerStatus);
}
