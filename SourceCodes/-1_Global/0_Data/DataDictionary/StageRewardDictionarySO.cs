using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "StageRewardDictionary", menuName = "SO/Dictionary/StageReward", order = int.MaxValue)]
public class StageRewardDictionarySO : DataDictionarySO
{
    public bool TryGetRandomSkillReward(out PickableItemSO skillReward)
    {
        skillReward = null;

        List<PickableItemSO> totalSkillRewards = list.OfType<PickableItemSO>().OrderBy(x => random.Next()).ToList();

        if (totalSkillRewards.Count>0)
        {
            skillReward = totalSkillRewards[0];
            return true;
        }
        
        return false;
    }

    public bool TryGetStatusPointReward(int amount, out PI_StatusPointRewardSO statusPointReward)
    {
        statusPointReward  = list.OfType<PI_StatusPointRewardSO>().SingleOrDefault(x => x.amount == amount);
        if (statusPointReward != null)
        {
            return true;
        }
        
        return false;
    }
}
