using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BW;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EP_N01_FireBall : EnemyProjectile
{
    [SerializeField] ParticleSystem ps;
    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider _collider;
    protected override string id => "N01";

    float lifeTime;
    float speed; 

    [SerializeField] List<TrailRenderer> trailRenderers;


    //=====================================================================
    protected override void Init_Custom()
    {
        ps = GetComponent<ParticleSystem>();

        if (eaData is EA_N01_FireBall abilityData)
        {
            lifeTime = abilityData.lifeTime;
            speed = abilityData.movementSpeed;
        }
        

        float noiseAmount = 15f; // 노이즈의 크기 (도 단위)
        float randomYaw = Math.GetRandom(-noiseAmount, noiseAmount); // -10도 ~ 10도 랜덤값

        Quaternion noiseRotation = Quaternion.AngleAxis(randomYaw, Vector3.up); // Y축 회전 노이즈 추가

        myTransform.rotation *= noiseRotation;
        Vector3 noiseDir = noiseRotation * dir;
        dir = noiseDir;
        
        rb.velocity = dir * speed;  // 출발
    }
    
    
    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitForSeconds(lifeTime);
        // var psm = ps.main;
        // yield return new WaitForSeconds(psm.duration);

        foreach( var tr in trailRenderers)
        {
            tr.Clear();
            tr.enabled = false;
        }
    }


    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();
        rb.velocity = Vector3.zero;

        
        foreach( var tr in trailRenderers)
        {
            tr.enabled = true; 
        }
    }

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();

        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
        trailRenderers = GetComponentsInChildren<TrailRenderer>().ToList();
        foreach( var tr in trailRenderers)
        {
            tr.autodestruct = false; 
        }
        

    }

    //================================================

    void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            Vector3 hitPoint = other.ClosestPoint(myTransform.position).WithStandardHeight();
            
            if(player.TryGetDamaged(hitPoint, damage))
            {
                DestroyImmediately();
        
            }
        }    
    }
}
