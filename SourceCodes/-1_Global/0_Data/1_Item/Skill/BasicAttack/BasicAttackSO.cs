using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using BW;

[CreateAssetMenu(fileName = "PA_100", menuName = "SO/SkillItem/100_BasicAttack", order = int.MaxValue)]
public class BasicAttackSO : SkillItemSO
{
    [Header("BasicAttack Setting")]
    public float defaultDamage;
    public float damageWeight;
    public float radiusMultiplier=1;


    public float finalDmg=>defaultDamage +Player.Instance.status.pDmg.value * damageWeight;

    public override string id_base => "PA_100_BasicAttack";

    public override string description => "평타";

    public override string dataName => "평타";

    public int comboCount;
    public float comboResetTime = 1f;

    public List<PlayerProjectile> effects;        // 각 모션의 이펙트 
    public List<AnimationClip> animations;  // 각 모션의 애니메이션
    public List<float> delays;              // 다음 모션까지의 딜레이
    // public List<AttackDetectionSO> detections;      // 공격 적용 판정 (SO로 뺼거임)



    public BasicAttackSO()
    {
        skillType = SkillType.BasicAttack;
    }
    
    
    //===============================================
    void OnValidate()
    {
        //
        FixListCap(ref effects);
        FixListCap(ref animations);
        FixListCap(ref delays);
        // FixListCap(ref detections);
    }

    // 콤보카운트에 맞춰 리스트의 크기를 조절한다.     
    void FixListCap<T>(ref List<T> list)
    {
        list = list
        .Take(comboCount)
        .Concat(Enumerable.Repeat(default(T), Mathf.Max(0, comboCount - list.Count)))  // 부족한 부분을 0으로 채움
        .ToList();
    }


    //==========================================================

    public override void OnEquip()
    {
        for(int i=0;i<comboCount;i++)
        {
            PlayerProjectile effect = effects[i].GetComponent<PlayerProjectile>(); 
            PoolManager.Instance.playerProjectilePoolSys.AddPoolItem(  effect );
        }
    }

    public override void OnUnEquip()
    {
        for(int i=0;i<comboCount;i++)
        {
            PlayerProjectile effect =effects[i].GetComponent<PlayerProjectile>(); 
            PoolManager.Instance.playerProjectilePoolSys.RemovePoolItem(  effect );
        }
    }



    public override IEnumerator UseRoutine()
    {
        //Player BasicAttack 함수 실행.
        Vector3 targetPos = PlayerInputManager.Instance.mouseWorldPos;

        yield return AttackRoutine(targetPos);
    }

    IEnumerator AttackRoutine(Vector3 targetPos )
    {
        //
        Vector3 effectPos = Player.Instance.t.position.WithStandardHeight();

        for(int i=0;i<comboCount;i++)
        {
            // Player.Instance.animator.OnBasicAttackStart();      // 이건 이제 애니메이션 클립으로 재생할거임. 
            Player.Instance.spineAnimationController.OnBasicAttackStart();
            SoundManager.Instance.Invoke(Player.Instance.transform, SoundEventType.Player_BasicAttack);
            
            AnimationClip animation = animations[i];       // 이건 애니메이션 오버라이드 시켜야할듯.
            
        
            // 현재 이펙트만 활성화
            PlayerProjectile attackEffect = PoolManager.Instance.playerProjectilePoolSys.GetPP(this, effects[i], effectPos, targetPos );
            Vector3 attackDir = (targetPos - Player.Instance.t.position).normalized;


            Vector3 followOffset =  attackDir;
            // Vector3 followOffset =  Vector3.zero;
            attackEffect.Follow(Player.Instance.t,followOffset);

            //
            float delay = delays[i];
            yield return new WaitForSeconds(delay);
        }     

        //
    }

}
