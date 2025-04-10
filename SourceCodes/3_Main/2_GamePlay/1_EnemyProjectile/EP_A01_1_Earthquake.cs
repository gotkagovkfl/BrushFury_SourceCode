using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BW;

public class EP_A01_1_Earthquake: EnemyProjectile
{
    [SerializeField] ParticleSystem ps;
    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider _collider;
    protected override string id => "A01_1";

    float lifeTime;
    float radius;
    float speed; 

    [SerializeField] ParticleSystem[] mainParticles;
    [SerializeField] ParticleSystem rocksEffect;


    //=====================================================================
    protected override void Init_Custom()
    {
        ps = GetComponent<ParticleSystem>();

        if (eaData is EA_A01_1_Earthquake abilityData)
        {
            lifeTime = abilityData.lifeTime;
            speed = abilityData.movementSpeed;
            radius = abilityData.radius;
        }

        rb.velocity = dir * speed;  // 출발
        _collider.radius = radius;
         _collider.enabled = true;

        foreach(var particle in mainParticles)
        {
            ParticleSystem.MainModule psMain = particle.main;
            psMain.startLifetime = lifeTime;
        }
        // rocksEffect.gameObject.SetActive(true);
    }
    
    
    protected override IEnumerator DestroyCondition()
    {
        yield return new WaitForSeconds(lifeTime);
        // var psm = ps.main;
        _collider.enabled = false;
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(3f);

        
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
        _collider = GetComponent<SphereCollider>();
    }

    //================================================

    void OnTriggerEnter(Collider other)
    {
        if( other.CompareTag("Player") && other.TryGetComponent(out Player player))
        {
            Vector3 hitPoint = other.ClosestPoint(myTransform.position).WithStandardHeight();
            
            if(player.TryGetDamaged(hitPoint, damage))
            {

            }
        }    
    }
}