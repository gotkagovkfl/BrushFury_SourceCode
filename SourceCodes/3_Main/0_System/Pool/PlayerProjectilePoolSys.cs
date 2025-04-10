using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerProjectilePoolSys : BwPoolSystem
{
    
    /// <summary>
    ///  아이템 같이 스킬에서 나오는 게 아닌경우,
    /// </summary>
    public PlayerProjectile GetPP(float dmg, PlayerProjectile ppPrefab, Vector3 initPos, Vector3 targetPos)
    {
        PlayerProjectile po = Get<PlayerProjectile>( ppPrefab, initPos );
        po.Init(dmg,targetPos);

        return po;
    }
    
    /// <summary>
    /// 스킬에서 나오는 경우
    /// </summary>
    public PlayerProjectile GetPP(SkillItemSO skillData, PlayerProjectile ppPrefab, Vector3 initPos, Vector3 targetPos)
    {
        PlayerProjectile po = Get<PlayerProjectile>( ppPrefab, initPos );
        po.Init(skillData,targetPos);

        return po;
    }
    
    
    //=========================================================================================================================


    public T GetPP<T>(SkillItemSO skillData, PlayerProjectile po, Vector3 initPos, Vector3 targetPos)   where T : PlayerProjectile
    {
        return GetPP(skillData,po, initPos, targetPos) as T;
    }


    //======================================================
}
