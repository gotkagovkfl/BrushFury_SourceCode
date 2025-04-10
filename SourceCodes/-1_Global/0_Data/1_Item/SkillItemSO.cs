using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum SkillType
{
    BasicAttack,
    Unique,
    Scroll,
}

/// <summary>
/// 스킬은 장착 아이템이면서도, 사용 효과가 있는 아이템임. 
/// </summary>
public abstract class SkillItemSO : ItemDataSO
{
    public static readonly Dictionary<SkillType, string> typeKorDic = new(){ {SkillType.BasicAttack,"기본공격"},
                                                                            {SkillType.Unique,      "고유능력"},
                                                                            {SkillType.Scroll,      "두루마리"},
                                                                            };  
    
    [Header("Default Skill Setting")]
    public SkillType skillType;
    public int level;

    public abstract string id_base {get;}
    protected override string id_detail => $"{id_base}_{level}"; 

    // public SkillItemSO displayPrecondition;     //선행조건
    public override string typeKor => $"{typeKorDic[skillType]}";
    public bool hasCost;
    [SerializeField] protected float inkCost = 0f; 
    public float InkCost => inkCost;  // public getter 추가
    [Min(0.1f)] public float coolTime = 1;

    [Header("sfx")]
    public SoundSO sfx_use;



    public float baseDamage = 30f;
    public float coefficient_pDmg = 1f;

    
    //====================================================================
    public SkillItemSO()
    {
        type = ItemType.Skill;
    }



    protected override bool CanGet(out CantGetReason reason)
    {        
        reason = CantGetReason.None;

        return Player.Instance.skills.CanEquip( this);
    }


    protected override void Get()
    {
        Player.Instance.skills.Equip(this);
    }

    protected override void OnCantGet(CantGetReason reason)
    {
        switch(reason)
        {
            case CantGetReason.NoSpace:
                // GamePlayManager.Instance.OnInventoryFull(this);
                Debug.Log("장착할 스킬이 없음");
                break;
        }
    }



    // public void Equip()
    // {
        // OnEquip();  // 장착효과
    // }


    // public void UnEquip()
    // {
        //플레이어 장비창에서 사라짐. 획득 능력치는삭제

        // OnUnEquip(); // 장착효과 해제
    // }


    public abstract void OnEquip();
    public abstract void OnUnEquip();

    // public abstract void Use();

    // public abstract string 
    public abstract IEnumerator UseRoutine();
}
