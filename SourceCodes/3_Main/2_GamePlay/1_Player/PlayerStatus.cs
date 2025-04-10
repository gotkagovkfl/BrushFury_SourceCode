using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[System.Serializable]
public class PlayerStatus
{
    //                          0   1    2   3   4   5   6   7   8   9
    // int[] expIncrementTable = {100,0, 10, 30, 60, 60, 60, 60, 60, 60};

    // int[] lowLevelMaxExpTable = {0, 100, 110,140, 200};

    public EntityStatusField level= new("레벨",0);
    public EntityStatusField currExp = new("현재경험치",0);
    public EntityStatusField maxExp = new("필요경험치",0);

    public bool canLevelUp => currExp.value >= maxExp.value;

    public EntityStatusField selectableOptionCount = new("선택지수",3);


    #region 기본값
    public EntityStatusField maxHp = new("최대체력",0);
    public EntityStatusField maxInk = new("먹물량",0);
    public EntityStatusField movementSpeed = new("이동속도",0);    // 이동속도
    public EntityStatusField pickupRange = new("획득반경",2.5f);       // 아이템 획득 범위


    #endregion
    //
    #region 수정값
    public EntityStatusField expMultiplier = new("경험치획득율",100);


    [SerializeField] EntityStatusField _currHp =new("현재체력",0);
    public float currHp  // 현재체력
    {
        get => _currHp.value;
        set
        {
            _currHp.value = Math.Clamp( value, 0, maxHp.value);
        }
    }


    [SerializeField] EntityStatusField _currInk=new("현재먹량",0);
    public float currInk
    {
        get => _currInk.value;
        set
        {
            _currInk.value = Math.Clamp(value, 0, maxInk.value);
        }
    }

    

    public EntityStatusField armor =new();
    public EntityStatusField evasionRate =new();
    public EntityStatusField critRate=new();
    public EntityStatusField critDamageMultiplier = new("",1.5f);
    public EntityStatusField movementSpeedMultiplier = new("이동속도",100);  
    public EntityStatusField rangeModifier = new("",1f);
    public EntityStatusField cooltimeModifier = new("",1f);
    public EntityStatusField luck=new();

    public EntityStatusField inkRecoveryRate = new("먹회복률", 5f);            // 먹회복률 - 평타때릴때마다 회복량임. 


    // public int rerollCount;
    // public int selectionCount;

    public int gold; 
    public int statusUpgradePoint;


    public EntityStatusField pDmg = new("공격력",0); // physical dmg;
    public EntityStatusField pDmgMultiplier  = new("공격력배율", 100);

    public float GetFinalPDmg(float inputDmg, float weight)
    {
        return (inputDmg + pDmg.value * weight) * pDmgMultiplier.value*0.01f; 
    }

    #endregion




    #region 최종값
    public float finalMovementSpeed => movementSpeed.value  * movementSpeedMultiplier.value *0.01f;
    public float finalPickUpRange => pickupRange.value  * rangeModifier.value;

    public float inkRecoveryAmount => maxInk.value * inkRecoveryRate.value *0.01f;


    #endregion

    #region 상태이상
    public float duration_invincible = 1f;  //0.5 의 배수 권장.
    public Vector3 forcedMoveVelocity;  // 강제로 플레이어를 이동시키는 힘

    public bool isStunned => immobilized && disarmed && uncastable;

    public bool invincible => stack_invincible >0;
    public bool immobilized => stack_immobilized>0;
    public bool disarmed => stack_disarmed>0;
    public bool uncastable => stack_uncastable>0;
    // public bool shiftUnavailable => stack_shiftUnavailable>0;

    public int stack_invincible; // 무적
    public int stack_immobilized; // 이동불가
    public int stack_disarmed;   // 무장해제 ( 기본 공격 불가 )
    public int stack_uncastable; // 시전불가 ( 그리기, 유틸, 두루마리 사용불가 )
    // public int stack_shiftUnavailable;

    


    #endregion 



    public PlayerStatus()
    {
        currHp = maxHp.value;
        currInk = maxInk.value;

    }


    public PlayerStatus(UserDataSO userData, CharacterDataSO characterData) :base()
    {
        maxHp = new("최대체력",characterData.initMaxHp);
        maxInk = new("최대먹량",characterData.initMaxInk);
        movementSpeed = new("이동속도",characterData.initMovementSpeed);
        pickupRange = new("획득반경", characterData.initPickupRange);
        

        currHp = maxHp.value;
        currInk = maxInk.value;

        
        level= new("레벨",0);
        currExp = new("현재경험치",0);
        maxExp = new("필요경험치", 0);

        selectableOptionCount = new("선택지수",3);
        
        SetMaxExp();
    }


    // public PlayerStatus(PlayerStatus savedStatus)
    // {
    //     currHp = savedStatus.currHp;
    //     currInk = savedStatus.currInk;
    //     rerollCount = savedStatus.rerollCount;
    //     selectionCount = savedStatus.selectionCount;

    //     gold = savedStatus.gold;
    //     statusUpgradePoint = savedStatus.statusUpgradePoint;
    // }


    #region 데이터 수정 

    //====================================
    /// <summary>
    /// 해당 exp 만큼 경험치를 획득하고, 레벨업시 레벨과 경험치 요구량을 변경한다.
    /// </summary>
    /// <param name="exp"></param> - 
    /// <returns></returns>     
    public void GetExp(float exp)
    {
        //
        currExp.value += exp *  expMultiplier.value * 0.01f; 
    }

    public void LevelUp()
    {
        level.value++;
        currExp.value -= maxExp.value;
        SetMaxExp();
        Debug.Log($"레벨업! {level.value} : {currExp.value} / {maxExp.value}");
    }



    void SetMaxExp()
    {
        int currLevel = (int)level.value;
        // int currLevel = 0;
        maxExp.value = 5*Mathf.Pow(1.15f,currLevel);
    }

    //=============================================================
    #endregion



    //=========================================================
    // public bool TryUseGold(int cost)
    // {
    //     if (gold >= cost)
    //     {
    //         UseGold(cost);
    //         return true;
    //     }
    //     return false;
    // }

    public void GetGold(int amount)
    {
        gold += amount;

        GameEventManager.Instance.onChangePlayerGold.Invoke(amount, gold);
    }

    public void UseGold(int amount)
    {
        gold -= amount;

        GameEventManager.Instance.onChangePlayerGold.Invoke(-amount, gold);
    }

    //=======================================================
    public void GetStatusUpgradePoint(int amount)
    {
        statusUpgradePoint += amount;
    }

    public void UseStatusUpgradePoint(int amount)
    {
        statusUpgradePoint -= amount;
        // Debug.Log("띵");
    }
}
