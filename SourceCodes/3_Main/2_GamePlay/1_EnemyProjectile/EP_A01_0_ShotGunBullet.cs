using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BW;

public class EP_A01_0_ShotGunBullet : EnemyProjectile
{
   [SerializeField] ParticleSystem ps;
    [SerializeField] Rigidbody rb;
    [SerializeField] SphereCollider _collider;
    protected override string id => "A01_0";

    float lifeTime;
    float speed; 

    [SerializeField] TrailRenderer[] trailRenderers;


    //=====================================================================
    protected override void Init_Custom()
    {
        ps = GetComponent<ParticleSystem>();

        if (eaData is EA_A01_0_Shotgun abilityData)
        {
            lifeTime = abilityData.lifeTime;
            speed = abilityData.movementSpeed;
        }
    }
    
    public void SetDirAndMove(float noiseAngle)
    {
        Quaternion noiseRotation = Quaternion.AngleAxis(noiseAngle, Vector3.up); // Y축 회전 노이즈 추가

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
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
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
