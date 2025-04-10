using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class PlayerAbilityInfo : MonoBehaviour
{
    PlayerSkill targetPlayerSkill;
    
    // [SerializeField] Image img_availEffect;
    [SerializeField] Image img_iconBase;
    [SerializeField] Image img_cooltime;

    //
    bool isAvailabilityUpdated;
    Sequence seq_use;


    [Header("Effect")]

    [SerializeField] GameEffect effect_onEnabled;

    //=======================================================================
    void Update()
    {
        if( targetPlayerSkill ==null )
        {
            return;
        }
        if( targetPlayerSkill.activated == false)
        {
            return;
        }
        
        // 사용가능
        if( targetPlayerSkill.CanUse())
        {
            img_cooltime.fillAmount = 0;

            if(isAvailabilityUpdated == false)
            {
                isAvailabilityUpdated = true;

                var effect = PoolManager.Instance.effectPoolSys.GetEffect<SkillEnabledEffect>(effect_onEnabled.poolId, transform.position );
                effect.SetSprite(img_iconBase.sprite);
                effect.myTransform.SetParent(transform);
                
            }
        }
        else
        {
            if (isAvailabilityUpdated )
            {
                isAvailabilityUpdated = false;
            }

            // 사용중
            if( targetPlayerSkill.isUsing )
            {
                img_cooltime.fillAmount = 1;
            }
            // 쿨타임 도는 중
            else
            {
                float cooltimeRemain = targetPlayerSkill.coolTimeRemain;
                float cooltime = targetPlayerSkill.data.coolTime;

                float ratio = cooltimeRemain / cooltime;

                img_cooltime.fillAmount = ratio;
            }
        }

    }


    public void Init(PlayerSkill playerSkill)
    {
        targetPlayerSkill = playerSkill;
        targetPlayerSkill.onInit += SetIcon;
        targetPlayerSkill.onUse += PlaySeq_Use;

        SetIcon(targetPlayerSkill);


        PoolManager.Instance.effectPoolSys.AddPoolItem(effect_onEnabled);
    }

    void SetIcon(PlayerSkill playerSkill)
    {
        img_iconBase.sprite = targetPlayerSkill.data?.sprite;

        img_iconBase.gameObject.SetActive( playerSkill.activated );
    }


    void PlaySeq_Use(PlayerSkill playerSkill)
    {
        if (seq_use!=null && seq_use.IsActive())
        {
            seq_use.Kill();
        }

        Transform t_iconBase = img_iconBase.transform;
        t_iconBase.localScale = Vector3.one;

        seq_use = DOTween.Sequence()
        .Append(t_iconBase.DOScale(0.9f,0.2f))
        .Append(t_iconBase.DOScale(1f,0.2f))
        .Play();
    }
}
