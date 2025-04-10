using System.Collections;
using System.Collections.Generic;
using BW;

using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;
using System.Linq.Expressions;



[RequireComponent(typeof(CharacterController))]
[RequireComponent( typeof(PlayerSkills), typeof(PlayerInteraction))]
public class Player : Singleton<Player>   // ui 등에서 플레이어 컴포넌트에 접근하기 쉽도록 싱글톤
{
    [HideInInspector] public Transform t;
    public bool initialized;

    
    [Space(20)] public PlayerStatus status;     // 플레이어의 능력치 정보 
    [Space(20)] public PlayerStatusEffects statusEffects;
    // public SpriteEntity spriteEntity;

    public SpineEntity spineEntity;
    public PlayerSpineAnimationController spineAnimationController;

    //======= ui ========

    [SerializeField] PlayerUI playerUI;
    
    // public PlayerDraw playerDraw;
    //
    CharacterController controller;
    Collider playerCollider;
    // Canvas playerCanvas;

    //===========================
    // public Vector3 mouseDir;
    public Vector3 lastMoveDir;

    // public PlayerAnimator animator;

    public bool isAlive => status.currHp > 0;


    //------- after hit--------
    Sequence onHitSeq;


    //-------- skills ------------
    //
    public PlayerSkills skills;
    // public PlayerEquipments equipments;


    PlayerInteraction playerInteraction;


    [SerializeField] List<Enemy> collidingEnemies = new();
    //====================================================================================


    

    void Update()
    {
        if (GamePlayManager.isGamePlaying == false) return;

        //
        // mouseDir = (PlayerInputManager.Instance.mouseWorldPos - t.position).WithFloorHeight().normalized;
        // mouseDir.y = 0;    

        // 스턴 지속시간 감소
        statusEffects.OnUpdate();

        skills.OnUpdate();

        Move();

        //
        if( status.isStunned == false)
        {
            playerInteraction.OnUpdate(t.position);
        }

    }


    //============================================================================
    void OnTriggerEnter(Collider other)
    {
        // 적 몸통. 
        if (other.CompareTag("Enemy"))
        {
            //
            if (other.TryGetComponent(out Enemy e))
            {
                collidingEnemies.Add(e);
            }
        }

        // 적 투사체. 
        // if (other.CompareTag("EnemyProjectile"))
        // {
        //     Vector3 hitPoint = other.ClosestPoint(t.position);
        //     if (other.TryGetComponent(out EnemyProjectile ep) && TryGetDamaged(hitPoint, ep.damage))
        //     {
        //         ep.DestroyProjectile(0);
        //     }
        //     //
        // }
    }

    void OnTriggerExit(Collider other)
    {
        // 적 몸통. 
        if (other.CompareTag("Enemy"))
        {
            //
            if (other.TryGetComponent(out Enemy e))
            {
                collidingEnemies.Remove(e);
            }
        }
    }


    IEnumerator GetDamagedByCollision()
    {
        WaitUntil wu = new WaitUntil( ()=> status.invincible == false);
        WaitForFixedUpdate wffu = new();

        while(isAlive)
        {
            //
            if (collidingEnemies.Count>0)
            {
                float dmg  = collidingEnemies.Where(x=>x.isAlive).Max(x=>x.status.collisionDmg);
                if( TryGetDamaged( t.position.WithStandardHeight(),dmg) ==false)
                {
                    yield return wu;
                }
            }
            yield return wffu;
        }
        
       
    }



    /// <summary>
    /// 플레이어 초기화시 호출. 
    /// </summary>
    public void InitPlayer(UserDataSO UserData, CharacterDataSO characterData)
    {
        t = transform;
        //
        t.position = Stage.Instance.playerInitPos;  // 플레이어 위치 지정 

        controller = GetComponent<CharacterController>();
        playerCollider = GetComponent<Collider>();
        playerCollider.enabled = true;

        //
        status = new(UserData, characterData);  

        statusEffects= GetComponent<PlayerStatusEffects>();
        statusEffects.Init();
        //--------- after init status --------------

        skills = GetComponent<PlayerSkills>();
        skills.Init(characterData);

        playerInteraction = GetComponent<PlayerInteraction>();



        // animator = GetComponentInChildren<PlayerAnimator>();
        //----------- after init finished ---------------------

        playerUI.Init(this);     // 

        spineEntity = GetComponentInChildren<SpineEntity>();
        spineEntity.Init(controller.radius, controller.height);


        spineAnimationController = GetComponentInChildren<PlayerSpineAnimationController>();
        spineAnimationController.Init(characterData);
        // spriteEntity = GetComponent<SpriteEntity>();
        // spriteEntity.Init(controller.radius, controller.height);

        //
        StartCoroutine( GetDamagedByCollision());
        StartCoroutine( CheckLevelUpRoutine() );

        GameEventManager.Instance.onInitPlayer.Invoke();    // 플레이어 초기화가 필요한 ui 작업을 하기 위함. 
        initialized = true;
    }


    public void OnStageFinish()
    {
        // equipments.OnStageFinish();
    }

    //========================================================================
    // public void AcquireItem(ItemDataSO itemData)
    // {
    //     switch(itemData.type)
    //     {
    //         case ItemType.Consumable:
    //             itemData.TryGet();
    //             break;

    //         case ItemType.Skill:
    //             skills.SwitchSkill( (SkillItemSO)itemData);
    //             break;

    //         case ItemType.Equipment:
                
    //             PlayerEquipment pe = new PlayerEquipment( (EquipmentItemSO)itemData, -1);    // 나중에 아이템 수명 생기면 코드수정해야함. 
    //             equipments.Equip( pe );
    //             break;
    //     }
    // }





    //==========================================================================
    void MoveActively()
    {
        // 이동불가 상태의 경우, 실행 x - 유저의 입력을 받아 이동
        if (status.immobilized)
        {
            return;
        }

        // 땅위의 경우
        Vector2 inputVector = PlayerInputManager.Instance.moveVector;

        // Debug.Log(moveVector);
        lastMoveDir = transform.right * inputVector.x + transform.forward * inputVector.y;
        lastMoveDir.y = 0;      // 방향 조절에 필요 없기떄문.

        Vector3 moveVector = lastMoveDir.normalized * status.finalMovementSpeed;

        // animator.OnMove(moveVector.magnitude);
        spineAnimationController.OnMove(moveVector.magnitude,lastMoveDir.x);
        controller.Move(moveVector * Time.deltaTime);
    }

    /// <summary>
    /// 상태 이상 걸렸을 때를 위함. - 넉백, 공포, 매혹 등이 있을 듯. 
    /// </summary>
    void MovePassively()
    {
        // 이동불가 상태의 경우에만 실행 
        if (status.immobilized == false)
        {
            return;
        }

        
        Vector3 moveVector =  status.forcedMoveVelocity; // 넉백 벡터 추가
        
        controller.Move(moveVector * Time.deltaTime);
        spineAnimationController.OnMove(0, 0);     // 넉백이 아니라면 이렇게 해선 안됨. 
        // animator.OnMove(0);
    }


    /// <summary>
    /// 움직임
    /// </summary>
    void Move()
    {
        // 그리기 모드일 때는 움직이지 못하도록 함
        if(status.immobilized)
        {
            MovePassively();
        }
        else
        {
            MoveActively(); 
        }
    }

    //========================================================================
    /// <summary>
    /// 무적이면 피해X
    /// </summary>
    /// <param name="damage"></param>
    public bool TryGetDamaged( Vector3 hitPoint, float damage)
    {
        if (status.invincible)
        {
            return false;
        }
        //
        GetDamaged(hitPoint,damage);

        //
        float invincibleDuration = status.duration_invincible;
        statusEffects.SetInvincibleOnHit(invincibleDuration);  // 피해 직후 무적 적용. 
        PlayAnim_PlayerHit(invincibleDuration); 
        SoundManager.Instance.Invoke(t, SoundEventType.Player_Hit); 
        // statusEffects.GetNockbacked(attackerPos, hitPoint, 9);
        return true;
    }
    
    
    public void GetImpulsiveDamaged(float dmg, Vector3 enemyPos, Vector3 hitPoint, float impulse)
    {
        
        GetDamaged(hitPoint, dmg);

        //
        float invincibleDuration = status.duration_invincible;
        statusEffects.SetInvincibleOnHit(invincibleDuration);  // 피해 직후 무적 적용. 
        PlayAnim_PlayerHit(invincibleDuration); 
        SoundManager.Instance.Invoke(t, SoundEventType.Player_Hit); 
        

        statusEffects.GetNockbacked(enemyPos, hitPoint , impulse);
    }

    public void GetDamaged(Vector3 hitPoint, float amount)
    {
        status.currHp -= amount;

        if (status.currHp <= 0)
        {
            Die();
        }

        // ui
        playerUI.UpdateCurrHp();


        PoolManager.Instance.effectPoolSys.GetDamageText(amount, hitPoint,DamageType.DMG_PLAYER);
    }

    public void GetSlow(float amount,float duration)
    {
        statusEffects.GetSlow(amount, duration);
    }

    //==============================================================
    public void GetExp(float value)
    {
        status.GetExp(value);
        playerUI.UpdateCurrExp();
    }


    IEnumerator CheckLevelUpRoutine()
    {
        WaitUntil wu = new( ()=> status.canLevelUp);
        WaitForSeconds wfs = new(1f);

        while(isAlive)
        {
            yield return wu;

            yield return wfs;
            status.LevelUp();
            GamePlayManager.Instance.OnLevelUp();


            //
            playerUI.UpdateCurrExp();
            playerUI.UpdateMaxExp();
            playerUI.UpdateLevel();
        }
        
    }



    public void GetHealed(float amount)
    {
        status.currHp += amount;

        // ui
        playerUI.UpdateCurrHp();


        PoolManager.Instance.effectPoolSys.GetDamageText(amount,transform.position.WithStandardHeight(), DamageType.HEAL_PLAYER);
    }

    public void GetInk(float amount)
    {
        status.currInk += amount;
        playerUI.UpdateCurrInk();
    }

    /// <summary>
    /// 평타 적중 시 적중된 적에 비례해서 먹이 회복된다. 
    /// </summary>
    /// <param name="hitCount"></param>
    public void RecoverInkWithBasicAttack(int hitCount)
    {
        float inkRecoveryAmount = status.inkRecoveryAmount * hitCount;
        GetInk(inkRecoveryAmount);

        // Debug.Log($"[먹회복] {hitCount}, {inkRecoveryAmount}");
    }

    public void UseInk(float amount)
    {
        if (amount <= 0) 
            return; 
        
        status.currInk = status.currInk - amount;

        playerUI.UpdateCurrInk();
    }

    
    void Die()
    {
        // brushAttack.drawArea.gameObject.SetActive(false);

        // playerCollider.enabled = false;        // 이게 brush collider 는 true로 세팅하네??

        SoundManager.Instance.Invoke(t, SoundEventType.Player_Die);
        GamePlayManager.Instance.GameOver();
    }





    //=====================================================

    //
    #region ==== 연출 ======


    /// <summary>
    ///  맞으면 빨간색으로 깜빡깜빡임
    /// </summary>
    public void PlayAnim_PlayerHit(float targetDuration)
    {
        // Color targetColor = new Color(1, 0.2f, 0.2f);
        // Color originColor = Color.white;

        // if (onHitSeq != null && onHitSeq.IsActive())
        // {
        //     onHitSeq.Kill();
        // }
        
        // float fadeInTime=0.15f;
        // float fadeOutTime = 0.15f;
        // float interval = 0.2f;
        // float animTime = fadeInTime+ fadeOutTime + interval;

        // onHitSeq = DOTween.Sequence()
        // .OnComplete(() =>
        // {
        //     spriteEntity.spriteRenderer.color = originColor;

        // })
        // .Append(spriteEntity.spriteRenderer.DOColor(targetColor, fadeInTime))
        // .Append(spriteEntity.spriteRenderer.DOColor(originColor, fadeOutTime))
        // .AppendInterval(interval)
        // .SetLoops( (int) (targetDuration / animTime) )
        // .Play();
    }


    public void OnStartGamePlay()
    {
        // playerCanvas.gameObject.SetActive(true);
    }

    #endregion











    // float radius_1 = 15f;
    // float radius_2 = 6f;
    // public int segments = 36; // 선분 개수(높을수록 원이 부드럽게 보임)
    // public Color gizmoColor = Color.red;
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = gizmoColor;  // 원하는 색상 지정

    //     Vector3 center = transform.position;
    //     float angleStep = 2f * Mathf.PI / segments;

    //     // 첫 점을 미리 계산해둠
    //     Vector3 prevPoint = center + new Vector3(Mathf.Cos(0), 0f, Mathf.Sin(0)) * radius_1;

    //     // segments번 반복하여 각도별 점을 그리고 이전 점과 연결
    //     for (int i = 1; i <= segments; i++)
    //     {
    //         float angle = angleStep * i;
    //         Vector3 currentPoint = center + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius_1;

    //         Gizmos.DrawLine(prevPoint, currentPoint);
    //         prevPoint = currentPoint;
    //     }


    //     // 첫 점을 미리 계산해둠
    //     prevPoint = center + new Vector3(Mathf.Cos(0), 0f, Mathf.Sin(0)) * radius_2; 

    //     // segments번 반복하여 각도별 점을 그리고 이전 점과 연결
    //     for (int i = 1; i <= segments; i++)
    //     {
    //         float angle = angleStep * i;
    //         Vector3 currentPoint = center + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius_2;

    //         Gizmos.DrawLine(prevPoint, currentPoint);
    //         prevPoint = currentPoint;
    //     }
    // }






}

public interface ITimeScaleable
{
    void SetTimeScale(float scale);
}