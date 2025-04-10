using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using DG.Tweening;

public class GamePlayStartUI : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] TextMeshProUGUI text_stageNum;
    [SerializeField] TextMeshProUGUI text_stageStart;
    // public float bootingTime;
    
    // public Sequence startSequence;


    public Sequence GetSeq_GamePlayStart(string stageNum)
    {
        gameObject.SetActive(true);
        
        text_stageNum.SetText( $"{stageNum} 단계");

        img.color = new Color(1,1,1,0);
        text_stageNum.color = new Color(1,1,1,0);
        text_stageStart.color = new Color(1,1,1,0);
        //

        //
        var ret = DOTween.Sequence()
        .OnComplete( ()=>{
            gameObject.SetActive(false);
        }) 
        .Append(img.DOFade(1f,0.5f))
        //
        .Append(text_stageNum.DOFade(1f,0.5f))
        .Join(text_stageStart.DOFade(1f,0.5f))
        .AppendCallback( ()=>SoundManager.Instance.Invoke(transform, SoundEventType.UI_GamePlayStart)   )
        //
        .AppendInterval(1f)
        //
        .Append(img.DOFade(0f,1f))
        .Join(text_stageStart.DOFade(0f,1f))
        .Join(text_stageNum.DOFade(0f,1f));
        //

        //
        return ret;
    }


    
}
