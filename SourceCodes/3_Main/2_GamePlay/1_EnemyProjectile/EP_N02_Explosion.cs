using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;

public class EP_N02_Explosion : EnemyProjectile
{
    [SerializeField] ParticleSystem ps;

    protected override string id => "N02";



    float explosionRadius;
    float impulse; 


    //=====================================================================
    protected override void Init_Custom()
    {
        ps = GetComponent<ParticleSystem>();

        if (eaData is EA_N02_Explosion n02)
        {
            explosionRadius = n02.explosionRadius;
            impulse = n02.impulse;
        }
        Explode();
    }
    
    
    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitUntil( ()=> ps.IsAlive()==false);
        // var psm = ps.main;
        // yield return new WaitForSeconds(psm.duration);
    }


    void Explode()
    {        
        Vector3 explosionPos = myTransform.position;
        ps.Play();

        Collider[] hits = Physics.OverlapSphere(explosionPos, explosionRadius,GameConstants.playerLayer);

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
