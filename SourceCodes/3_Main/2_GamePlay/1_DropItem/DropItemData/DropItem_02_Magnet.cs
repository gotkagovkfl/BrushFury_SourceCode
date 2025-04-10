using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DropItem_002_Magnet", menuName = "SO/DropItem/002_Magnet", order = int.MaxValue)] 
public class DropItem_02_Magnet : DropItemDataSO
{
    public override string id => "002";

    public override string dataName => "자석";
    
    public override void PickUp(DropItem di, float value)
    {
        var exps = PoolManager.Instance.dropItemPoolSys.existingExps.GetTotalItems();

        foreach(var exp in exps)
        {
            exp.SetCaptured();
        }
    }
}
