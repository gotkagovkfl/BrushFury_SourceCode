using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using BW;
using UnityEditor;
using UnityEngine;

// using Redcode.Pools;

// public enum EffectEvent
// {
//     EnemyHit,
//     PlayerHit,
//     Test
// }




public class EffectPoolSys : BwPoolSystem
{
    
    

    //============================================================================
    public T GetEffect<T>(string poolId, Vector3 initPos) where T : GameEffect
    {
        T po = Get<GameEffect>(poolId, initPos ) as T;
        po.Init();

        return po;
    }
    
    
    
    
    public GameEffect GetEffect(string poolId, Vector3 initPos)
    {
        GameEffect po = Get<GameEffect>(poolId, initPos );
        po.Init();

        return po;
    }

    public GameEffect GetEffect(GameEffect prefab, Vector3 initPos)
    {
        GameEffect po = Get<GameEffect>( prefab, initPos );
        po.Init();

        return po;
    }






    public GameEffect GetEffect(string poolId, Vector3 initPos, Vector3 dir)
    {
        GameEffect po = Get<GameEffect>( poolId, initPos );
        po.Init();
        po.SetDirection(dir);

        return po;
    }


    public GameEffect GetEffect(GameEffect prefab, Vector3 initPos, Vector3 dir)
    {
        GameEffect po = Get<GameEffect>( prefab, initPos );
        po.Init();
        po.SetDirection(dir);

        return po;
    }



    //=============================================================================================

    public GameEffect GetEnemyHitEffect(Vector3 lastHitPoint)
    {
        return GetEffect($"{PoolType.Effect}_EnemyHit", lastHitPoint);
    }

    public GameEffect GetDamageText(float dmg, Vector3 initPos, DamageType damageType = DamageType.DMG_NORMAL)
    {
        DamageText te = GetEffect($"{PoolType.Effect}_TextEffect", initPos) as DamageText;
        te.Init(dmg,damageType);

        return te;
    }

    public GameEffect GetTextEffect(string content, Vector3 initPos)
    {
        DamageText te = GetEffect($"{PoolType.Effect}_TextEffect", initPos) as DamageText;
        te.SetText(content);

        return te;
    }

}
