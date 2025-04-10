using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using DG.Tweening;
using BW;
using System.Reflection;

public class BattleStage : Stage
{
    [Space (50) ]
    [Header("UI")]
    [SerializeField] Canvas canvas;
    [SerializeField] GamePlayStartUI gamePlayStartUI;    // 스테이지 시작시 안내창
    [SerializeField] StageTimer stageTimer;
    [SerializeField] StageGoalText stageText;


    // [SerializeField] EntrancePortal entrancePortal;

    
    [Header("Battle Settings")]
    [SerializeField] Transform t_bossSpawnPoint;

    [SerializeField] StageMissionSO missionInfo;
    public float battleStartTime;
    // public float stageFinishTime;
    public float stagePlayTime => Time.time - battleStartTime;


    // public SerializableDictionary<string, int> currTargets = new();

    List<Coroutine> enemySpawnRoutines;


    // InteractiveObject survivalReward;

    [Header("Boundary Setting")]
    [SerializeField] GameObject prefab_tree;
    

    //===========================================================================


    protected override void Init_Custom()
    {
        Debug.Log("전투 스테이지 초기화 ");

        stageTimer.Init(this);
        stageText.Init();
 
    }
    public override IEnumerator StageStartSequence()
    {
        
        canvas.gameObject.SetActive(true);

        string text = (GameManager.Instance.userData.currStageNum+1).ToString();
        Sequence startSequence = gamePlayStartUI.GetSeq_GamePlayStart(text);
        // startSequence.Play();

        // yield return new WaitUntil(() => startSequence.IsActive() == false);
        yield return null;

        // canvas.gameObject.SetActive(false);
        SoundManager.Instance.Invoke(transform, SoundEventType.BGM_Battle);
    }

    // public void FinishStageInstantly()
    // {
    //     stageFinishTime = Time.time+2;
    // }

    protected override void StartStage_Custom()
    {
        StartMission();


        stageTimer.Run();
    }

    protected override void FinishStage_Custom()            // 나중에 여기에 보스 제거 연출 나와야함. 
    {
        // UI세팅
        stageTimer.Stop();
        stageText.Actiavte("단계 완료!");

        // 모든 생성 루틴 종료.
        foreach(Coroutine c in enemySpawnRoutines)
        {
            StopCoroutine(c);
        }

        
        // 
        GeneratePortal();

        //
        GamePlayManager.Instance.OnStageClear();    // 이거 ui잖아. 
    }


    // public override bool IsStageFinished()
    // {
    //     return stagePlayTime <=0;
    // }

    ///====================================================
    ///
    void StartMission()
    {
        BossSpawnData bossSpawnData  = missionInfo.bossSpawnData;
        
        List<EnemySpawnDataSO> enemySpawnData  = missionInfo.enemySpawnData;
        
        enemySpawnRoutines=new();
        foreach( EnemySpawnDataSO spawn in enemySpawnData)
        {
            enemySpawnRoutines.Add( StartCoroutine( spawn.SpawnRoutine(this,Time.time) ));
        }

        StartCoroutine( BossSpwanRoutine(bossSpawnData));


        battleStartTime =Time.time;
    }


    IEnumerator BossSpwanRoutine(BossSpawnData bossSpawnData)
    {
        float bossSpawnTime = bossSpawnData.bossSpawnTime;

        yield return new WaitForSeconds(bossSpawnTime);
        
        SpawnBoss(bossSpawnData.bossPrefab);
    }

    void SpawnBoss(Enemy bossPrefab)
    {
        Vector3 spawnPos = t_bossSpawnPoint.position;
        PoolManager.Instance.enemyPoolSys.GetEnemy(bossPrefab,spawnPos);
    }
    
    void GeneratePortal()
    {
        // 챕터 마지막포탈의 경우. 
        ChapterLastPortal portal = Instantiate(ResourceManager.Instance.prefab_stagePortal).GetComponent<ChapterLastPortal>();
        portal.Init( 2 );
        portal.transform.position =  portalPos;
    }
}
