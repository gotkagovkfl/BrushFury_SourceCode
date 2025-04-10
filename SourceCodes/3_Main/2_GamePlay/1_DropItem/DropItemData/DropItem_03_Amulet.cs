using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DropItem_003_Amulet", menuName = "SO/DropItem/003_Amulet", order = int.MaxValue)] 
public class DropItem_03_Amulet : DropItemDataSO
{
    public override string id => "003";

    public override string dataName => "부적";


    [SerializeField] PlayerProjectile prefab_explosion;

    
    public override void PickUp(DropItem di, float value)
    {
        Vector3 initPos = di.myTransform.position;
        PoolManager.Instance.playerProjectilePoolSys.GetPP(value, prefab_explosion,initPos, initPos);
    }
}