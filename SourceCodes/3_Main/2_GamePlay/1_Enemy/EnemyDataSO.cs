using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System;
using Spine.Unity;
using Spine;

/// <summary>
/// 공격 타입
/// </summary>
// public enum EnemyAttackType
// {
//     None,
//     Melee,
//     Range,
// }

/// <summary>
/// 적 등급
/// </summary>
// public enum EnemyRank
// {
//     Normal,
//     Elite
// }

/// <summary>
/// 적 타입
/// </summary>
// public enum EnemyType
// {
//     Ghost,
//     Beast
// }



public class EnemyDataSO : ScriptableObject
{
    public string id;           // 시스템 구분용 식별번호
    public string entityName;    // 개체 이름
    public Sprite sprite;       // 개체의 스프라이트
    public float size;       // 기준보다 몇배가 큰지.
    //
    public float maxHp = 50;
    public float armor;
    public float tenacity;
    public float retreatRange;  // 후퇴 사거리. ( 카이팅의 경우에만 적용됨. 혹은 공포 상태이상을 만든다 할 때, )
    public float ad = 20;           // 평타 뎀지 
    public float movementSpeed = 3;    // 이동속도

    public float exp = 30;   // 주는 경험치
    
    public List<EnemyAbilitySO> abilitiesOnDeath;   // 죽을 때 발동되는 스킬.  - 이 기술들은 즉시 사용됨. (시전시간 없음)
    public List<EnemyAbilitySO> abilities;  // 스킬이 있을 수도 있음.
    public EnemyMoveType moveType;

    [Space(30)]
    [Header("Animation")]
    // public RuntimeAnimatorController animator;
    // public AnimatorOverrideController abilityAnimator;
    // public AnimationClip defaultAnimation;

    //
    public AnimationReferenceAsset idleAnimaton;
    public AnimationReferenceAsset moveAnimaton;
    public AnimationReferenceAsset deathAnimaton;
    // public SerializableDictionary<EnemyAbilitySO, AnimationReferenceAsset> abilityAnimations;


    [Space(30)]
    [Header("Sound")]
    public SoundSO sfx_die;





    //==================================================

    void OnValidate()
    {

    }
}
