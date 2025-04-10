using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PA_200", menuName = "SO/SkillItem/200_Dash", order = int.MaxValue)]
public class PA_4000_DashBase : SkillItemSO
{
    [Header("Dash Settings")]
    public float dashMultiplier = 3f;
    public float dashDuration = 0.2f;
    public float delay_afterCast = 0.3f;
    
    [Header("Effect Settings")]
    // public GameObject dashEffectPrefab;  // 대쉬 이펙트 프리팹
    [SerializeField] GameEffect prefab_dashStartEffect;
    [SerializeField] GameEffect prefab_ghostTrailEffect;
    [SerializeField] GameEffect prefab_inkTrailEffect;

    [Header("Damage setting")]
    public bool canBodyAttack;
    public float damageRadius = 1.5f;

    [Header("DashCombo Setting")]
    public int maxDashCount = 1;
    [Range(0f,1f)] public float comboDetectionThreshold = 0.5f;

    public override string id_base => "대쉬~~";

    public override string description => "대쉬~~";

    public override string dataName => "대쉬~~";




    //================================================================================
    protected PA_4000_DashBase()
    {
        skillType = SkillType.Unique;
    }

    public override void OnEquip()
    {
        EffectPoolSys effectPoolSys = PoolManager.Instance.effectPoolSys;

        effectPoolSys.AddPoolItem(prefab_dashStartEffect);
        effectPoolSys.AddPoolItem(prefab_ghostTrailEffect);
        effectPoolSys.AddPoolItem(prefab_inkTrailEffect);
    }


    public override void OnUnEquip()
    {
        EffectPoolSys effectPoolSys = PoolManager.Instance.effectPoolSys;

        effectPoolSys.RemovePoolItem(prefab_dashStartEffect);
        effectPoolSys.RemovePoolItem(prefab_ghostTrailEffect);
        effectPoolSys.RemovePoolItem(prefab_inkTrailEffect);
    }



    public override IEnumerator UseRoutine()
    { 
        Player player = Player.Instance;
        Vector3 dir = player.lastMoveDir.normalized;

        player.spineAnimationController.OnUseUniqueAbility();
        
        player.StartCoroutine(DashRoutine_Custom(player));  
        player.StartCoroutine(EffectRoutine(dir));
        yield return DashRoutine(player,dir, 1);     
    }

    protected IEnumerator DashRoutine(Player player, Vector3 dir, int currDashCount)
    { 
        if (currDashCount> maxDashCount )
        {
            yield break;
        }
        
        SoundManager.Instance.Invoke(Player.Instance.t, SoundEventType.Player_Dash);        // 나중에 사운드 파일 별도로 저장. 
        bool ComboDetected = false;


        
        
        // 대시 실행  
        Vector3 forcedMoveVelocity = dir * player.status.finalMovementSpeed * dashMultiplier;
        player.status.forcedMoveVelocity += forcedMoveVelocity;
        player.status.stack_immobilized++;
        player.status.stack_invincible++;


        // 대시 지속
        float totalElapsedTime = 0f;
        float elapsedTime = 0f;
        float comboDetectionStartTime = (dashDuration + delay_afterCast) * comboDetectionThreshold;

        Vector3 nextDir = dir;
        while (elapsedTime < dashDuration)
        {
            if( totalElapsedTime >= comboDetectionStartTime  && PlayerInputManager.Instance.uniqueSkill== true )
            {
                ComboDetected = true;
                // 땅위의 경우
                Vector2 inputVector = PlayerInputManager.Instance.moveVector;
                nextDir = player.t.right * inputVector.x + player.t.forward * inputVector.y;
                nextDir.y = 0;  
            }
            
            
            elapsedTime += Time.fixedDeltaTime;
            totalElapsedTime += Time.fixedDeltaTime;
            yield return GameConstants.waitForFixedUpdate;
        }

        // 원래 속도로 복구
        player.status.forcedMoveVelocity -= forcedMoveVelocity;
        player.status.stack_immobilized--;
        yield return GameConstants.waitForFixedUpdate;
        player.status.stack_invincible--;

        elapsedTime = 0f;
        while (elapsedTime < delay_afterCast)
        {
            
            if(totalElapsedTime >= comboDetectionStartTime &&PlayerInputManager.Instance.uniqueSkill== true )
            {
                ComboDetected = true;
                Vector2 inputVector = PlayerInputManager.Instance.moveVector;
                nextDir = player.t.right * inputVector.x + player.t.forward * inputVector.y;
                nextDir.y = 0;  
            }
            if( ComboDetected)
            {
                break;
            }

            elapsedTime += Time.fixedDeltaTime;
            totalElapsedTime += Time.fixedDeltaTime;
            yield return GameConstants.waitForFixedUpdate;
        }
        

        // 연속 대쉬 

        if ( ComboDetected  && currDashCount < maxDashCount )
        {
            // Debug.Log($"======={currDashCount} {maxDashCount} ");
            player.StartCoroutine(DashRoutine(player,nextDir, currDashCount+1));
            player.StartCoroutine(DashRoutine_Custom(player));       
        }
    }

    protected IEnumerator DashRoutine_Custom(Player player)
    {
        if ( canBodyAttack )
            yield return  BodyAttackRoutine(player);
    }




    /// <summary>
    /// 직접 충돌 시 적에게 피해를 입힌다. (성능 테스트 필요)
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    protected IEnumerator BodyAttackRoutine(Player player)
    {

        float damage = player.status.GetFinalPDmg(baseDamage, coefficient_pDmg);

        Dictionary<Enemy,bool> collidedEnemies = new();


        float elapsedTime = 0;
        while(elapsedTime < dashDuration )
        {
            Collider[] hits = Physics.OverlapSphere(player.t.position.WithFloorHeight(), damageRadius, GameConstants.enemyLayer);

            // 충돌지역에 플레이어가 있으면. 
            if(hits.Length>0)
            {
                foreach(Collider hit in hits)
                {
                    // 적에게 피해를 입히는 로직
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null && collidedEnemies.ContainsKey( enemy )==false)
                    {
                        collidedEnemies[enemy] = true;
                        enemy.GetDamaged( hit.ClosestPoint( player.t.position),damage );
                    }
                }

            }
            
            elapsedTime += Time.fixedDeltaTime;
            yield return GameConstants.waitForFixedUpdate;
        }
    }


    IEnumerator EffectRoutine(Vector3 dir)
    {
        
        
        
        int effectCount = 8;
        float interval = dashDuration / effectCount; 


        //
        EffectPoolSys effectPoolSys = PoolManager.Instance.effectPoolSys;

        GameEffect  effect = effectPoolSys.GetEffect(prefab_dashStartEffect.poolId, Player.Instance.t.position.WithStandardHeight(),dir*-1);
        GameEffect inkTrailEffect = effectPoolSys.GetEffect(prefab_inkTrailEffect.poolId, Player.Instance.t.position);

        for(int i=0;i<effectCount;i++)
        {
            effectPoolSys.GetEffect(prefab_ghostTrailEffect.poolId, Player.Instance.t.position);
            yield return new WaitForSeconds(interval);
        }

    }
   
}
