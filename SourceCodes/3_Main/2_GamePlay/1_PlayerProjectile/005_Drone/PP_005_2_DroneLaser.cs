using System.Collections;
using System.Collections.Generic;
using BW;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PP_005_2_DroneLaser  : PlayerProjectile
{
    protected override string id => "005_2";

    [Header("Laser Settting")]
    int explosionCount;
    float explosionDelay;
    float unitDistance;          // 폭발이 일어나는 단위 길이
    float duration;             // 처음부터 끝까지 레이저를 긋는 시간. 
    PlayerProjectile prefab_explosion;

    public PP_005_4_LaserDrone drone;

    [SerializeField] LineRenderer lr;

    //=====================================================
    public override void OnCreatedInPool()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
    }
    
    
    protected override void Init_Custom()
    {
        if (baseData is PA_005T_LaserDrone droneData )
        {
            explosionCount = droneData.explosionCount_laser;
            explosionDelay = droneData.explosionDelay;
            unitDistance = droneData.unitDistance;
            duration = droneData.duration_laser;
            prefab_explosion = droneData.pp_laserExplosion;
        }

        lr.positionCount = 2;
        dir = (targetPos -  initPos).WithFloorHeight().normalized; 

        StartCoroutine(FireRoutine());
    }

    void DrawLaser(Vector3 laserStartPos, Vector3 laserTargetPos)
    {
        lr.SetPosition(0, laserStartPos);
        lr.SetPosition(1, laserTargetPos);
    }



    IEnumerator FireRoutine()
    {
        float unitDuration = duration / explosionCount;
        for(int i=0;i<explosionCount; i++)
        {
            Vector3 startUnitPos = myTransform.position.WithFloorHeight();
            Vector3 destination = startUnitPos + dir * unitDistance;
            Vector3 explostionPos  = (startUnitPos + destination) *0.5f;
            float elapsed = 0;
            while( elapsed <= unitDuration)
            {
                float ratio = elapsed / unitDuration;
                Vector3 nextPos = Vector3.Lerp(startUnitPos, destination, ratio);
                
                //
                elapsed += Time.fixedDeltaTime;
                yield return GameConstants.waitForFixedUpdate;
                
                //
                myTransform.position = nextPos;
                
                Vector3 dronePos = drone!=null?drone.myTransform.position: initPos;
                DrawLaser(  dronePos ,nextPos);
            }
            myTransform.position = destination;

            //
            StartCoroutine(ExplosionRoutine(explostionPos));
        }

        lr.positionCount = 0;
        yield return new WaitForSeconds( explosionDelay + 0.1f);
        DestroyProjectile();
    }


    IEnumerator ExplosionRoutine(Vector3 initPos)
    {
        yield return new WaitForSeconds(explosionDelay);
        var explosion = PoolManager.Instance.playerProjectilePoolSys.GetPP(baseData, prefab_explosion, initPos, initPos);
    }


    // 레이저는 지나는 적에게 피해를 입힌다. 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.TryGetComponent(out Enemy enemy))
        {
            Vector3 hitPoint = other.ClosestPoint(myTransform.position);
            enemy.GetDamaged(hitPoint,finalDmg);
        }
    }
}
