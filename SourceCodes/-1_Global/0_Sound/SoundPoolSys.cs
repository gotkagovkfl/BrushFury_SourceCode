using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPoolSys : BwPoolSystem
{
    [SerializeField] SFX prefab_sfx;


    public SFX GetSFX(SoundData soundData)
    {
        SFX sfx = Get<SFX>(prefab_sfx.poolId, soundData.pos);
        return sfx;
    }


    void OnValidate()
    {
        list_defaultPrefabs = new(){prefab_sfx};

    }
}
