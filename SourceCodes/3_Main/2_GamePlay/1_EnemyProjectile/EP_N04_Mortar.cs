using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using System.Linq;
using UnityEditor.Build;

public class EP_N04_Mortar  : EnemyProjectile
{
    [SerializeField] ParticleSystem ps;
    [SerializeField] Rigidbody rb;
    protected override string id => "N04_a";

    float timeToImpact;
    float radius;
    float offScreenTimeRatio; 
    float speed;
    float targetHeight = 20f;

    [SerializeField] List<TrailRenderer> trailRenderers;


    //=====================================================================
    protected override void Init_Custom()
    {
        ps = GetComponent<ParticleSystem>();

        if (eaData is EA_N04_Mortar abilityData)
        {
            timeToImpact = abilityData.timeToImpact;
            radius = abilityData.radius;
            offScreenTimeRatio = abilityData.offScreenTimeRatio;
        }


        foreach( var tr in trailRenderers)
        {
            tr.enabled = true; 
        }
    }
    
    
    protected override IEnumerator DestroyCondition()
    {
        // yield return null;
                

        
        
        yield return ProjectileMotion();    // 포물선 운동 후에, 
        
        

        foreach( var tr in trailRenderers)
        {
            tr.Clear();
            tr.enabled = false;
        }


        Explode();

        yield return new WaitForSeconds(1);
    }


    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();
        rb.velocity = Vector3.zero;


    }

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();

        rb = GetComponent<Rigidbody>();
        trailRenderers = GetComponentsInChildren<TrailRenderer>().ToList();
        foreach( var tr in trailRenderers)
        {
            tr.autodestruct = false; 
        }
        

    }

    //================================================

    IEnumerator ProjectileMotion()
    {
        float offScreenTime = timeToImpact * offScreenTimeRatio;
        float onScreenTime = timeToImpact - offScreenTime ;
        speed = targetHeight / onScreenTime * 2;

        // 우선 하늘로 특정 각도로 날라감. 
        float randomYaw = Math.GetRandom(80, 89); // -10도 ~ 10도 랜덤값
        Quaternion noiseRotation = Quaternion.AngleAxis(-randomYaw, Vector3.right); // x축 회전 노이즈 추가 (하늘로 날라가게)
        myTransform.rotation *= noiseRotation;
        
        rb.velocity = myTransform.forward * speed;
        yield return new WaitForSeconds(onScreenTime  * 0.5f );
   


        // 화면 밖에선 가만히
        yield return new WaitForSeconds(offScreenTime);


        // 대상 지점에 수직으로 떨어짐.
        myTransform.position = targetPos + new Vector3(0,targetHeight,0); 
        myTransform.rotation  = Quaternion.LookRotation(Vector3.down);
        rb.velocity = myTransform.forward * speed;



        // 트레일 잠깐 지웠다가.
        foreach( var tr in trailRenderers)
        {
            tr.Clear();
            tr.enabled = false;
        }


        yield return null;
        
        foreach( var tr in trailRenderers)
        {
            tr.enabled = true; 
        }
        


        yield return new WaitForSeconds(onScreenTime  * 0.5f );
    }


    void Explode()
    {
        // 폭발이펙트
        ps.Stop();
        int subEmitterCount = ps.subEmitters.subEmittersCount; 
        for (int i = 0; i < subEmitterCount; i++)
        {
            ps.TriggerSubEmitter(i);
        }
        

        //
        Collider[] hits = Physics.OverlapSphere(targetPos, radius,GameConstants.playerLayer);

        // 충돌지역에 플레이어가 있으면. 
        if(hits.Length>0)
        {
            Collider hit = hits[0];
            Vector3 hitPoint = hit.ClosestPoint( targetPos ).WithStandardHeight();
            // 적에게 피해를 입히는 로직
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.TryGetDamaged(  hitPoint, damage);
            }
        }
    }
}