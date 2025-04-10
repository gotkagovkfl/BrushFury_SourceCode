using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using BW;

public class EnemyAbilitySystem : MonoBehaviour
{
    Enemy enemy;
    
    public List<EnemyAbility> abilities = new();
    public List<EnemyAbility> abilitiesOnDeath = new();
    public EnemyAbility usingAbility;
    public bool isCasting;
    public bool isUsing;
    bool isChanneling;
    // public bool isFixedTarget;
    CancellationTokenSource abilityTokenSource;
    // Coroutine abilityRoutine;
    // WaitWhile waitForCast;


    public float currAbilityRange
    {
        get
        {
            float ret = 1f;
            if( usingAbility != null)
            {
                ret = usingAbility.data.range;
            }
            return ret;
        }
    }

    
    //==================================================

    public void Init(Enemy enemy)
    {    
        this.enemy = enemy;
        usingAbility = null;
        isUsing = false;

        //
        abilities.Clear();
        for(int i=0;i<enemy.data.abilities.Count;i++)    
        {
            var abilityData =  enemy.data.abilities[i];
            
            EnemyAbility ability = new(abilityData, enemy,i);
            abilities.Add(ability);
        }

        //
        abilitiesOnDeath.Clear();
        for(int i=0;i<enemy.data.abilitiesOnDeath.Count;i++)
        {
            var abilityData =  enemy.data.abilitiesOnDeath[i];
            
            EnemyAbility ability = new(abilityData, enemy,i);
            abilitiesOnDeath.Add(ability);
        }
    }


    public void SetCurrAbility()
    {
        if (isUsing)
        {
            return;
        }
        
        EnemyAbility ability = abilities
        .Where(a =>a.CanSelected()) 
        .OrderByDescending(a => a.data.priority)
        .ThenBy(a => a.useCount)
        .FirstOrDefault(); 

        usingAbility = ability;
    }


    public bool CanCastCurrAbility()
    {
        if( enemy.status.uncastable )
        {
            // Debug.Log("U");
            return false;
        }
        if (usingAbility == null )
        {
            // Debug.Log(" NNNN");
            return false;
        }
        
        if(isUsing)
        {
            return false;
        }

        //
        return usingAbility.CanUse();
    }

    public bool TryStartAbilityRoutine()
    {
        // 능력 설정해서 
        SetCurrAbility();
        // 사용할 스킬이 있다면 
        if( CanCastCurrAbility() )
        {
            UseCurrAbility();
            return true;
        }
        return false;
    }

    public void UseCurrAbility()
    {
        isUsing = true;
        enemy.status.tenacityModifier += usingAbility.data.tenacityOnCasting;
        SetChanneling(true);

        
        //
        abilityTokenSource = new();
        var autoToken = this.GetCancellationTokenOnDestroy();
        var linkedCTS = CancellationTokenSource.CreateLinkedTokenSource(abilityTokenSource.Token, autoToken);
        abilityTokenSource = linkedCTS; // 그대로 재활용
        AbilityTask(linkedCTS.Token).Forget();
    }


    public async UniTask AbilityTask(CancellationToken token)
    {
        // cast
        isCasting = true;
        if(await usingAbility.CastingTask(token).CanNextStep(token)==false)  return;
        isCasting = false;

        // activation
        var animationTask = enemy.animationController.WaitForAnimation_AbilityActivation(usingAbility.data,token);   // 애니메이션
        var activationTask = usingAbility.ActivationTask(token);    // 능력 사용 로직
        if( await UniTask.WhenAll(animationTask, activationTask).CanNextStep(token)==false)return;

        // after cast
        if(await usingAbility.AfterDelayTask(token).CanNextStep(token)==false) return; 

        // Finish
        FinishUsingAbility();
    }

    void FinishUsingAbility()
    {      
        // 스킬 상태 해제 
        SetChanneling(false);
        enemy.status.tenacityModifier -= usingAbility.data.tenacityOnCasting;
        usingAbility = null;
        isUsing = false;
        isCasting = false;
    }


    public void Interrupt()
    {
        if(usingAbility == null || isCasting==false || usingAbility.data.uninterruptible)
        {
            return;
        }

        usingAbility.OnInterrupted();


        abilityTokenSource?.Cancel();
        abilityTokenSource?.Dispose();
        abilityTokenSource = null;


        FinishUsingAbility(); 
    }

    public void OnDie()
    {
        Interrupt();

        UseOnDeathAbilities();
    }


    void UseOnDeathAbilities()
    {
        foreach(var ability in abilitiesOnDeath)
        {
            ability.UseOnDeathAbility();
        }
    }


    void SetChanneling(bool isOn)
    {
        if (usingAbility == null || usingAbility.data.castWhileMoving )
        {
            return;
        }
        
        //
        if (isOn && isChanneling==false)
        {
            enemy.status.stack_immobilized++;
            isChanneling = true;
        }
        else if(isOn==false && isChanneling==true)
        {
            enemy.status.stack_immobilized--;
            isChanneling = false;
        }

    }

}
