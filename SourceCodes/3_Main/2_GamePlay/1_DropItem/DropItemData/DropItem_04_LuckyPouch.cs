using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem_004_LuckyPouch", menuName = "SO/DropItem/004_LuckyPouch", order = int.MaxValue)] 
public class DropItem_04_LuckyPouch : DropItemDataSO
{
    public override string id => "004";

    public override string dataName => "복주머니";
    
    public override void PickUp(DropItem di, float value)
    {
        // 폭발 생성
        GamePlayManager.Instance.OnLevelUp();
    }
}
