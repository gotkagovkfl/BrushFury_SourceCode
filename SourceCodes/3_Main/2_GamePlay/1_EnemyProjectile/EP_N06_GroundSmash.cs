using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;

public class EP_N06_GroundSmash  : EnemyProjectile
{
    [SerializeField] ParticleSystem ps;

    protected override string id => "N06";



    float radius;
    float impulse; 


    //=====================================================================
    protected override void Init_Custom()
    {
        ps = GetComponent<ParticleSystem>();

        if (eaData is EA_N06_GroundSmash abilityData)
        {
            radius = abilityData.radius;
            impulse = abilityData.impulse;
        }
        Smash();
    }
    
    
    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitUntil( ()=> ps.IsAlive()==false);
        // var psm = ps.main;
        // yield return new WaitForSeconds(psm.duration);
    }


    void Smash()
    {
        Vector3 explosionPos = myTransform.position;
        ps.Play();

        Collider[] hits = Physics.OverlapSphere(explosionPos, radius,GameConstants.playerLayer);

        // 충돌지역에 플레이어가 있으면. 
        if(hits.Length>0)
        {
            Collider hit = hits[0];
            Vector3 hitPoint = hit.ClosestPoint(explosionPos ).WithStandardHeight();;
            // 적에게 피해를 입히는 로직
            Player player = hit.GetComponent<Player>();
            if (player != null)
            {
                player.GetImpulsiveDamaged( damage, explosionPos, hitPoint , impulse);
            }
        }
    }
}