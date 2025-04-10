using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class PlayerSkill : PlayerItem<SkillItemSO>
{    
    // public SkillItemSO data;
    
    public float lastUseTime;
    public float coolTimeRemain =>  data.coolTime  - (Time.time - lastUseTime);

    public bool isUsing;

    //Events
    public Action<PlayerSkill> onInit;  // 스킬 교체될 때, 
    public Action<PlayerSkill> onUse;   // 스킬 사용될 때,

    //=================================================================
    
    
    // public PlayerSkill(SkillItemSO skillData)
    // {
    //     Init(skillData);
    // }

    public void UnEquip()
    {
        if( data != null)
        {
            data.OnUnEquip();
            data=null;
        }
    }

    public void Init(SkillItemSO skillData)
    {
        if ( skillData == null)
        {
            Debug.Log($"스킬 장착 실패! 데이터가 null");
            return;
        }
        
        data?.OnUnEquip();
        //
        data = skillData;
        if(data!=null)
        {
            data.OnEquip();
            lastUseTime = Time.time - skillData.coolTime +0.5f;
        }

        onInit?.Invoke(this);   // 이벤트 실행 
    }

    public IEnumerator UseRoutine()
    {
        if (data == null)        
        {
            yield break;
        }
        onUse?.Invoke(this);         // 이벤트 실행
        isUsing = true;
        // 잉크가 충분할 때만 스킬 실행 및 잉크 소모
        Player.Instance.UseInk(data.InkCost);

        SoundManager.Instance.Play(Player.Instance.t, data.sfx_use);
        yield return data.UseRoutine();
        isUsing = false;
        lastUseTime = Time.time;
    }
    
    public bool CanUse()
    {
        if (activated==false)
        {
            return false;
        }
        
        bool hasCooldown = coolTimeRemain  > 0;
        bool hasEnoughInk = Player.Instance.status.currInk >=  data.InkCost;
        
        return isUsing == false && hasCooldown == false && hasEnoughInk;
    }
}
