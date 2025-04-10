using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 한 종류의 적을 소환하는 정보.
/// </summary>
[CreateAssetMenu(fileName = "EnemyContinuousSpawn", menuName = "SO/EnemySpawn/Continuous")]
public class ContinuousSpawnSO : EnemySpawnDataSO
{
    public override string dataName => $"단순 생성";
    //==============================================
    public Enemy enemyPrefab;
    public int spawnCount;

    public float delay;             
    public float spawnInterval;        // 소환 시간 간격
    public float duration;          // 총 소환 시간
    public float restartInterval;   // 해당 소환 재시작 시간 

    //=======================================================
    public override IEnumerator SpawnRoutine(Stage currStage,float startTime)
    {
        // Debug.Log($"[스폰 시작] {enemy.id} : {enemy.name}");
        yield return new WaitForSeconds(delay);  // 딜레이 후에 시작
        
        yield return new WaitUntil(()=>PoolManager.Instance.initialized);
        while(Time.time <= startTime + duration)
        {   
            // 스폰
            for(int i=0;i<spawnCount;i++)
            {
                // Debug.Log($"[스폰] {enemy.id} : {enemy.name}");
                Vector3 randPos = currStage.GetRandomNearbySpawnPoint();
                Enemy e = PoolManager.Instance.enemyPoolSys.GetEnemy(enemyPrefab,randPos);
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
        // Debug.Log($"[스폰 종료] {enemy.id} : {enemy.name}");
        //
        if(restartInterval>0)
        {
            yield return SpawnRoutine(currStage,Time.time);
        }
    }
}
