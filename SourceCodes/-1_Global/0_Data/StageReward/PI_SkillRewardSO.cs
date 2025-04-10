using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "SkillReward", menuName = "SO/StageReward/Skill")]
public class PI_SkillRewardSO : PickableItemSO 
{
    public int rank;
    // public SkillProperty skillProperty;
    
    public override string id => $"002";

    public override string dataName => $"스킬 보상";


    public override void Acquire()
    {
        // GamePlayManager.Instance.OpenSkillUpgradePanel(skillProperty);
    }

}
