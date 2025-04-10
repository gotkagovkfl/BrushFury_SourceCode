using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public abstract class DropItemDataSO : GameData
{

    public SoundSO sfx_pickup;

    public GameEffect prefab_pickupEffect;

    public abstract void PickUp(DropItem di, float value);
}
