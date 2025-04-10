using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Spine.Unity;
using System.Linq;
// using UnityEditor.Animations;
using UnityEngine;
using BW;

[CreateAssetMenu(fileName = "CharacterData", menuName = "SO/Character")]
public class CharacterDataSO : ScriptableObject
{
    [Header("Default")]
    public string characterName;
    // public Sprite sprite;
    // public AnimatorController animatorController;       // 애니메이션
    
    //
    [Space(50)]
    [Header("init Status")]
    public float initMaxHp = 100f;
    public float initMaxInk = 100f;
    public float initMovementSpeed = 10f;
    public float initPickupRange = 4f;

    // 일단 기본 능력치는 이정도만 가져가고, 나중에 캐릭터 별로 특성 다르게 가져갈 때 추가.

    [Space(50)]
    [Header("Ability")]
    // public BasicAttackSO basicAttackData;  // 기본공격
    // public SkillItemSO uniqueAbility;       // 고유능력 

    public List<BasicAttackSO> basicAttackData;
    public List<SkillItemSO> uniqueAbilityData;


    [Space(50)]
    [Header("Animations")]
    public SerializableDictionary<PlayerState, AnimationReferenceAsset> animations; // 스파인 애니메이션 


    //================

    void OnValidate()
    {
       basicAttackData.EnsureListSize(6);
       uniqueAbilityData.EnsureListSize(6);
    }


}
