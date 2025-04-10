using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using UnityEditor.Rendering;

public class EP_N07_PoisonArea  : EnemyProjectile
{

    protected override string id => "N07";



    float radius;
    float slowPower; 
    float duration;

    [SerializeField] SphereCollider _collider;
    [SerializeField] ParticleSystem ps_aoe;
    [SerializeField] ParticleSystem ps_smoke;


    //=====================================================================
    protected override void Init_Custom()
    {
        _collider = GetComponent<SphereCollider>();


        if (eaData is EA_N07_PoisonArea abilityData)
        {
            radius = abilityData.radius;
            slowPower = abilityData.slowAmount;
            duration = abilityData.duration;
        }
        // ps_aoe.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        var main_aoe = ps_aoe.main;
        // main_aoe.duration = duration;
        main_aoe.startLifetime = duration;
        main_aoe.startSize = radius*2;

        var main_smoke = ps_smoke.main;
        main_smoke.startLifetime = duration;


        _collider.radius = radius;


        ps_aoe.Play();


        StartCoroutine(DotRoutine());
    }
    
    
    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitUntil( ()=> ps_aoe.IsAlive()==false);
        // var psm = ps.main;
        // yield return new WaitForSeconds(psm.duration);
    }


    IEnumerator DotRoutine()
    {
        WaitForSeconds wfs = new(0.5f);

        yield return new WaitUntil( ()=>activated );
        while(activated)
        {
            yield return wfs;
            
            // Debug.Log("ㅇㄹㄴㅁㅁㅇㄹㄴ");
            
            Vector3 initPos = myTransform.position;

            Collider[] hits = Physics.OverlapSphere(initPos, radius, GameConstants.playerLayer);

            // 충돌지역에 플레이어가 있으면. 
            if(hits.Length>0)
            {
                Collider hit = hits[0];
                Vector3 hitPoint = hit.ClosestPoint(initPos ).WithStandardHeight();;
                // 적에게 피해를 입히는 로직
                Player player = hit.GetComponent<Player>();
                if (player != null)
                {
                    player.GetDamaged( hitPoint, damage);
                    player.GetSlow(slowPower, 0.5f);       
                }
                
            }
            
        }
    }


}