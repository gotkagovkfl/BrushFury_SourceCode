using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Burst.CompilerServices;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerSkills : MonoBehaviour
{

    public PlayerSkill basicAttack;
    public PlayerSkill uniqueSkill;
    
    

    public List<PlayerSkill> automaticSkills;  
    public List<PlayerEquipment> passives;
    //

    //=======================================================================================================
    // 데이터 상의 모든 스킬장착
    public void Init(CharacterDataSO characterData)
    {
        // 컨테이너 초기화.
        basicAttack = new();
        uniqueSkill = new();
        automaticSkills = Enumerable.Range(0,GameConfig.maxAutomaticSkillCount) .Select(_=>new PlayerSkill()).ToList();
        passives        = Enumerable.Range(0,GameConfig.maxPassiveCount)        .Select(_=>new PlayerEquipment()).ToList();

  
        // 기본 캐릭터 스킬 장착 
        Equip( characterData.basicAttackData[0]);
        Equip( characterData.uniqueAbilityData[0]);    
        // Equip(automaticSkills[0], testSkillData );
    }

    // 
    public void Equip(ItemDataSO itemData)
    {

        // 
        if( itemData.type == ItemType.Skill)
        {
            SkillItemSO skillData = (SkillItemSO)itemData;
            EquipSkill(skillData);
            
        }
        else if( itemData.type == ItemType.Equipment)
        {
            EquipmentItemSO passiveData = (EquipmentItemSO)itemData;
            EquipPassive( passiveData );
        }
    }


   // 평타 아이템 획득시, 장착
    void EquipSkill( SkillItemSO skillData )
    {
        List<PlayerSkill> slots = new();
        

        if (skillData.skillType == SkillType.BasicAttack)
        {
            slots.Add(basicAttack);
        }
        else if (skillData.skillType == SkillType.Unique)
        {
            slots.Add( uniqueSkill);
        }
        else if (skillData.skillType == SkillType.Scroll)
        {
            slots = GetEquipSlot_Automatic(skillData);
        }

        // 
        if ( slots !=null )
        {   
            for(int i=0;i<slots.Count;i++)
            {
                if (i==0)   // 보통의 경우. 
                {
                    slots[i]?.Init(skillData);
                }
                else        // 스킬들이 조합되는 경우. 첫번재 스킬은 교체하고, 나머지 스킬은 비운다.  
                {
                    slots[i]?.UnEquip();
                }
            }
        }
    }

    void EquipPassive(EquipmentItemSO equipmentData)
    {
        // slot.Init( equipmentData );
        List<PlayerEquipment> slots = GetEquipSlot_Passive(equipmentData);
        // 
        if ( slots !=null )
        {   
            for(int i=0;i<slots.Count;i++)
            {
                if (i==0)   // 보통의 경우. 
                {
                    slots[i]?.Init(equipmentData);
                }
                else        // 스킬들이 조합되는 경우. 첫번재 스킬은 교체하고, 나머지 스킬은 비운다.  
                {
                    slots[i]?.UnEquip();
                }
            }
        }
    }



    //===========================================
    public bool CanEquip(ItemDataSO itemData)
    {
        bool ret = false;
        if( itemData.type == ItemType.Skill)
        {
            if(itemData.hasDisplayCondition)
            {
                ret = itemData.displayCondition.All( x => Has(x));   // 모든 요구사항을 만족해야함. 
            }
            else
            {
                ret = HasEmptySpace(itemData);
            }
        }
        else if (itemData.type==ItemType.Equipment)
        {
            EquipmentItemSO passiveData = (EquipmentItemSO) itemData;
            ret = GetEquipSlot_Passive(passiveData).Count > 0;
        }
        return ret;
    }



    public bool Has(ItemDataSO itemData)
    { 
        bool cb = basicAttack.data == itemData;
        bool cu = uniqueSkill.data == itemData;
        bool ca = automaticSkills.Any(x=> x.data == itemData);
        bool cp = passives.Any(x=> x.data == itemData);

        
        return cb||cu||ca||cp;
    }


    List<PlayerSkill> GetEquipSlot_Automatic(SkillItemSO skillData)
    {
        List<PlayerSkill> ret = new();
        
        //선행조건이 있으면,
        if (skillData.hasDisplayCondition)
        {
            ret =  automaticSkills.Where(x => skillData.displayCondition.Contains(x.data)).ToList(); // 스킬+패시브 말고 스킬+스킬의 경우도 처리하기위함
        }
        // 선행조건이 없는 경우 (거의 1렙 스킬)
        else
        {
            // 이미 스킬 칸에 해당 스킬이 없는 경우에만. 
            if ( automaticSkills.Where(x=>x.activated).All(x=> x.data.id_base.Equals(skillData.id_base)==false))
            {
                ret = automaticSkills.Where(x=>x.activated==false).Take(1).ToList();    // find 쓰면 null 떠서 귀찮아서 이렇게함. 
            }
        } 

        return ret;
    }
    List<PlayerEquipment> GetEquipSlot_Passive(EquipmentItemSO passiveData)
    {
        List<PlayerEquipment> ret = new();

        //선행조건이 있으면,
        if (passiveData.hasDisplayCondition)
        {
            ret =  passives.Where(x => passiveData.displayCondition.Contains(x.data)).ToList(); // 스킬+패시브 말고 스킬+스킬의 경우도 처리하기위함
        }
        // 선행조건이 없는 경우엔 빈 칸 암거나, 혹은 최대 중첩이 안된 같은 데이터가 든 슬롯. 
        else
        {
            List<PlayerEquipment> slots = passives.Where(x=> x.data == passiveData).ToList();
            if (slots.Count>0)
            {
                if(slots[0].currStackCount < passiveData.maxStackCount)
                {
                    ret = slots;
                }
            }
            else
            {
                ret = passives.Where(x=> x.activated == false).Take(1).ToList();
            }
        } 

        return ret;
    }

    /// <summary>
    /// 입력을 감지하고, 그에 맞는 스킬 실행
    /// </summary>
    public void OnUpdate()
    {
        if( Player.Instance.status.uncastable == false)
        {
            // 자동 시전 기술
            foreach( var skill in automaticSkills)
            {
                TryUse(skill);
            }
            
            
            // 고유기술 
            if (PlayerInputManager.Instance.uniqueSkill)
            {
                TryUse(uniqueSkill);
            }
        }

        if (PlayerInputManager.Instance.basicAttack 
        && Player.Instance.status.disarmed==false)
        {
            TryUse(basicAttack);
        }
    }





    void TryUse(PlayerSkill skill)
    {
        if (skill.CanUse())
        {
            StartCoroutine(skill.UseRoutine());
        }

    }

    //==============================================================
    bool HasEmptySpace(ItemDataSO itemData)
    {
        if ( itemData.type == ItemType.Skill)
        {
            SkillItemSO skillData = (SkillItemSO)itemData;
            SkillType skillType = skillData.skillType;
            if( skillType == SkillType.BasicAttack || skillType == SkillType.Unique)
            {
                return false;
            }
        
            return automaticSkills.Any(x=> x.activated==false) 
                    && automaticSkills.Where(x=>x.activated).All(x=> x.data.id_base.Equals(skillData.id_base)==false);  // 같은 무기가 있는지. 
        }
        else if ( itemData.type == ItemType.Equipment)
        {
            EquipmentItemSO passiveData = (EquipmentItemSO)itemData;
            return GetEquipSlot_Passive(passiveData).Count > 0;
        }
        return false;
    }

}
