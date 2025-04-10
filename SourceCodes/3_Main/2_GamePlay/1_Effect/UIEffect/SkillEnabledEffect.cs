using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SkillEnabledEffect : GameEffect
{
    protected override string id => "SkillEnabledEffect";

    Image img;
    RectTransform r;

    Sequence seq;


    public override void OnCreatedInPool()
    {
        r = GetComponent<RectTransform>();
        img = GetComponent<Image>();   
    }

    public override void OnGettingFromPool()
    {
        r.localScale = Vector3.one;
        img.color = Color.white;    
    }

    protected override void Init_Custom()
    {
        PlaySeq_Fade();
    }

    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitUntil(()=> seq!=null);
        yield return seq.WaitForCompletion();
    }
    //=====================================================
    public void SetSprite(Sprite sprite)
    {
        img.sprite  = sprite;
    }



    //=====================================================

    void PlaySeq_Fade()
    {
        float displayTime = 0.3f;

        img.color = new Color(1,1,1,0.8f);
        seq =  DOTween.Sequence()
            .Append(myTransform.DOScale(2f,displayTime))
            .Join(img.DOFade(0, displayTime))
            .Play();
    }

}
