using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;


public enum DamageType
{
    DMG_NORMAL,
    DMG_CRITICAL,
    DMG_TICK,
    DMG_PLAYER,
    HEAL_PLAYER
}

public class DamageText : GameEffect
{
    protected override string id => "TextEffect";
    TextMeshPro text;
    // private static float lastYOffset = 0f;
    // private static float yOffsetIncrement = 1.0f;
    // private static float resetTime = 0.5f;
    // private static float lastTextTime;   
    // private static int activeTextCount = 0;

    // 등장 딜레이
    // private const float APPEAR_DELAY_BASE = 0.05f;

    // Fade In 시간
    private const float FADE_IN_TIME = 0.15f;

    // 표시 지속 시간
    private const float DISPLAY_TIME_BASE = 0.3f;

    // Fade Out 시간
    private const float FADE_OUT_TIME = 0.3f;

    // 위로 올라가는 거리 (위로 쌓임)
    private const float MOVE_UP_DISTANCE = 1f;

    Sequence seq_text;


    public override void OnCreatedInPool()
    {
        text = GetComponent<TextMeshPro>();
        text.color = new Color(1, 1, 1, 1);
    }

    public override void OnGettingFromPool()
    {
        text.color = Color.white;
        seq_text = null;
    }



    public void SetText(string content, DamageType type = DamageType.DMG_NORMAL)
    {
        // transform.position = hitPoint ;

        text.SetText(content);
        Color textColor = Color.white;
        switch (type)
        {
            case DamageType.DMG_CRITICAL:
                textColor = new Color(1, 1, 0.5f);
                break;
            case DamageType.DMG_PLAYER:
                textColor = new Color(1, 0.2f, 0.2f);
                break;
            case DamageType.HEAL_PLAYER:
                textColor = new Color(0, 1, 0.5f);
                break;
        }

        float displayTime = DISPLAY_TIME_BASE ;
        PlayAnim_MoveAndFade( displayTime, textColor);
    }

    public void Init(float damage, DamageType type = DamageType.DMG_NORMAL)
    {
        SetText( damage.ToString("0"), type);
    }

    void PlayAnim_MoveAndFade(float displayTime, Color textColor)
    {
        
        text.color = textColor;
        seq_text =  DOTween.Sequence()
            // .AppendInterval(appearDelay)
            .Append(transform.DOScale(1.5f,0.15f))
            .Append(transform.DOScale(1f,0.15f))
            .AppendInterval(displayTime)
            // .Append(transform.DOLocalMoveY(transform.localPosition.y + MOVE_UP_DISTANCE, FADE_OUT_TIME))
            .Join(text.DOFade(0, FADE_OUT_TIME))
            .Play();
    }

    protected override void Init_Custom()
    {
        
    }

    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitUntil(()=> seq_text!=null);
        yield return seq_text.WaitForCompletion();
    }
}
