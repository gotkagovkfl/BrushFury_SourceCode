using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class StagePortal : InteractiveObject
{
    // [SerializeField] TextMeshPro text;
    [SerializeField] SpriteRenderer sr_reward;


    // public override bool hasSecondaryInteraction => false;

    public string stageNodeId;

    protected override string inspectingText => "진입";
    // protected override float TargetInteractingTime => 0;


    //========================================================================
    protected override void OnInspect_Custom(bool isOn)
    {
        
    }

    protected override void OnInteract_Custom()
    {
        Debug.Log("포탈 진입");

        //
        GoToNextStage();    
    }

    // protected override void OnSecondaryInteract_Custom()
    // {
        
    // }

    //========================================================================

    public void Init(string id)
    {
        stageNodeId = id;
        // if(GameManager.Instance.playerData.savedNodeData.TryGetNodeInfo(id, out StageNode stageNode) )
        // {
        //     sr_reward.sprite = stageNode.thumbnail;
        // }

        
        gameObject.SetActive(true);
        Activate();
    }

    public void OnStageClear()
    {

    }

    
    void GoToNextStage()
    {
        SoundManager.Instance.Invoke(transform, SoundEventType.Portal);
        
        // GameManager.Instance.playerData.OnStageClear(Player.Instance, stageNodeId);  // 데이터 저장.
        

        SceneLoadManager.Instance.Load_MainScene();
    }
    

    // public Sequence GetSeq_GeneratePortal(float playTime)
    // {
    //     var sr = GetComponent<SpriteRenderer>();
    //     sr.color = new Color(1,1,1,0);

    //     return DOTween.Sequence()
    //     .Append( sr.DOFade(1f, playTime));
    // }

    // public Sequence PlaySeq_DestroyPortal(float playTime)
    // {
    //     var sr = GetComponent<SpriteRenderer>();
    //     sr.color = Color.white;

    //     return DOTween.Sequence()
    //     .OnComplete( ()=>{Destroy(gameObject);})
    //     .Append( sr.DOFade(0f, playTime))
    //     .Play();
    // }
}
