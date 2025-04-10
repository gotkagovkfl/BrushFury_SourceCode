using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageReward : InteractiveObject
{
    [SerializeField] SpriteRenderer sr_reward;
    // [SerializeField] TextMeshPro text;
    public PickableItemSO data;

    protected override string inspectingText => "받기";

    // protected override float TargetInteractingTime => 0;


    //========================================================================
    protected override void OnInspect_Custom(bool isOn)
    {
        // text.gameObject.SetActive(isOn);
    }

    protected override void OnInteract_Custom()
    {
        data.Acquire();
        Destroy(gameObject);
    }

    //========================================================================
    public void Init(PickableItemSO data)
    {
        this.data = data;
        sr_reward.sprite = data.sprite;
    }
}
