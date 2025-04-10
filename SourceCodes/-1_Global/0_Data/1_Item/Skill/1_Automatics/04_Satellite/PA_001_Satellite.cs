using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BW;
using TMPro;
using System.Resources;


[CreateAssetMenu(fileName = "PA_001", menuName = "SO/SkillItem/001_Satellite", order = int.MaxValue)]
public class PA_001_Satellite  : SkillItemSO
{
    [Header("Settings")]
    public int satelliteCount = 3;
    public float duration = 4f;
    public float rotateSpeed = 180f;
    public float orbitRadius = 2f;
    public float activationTimeRate = 0.1f;



    
    [Header("Spawn Projectiles")]
    public PlayerProjectile prefab_pp_center;      
    public PlayerProjectile prefab_pp_satellite;

    //=====================================================================================


    protected PA_001_Satellite()
    {
        skillType = SkillType.Scroll;
    }

    public override string description => "소용돌이 설명";



    public override string dataName => "소용돌이";

    public override string id_base => $"PA_001_Satellite";



    //============================================================================

    public override void OnEquip()
    {
        PoolManager.Instance.playerProjectilePoolSys.AddPoolItem(prefab_pp_center);
        PoolManager.Instance.playerProjectilePoolSys.AddPoolItem(prefab_pp_satellite);
    }


    public override void OnUnEquip()
    {
        PoolManager.Instance.playerProjectilePoolSys.RemovePoolItem( prefab_pp_center );
        PoolManager.Instance.playerProjectilePoolSys.RemovePoolItem( prefab_pp_satellite );
    }

    //=======================================================================================

    public override IEnumerator UseRoutine()
    { 
        Player player = Player.Instance;

        // 중심생성
        var pp_center = PoolManager.Instance.playerProjectilePoolSys.GetPP<PP_001_0_SatelliteCenter>(this, prefab_pp_center, player.t.position, player.t.position);
        pp_center.Follow(player.t);

        // 회전 속도 증가, 
        player.StartCoroutine( RotateRoutine( pp_center ) );

        // 회전 위치 잡기.
        yield return ActivationRoutine( pp_center );

        pp_center.DestroyProjectile();
    }

    //=====================================================================================================
    
    /// <summary>
    /// 활성화할 때 위성이 중앙에서 바깥으로 이동한다.  
    /// </summary>
    protected IEnumerator ActivationRoutine( PP_001_0_SatelliteCenter satelliteCenter )
    {
        
        //  // 초기화.위성 생성
        List<PP_001_1_Satellite> satellites = new();
        for(int i=0;i<satelliteCount;i++)
        {
            float angle = i * (360f / satelliteCount );
            Vector3 initOffset = CalculateSatellitePosition(angle) + new Vector3(0,1,0);
            
            var pp_satellite = PoolManager.Instance.playerProjectilePoolSys.GetPP<PP_001_1_Satellite>(this, prefab_pp_satellite, satelliteCenter.myTransform.position, satelliteCenter.myTransform.position);
            pp_satellite.Follow(satelliteCenter.myTransform, initOffset);
            pp_satellite.InitTargetDist(orbitRadius);
            satellites.Add( pp_satellite );
        }
        int currSatelliteCount = satellites.Count;
        
        // 가속 시. 
        float activationTime = duration * activationTimeRate;
        float elapsed = 0f;
        while ( elapsed < activationTime)
        {
            float rate =  elapsed / activationTime;
            //
            for(int i=0;i<currSatelliteCount;i++)
            {
                satellites[i].UpdateRatio( rate );
            }

            //
            elapsed += Time.fixedDeltaTime;
            yield return GameConstants.waitForFixedUpdate;
        }
        
        // 유지
        yield return new WaitForSeconds(duration);

        // 감속 시 
        elapsed = 0f;
        while ( elapsed <= activationTime)
        {
            float rate =  1 - elapsed / activationTime;
            //
            for(int i=0;i<currSatelliteCount;i++)
            {
                satellites[i].UpdateRatio( rate );
            }

            //
            elapsed += Time.fixedDeltaTime;
            yield return GameConstants.waitForFixedUpdate;
        }





        // 끝나고 파괴
        foreach( PlayerProjectile satellite in satellites )
        {
            satellite.DestroyProjectile();
        }
    }


    /// <summary>
    /// 센터 오브젝트를 회전시킨다. 
    /// </summary>
    protected IEnumerator RotateRoutine( PP_001_0_SatelliteCenter satelliteCenter )
    {
        // 가속 
        float activationTime = duration * activationTimeRate;
        float elapsed = 0f;
        while ( elapsed < activationTime)
        {
            //
            float rate =  elapsed / activationTime;
            float currRotateSpeed = rotateSpeed * rate ;
            satelliteCenter.SetRotateSpeed(currRotateSpeed);

            //
            elapsed += Time.fixedDeltaTime;
            yield return GameConstants.waitForFixedUpdate;
        }

        // 유지.
        yield return new WaitForSeconds( duration );
        
        // 감속
        elapsed = 0f;
        while ( elapsed <= activationTime)
        {
            //
            float rate =  1 - elapsed / activationTime;
            float currRotateSpeed = rotateSpeed * rate ;
            satelliteCenter.SetRotateSpeed(currRotateSpeed);

            //
            elapsed += Time.fixedDeltaTime;
            yield return GameConstants.waitForFixedUpdate;
        }
    }


    Vector3 CalculateSatellitePosition(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float x = Mathf.Cos(radian);
        float z = Mathf.Sin(radian);
        
        //
        return new Vector3(x,0,z);
    }
}
