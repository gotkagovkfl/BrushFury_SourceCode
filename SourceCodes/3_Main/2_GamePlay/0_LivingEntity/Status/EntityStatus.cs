using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[Serializable]
public class EntityStatusModifier
{
    EntityStatusField targetField;
    float modifierValue;

    public EntityStatusModifier(EntityStatusField field, float value)
    {
        targetField = field;
        modifierValue = value;
    }

    public void Adjust()
    {
        targetField.value += modifierValue;

        Debug.Log($"{targetField.name} +{modifierValue}");
    }
    
    public void Undo()
    {
        targetField.value -= modifierValue;
    }
}

[Serializable]
public class EntityStatusField
{
    public string name;
    public float value;
    public Action<float> onChange;      // 값 변화시 이벤트 

    public EntityStatusField()
    {
        value = 0;
    }
    public EntityStatusField(string name, float value)
    {
        this.name = name;
        this.value = value;

    }
}


public class EntityStatus 
{
   //                        
    #region 기본값
    [SerializeField] protected float d_movementSpeed;    // 이동속도
    [SerializeField] protected float d_maxHp;           // 최대체력

    #endregion
    //
    #region 수정값

    [SerializeField] float _currHp;
    public float currHp  // 현재체력
    {
        get => _currHp;
        set
        {
            _currHp = Math.Clamp(value, 0, maxHp);
        }
    }

    public float Inc_maxHp;

    public float armor;
    public float evasionRate;
    public float critRate;
    public float critDamageMultiplier = 1.5f;
    public float movementSpeedMultiplier = 1f;
    public float rangeModifier = 1;
    public float cooltimeModifier = 1;
    
    
    public float pDmg;  // physical dmg;
    public float mDmg;  // magical dmg;

    #endregion









    #region 최종값

    public float maxHp => d_maxHp + Inc_maxHp;
    public float movementSpeed => d_movementSpeed * movementSpeedMultiplier;
    
    #endregion








    #region 상태이상
    public float duration_invincible = 1f;  //0.5 의 배수 권장.
    public Vector3 forcedMoveVelocity;  // 강제로 플레이어를 이동시키는 힘

    public bool isStunned => immobilized && disarmed && uncastable;

    public bool invincible => stack_invincible >0;
    public bool immobilized => stack_immobilized>0;
    public bool disarmed => stack_disarmed>0;
    public bool uncastable => stack_uncastable>0;

    public int stack_invincible; // 무적
    public int stack_immobilized; // 이동불가
    public int stack_disarmed;   // 무장해제 ( 기본 공격 불가 )
    public int stack_uncastable; // 시전불가 ( 그리기, 유틸, 두루마리 사용불가 )

    


    #endregion 

    //===========================================================================================
    public EntityStatus()
    {
        currHp = maxHp;
    }

    //=================================================================================================
    #region 데이터 수정 

    
    public void OnInit(ref float field, float amount, bool isIncreasing)
    {
        ChangeStatus(ref field, amount, isIncreasing);
    }


    public void ChangeStatus(ref float field, float amount, bool isIncreasing)
    {
        if (isIncreasing)
        {
            field += amount;
        }
        else
        {
            field -= amount;
        }
    }

    #endregion

}
