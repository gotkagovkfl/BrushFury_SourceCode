using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/*
드론은 여러개 소환 가능.
각 개체별로 부팅 / 공격루틴 / 셧다운 과정이 진행됨.

1. 부팅 : 플레이어 몸에서 나와 공중에 배치되기까지의 과정
    - 이땐 공격이 가능하지 않다.

2. 공격루틴 : 적을 공격하는 메인 로직 
    - 각 공격루틴이 진행되기 위한 cost (mp) 가 있다. 
    - cost가 부족하면 공격루틴은 실행되지 않고, 기다린다. 
    - 루틴이 진행되면, attackCount만큼 투사체를 발사하며 공격루틴이 종료된다. 
    - interval의 지속시간 후에 공격루틴이 다시 실행된다. 

3. 셧다운 : 공중에서 다시 플레이어 몸으로 돌아와 사라지며 스킬이 종료된다. 
    - 소환된 모든 드론이 기다리는 상태가 되면 셧다운 단계를 진행한다. 
    - 이땐 공격이 가능하지 않다.



*/
[CreateAssetMenu(fileName = "PA_005", menuName = "SO/SkillItem/005_Drone", order = int.MaxValue)]
public class PA_005_Drone : SkillItemSO
{
    
    [Space(30)]
    [Header("Settings")]
    [Header("drone")]
    public int droneCount= 1;               // 생성하는 드론의 개수 
    public float spawnDelay_perDrone =  0.5f;   // 각 드론은 지연되어서 생성됨. 
    public float bootingTime_perDrone = 1f;          //드론이 생성되고 공격이 가능한 상태까지 기다리는 시간
    public float attackRange_drone = 7f;
    public float cep = 2f;  // Circular error probable  - 원형 공산 오차
    // public int attackRoutineCount = 3;      // 드론이 공격을 가하는 루틴의 횟수 - 이 루틴을 마치면 스킬이 종료됨. 
    [Header("Routine")]
    public float interval_perRoutine = 2f;     // 각 루틴 사이 간격 
    public float cost_perRoutine = 5f;           // 
    public int attackCount_perRoutine=6;         // 루틴별 투사체 공격 횟수

    [Header("Attack")]
    public float interval_perAttack = 0.3f;
    
    [Header("DroneProjectile")]
    public float arrivalTime_proj = 0.5f;    //총알 도착시간 
    public float curveIntensity_proj = 2f;   // 베지어 곡선 강도
    



    [Header("Spawn Projectiles")]
    public PlayerProjectile prefab_pp_drone;        // 드론 개체
    public PlayerProjectile prefab_pp_droneProjectile;      // 드론이 쏘는 투사체 

    //=====================================================================================


    protected PA_005_Drone()
    {
        skillType = SkillType.Scroll;
    }

    public override string description => "드론";



    public override string dataName => "드론";

    public override string id_base => $"PA_005_Drone";



    //============================================================================

    public override void OnEquip()
    {
        PoolManager.Instance.playerProjectilePoolSys.AddPoolItem(prefab_pp_drone);
        PoolManager.Instance.playerProjectilePoolSys.AddPoolItem(prefab_pp_droneProjectile);
    }


    public override void OnUnEquip()
    {
        PoolManager.Instance.playerProjectilePoolSys.RemovePoolItem( prefab_pp_drone );
        PoolManager.Instance.playerProjectilePoolSys.RemovePoolItem( prefab_pp_droneProjectile);
    }

    //=======================================================================================

    public override IEnumerator UseRoutine()
    { 
        //
        Player player = Player.Instance;
        List<PP_005_0_Drone> drones = new();
        
        // 
        yield return SpawnRoutine(drones);

        // 모든 드론이 사용불가가 될 때까지 기다림.  
        yield return new WaitUntil( ()=> drones.All(x=>x.currState == PP_005_0_Drone.State.Unavailable) ); 


        foreach(var drone in drones)
        {
            drone.StartCoroutine( drone.DeactivationRoutine(bootingTime_perDrone) );
        }

        // 모든 드론이 셧다운 다 될 때까지 기다림. 
        yield return new WaitUntil( ()=> drones.All(x=>x.currState == PP_005_0_Drone.State.Deactivated) ); 
    }

    //=====================================================================================================

    protected IEnumerator SpawnRoutine(List<PP_005_0_Drone> drones)
    {
        Vector3 initPos = Player.Instance.t.position;

        //
        for(int i=0;i<droneCount;i++)
        {
            float angle = i * (360f / droneCount );
            Vector3 initOffset = CalculateSatellitePosition(angle) + new Vector3(0,1,0);
            
            var pp_drone = PoolManager.Instance.playerProjectilePoolSys.GetPP<PP_005_0_Drone>(this, prefab_pp_drone, initPos, initPos);
            pp_drone.Follow(Player.Instance.t, initOffset);
            pp_drone.StartCoroutine( DroneLifeRoutine(pp_drone) );  // 생성하자마자 바로 드론 생애주기 

            drones.Add(pp_drone);

            // 다음 드론 
            yield return new WaitForSeconds(spawnDelay_perDrone);
        }
    }

    protected IEnumerator DroneLifeRoutine(PP_005_0_Drone drone)
    {
        yield return drone.ActivationRoutine(bootingTime_perDrone);

        yield return drone.AttackRoutine();
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
