using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BW;

public class PP_005_3_DroneLaserExplosion: PlayerProjectile
{
    protected override string id => "005_3";

    [Header("Explosion Settting")]
    float explosionRadius;

    [SerializeField] ParticleSystem ps;

    //=====================================================
    public override void OnCreatedInPool()
    {
        ps = GetComponent<ParticleSystem>();
    }
    
    
    
    protected override void Init_Custom()
    {
        if (baseData is PA_005T_LaserDrone droneData )
        {
            explosionRadius = droneData.explosionRadius;
        }
        
        Explode();
        StartCoroutine( DestroyRoutine());
    }

    protected IEnumerator DestroyRoutine()
    {
        ps.Play();
        yield return new WaitUntil(()=>ps.IsAlive()==false);
        DestroyProjectile();
    }


    void Explode()
    {
        Vector3 explosionPos = myTransform.position;

        Collider[] hits = Physics.OverlapSphere(explosionPos, explosionRadius,GameConstants.enemyLayer);

        // 
        for(int i=0;i<hits.Length;i++)
        {
            Collider hit = hits[0];
            Vector3 hitPoint = hit.ClosestPoint( explosionPos ).WithStandardHeight();

            // 적에게 피해를 입히는 로직
            Enemy enemy = hit.GetComponent<Enemy>();
            enemy.GetDamaged(hitPoint, finalDmg);
        }
    }
}