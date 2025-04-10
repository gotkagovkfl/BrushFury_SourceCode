using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemListUI : MonoBehaviour
{
    [SerializeField] PlayerItemUI item_basicAttack;
    [SerializeField] PlayerItemUI item_uniqueSkill;
    [SerializeField] PlayerItemUI item_scroll_0;

    [SerializeField] PlayerExtraItemCountUI playerExtraItemCount;


    //==================================================================================
    
    public void UpdateList()
    {
        //
        PlayerSkills playerSkills = Player.Instance.skills;
        item_basicAttack.Init(0, playerSkills.basicAttack.data);
        // item_uniqueSkill.Init(2, playerSkills.uniqueSkill.data);
        // item_scroll_0.Init(3, playerSkills.automaticSkills[SkillType.Scroll].data);         //
        //
        // PlayerEquipments playerEquipments = Player.Instance.equipments;
        // playerExtraItemCount.Init(playerEquipments.equipments.Count);
        
        //
        // playerExtraItemCount.gameObject.SetActive(false);       // 일단은 비활성화 - 1.18일 이후에 패시브 아이템 생기면 다시 킬거임. 

    }
}
