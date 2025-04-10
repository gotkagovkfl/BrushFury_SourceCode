using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

using BW;
using System.Runtime.InteropServices.WindowsRuntime;

public abstract class Stage : Singleton<Stage>
{
    // protected StageNode nodeData;

    // public bool hasStageReward => nodeData.rewardInfo.dat
    
    public bool isFinished {get;protected set;}


    // 형태
    // [SerializeField] LineRenderer lineRenderer;
    [SerializeField] protected NavMeshSurface navMeshSurface;
    [SerializeField] protected Transform t_playerSpawnPoint;
    [SerializeField] protected Transform t_portalPos;
    [SerializeField] protected Transform t_enemySpawnAreaParent;

    public Vector3 playerInitPos => t_playerSpawnPoint.position;
    public Vector3 portalPos => t_portalPos.position;

    [SerializeField] protected BoxCollider[] enemySpawnArea;
    [SerializeField] protected BoxCollider[] boundary;

    [SerializeField] protected BoxCollider cameraConfiner;


    // public WaveActivationSwitch waveActivationSwitch;

    // public StagePortal stagePortal;
    // public SelectableItemList selectableItemList;
    protected Vector3 navCenterPos=> navMeshSurface.transform.position;


    //================================================================================================================
    public void Init()
    {
        enemySpawnArea = t_enemySpawnAreaParent.GetComponentsInChildren<BoxCollider>();
        // lineRenderer = GetComponent<LineRenderer>();
        navMeshSurface = GetComponentInChildren<NavMeshSurface>();

        InitByMapInfo();

        Init_Custom();
    }

    protected abstract void Init_Custom();




  

    //========================================================================
    
    
    // public IEnumerator StageRoutine()
    // {
    //     StartStage();
    //     yield return null;
    //     yield return new WaitUntil(()=>IsStageFinished()==true);
        
    //     FinishStage();
    // }

    
    

    
    
    public abstract IEnumerator StageStartSequence(); 


    public void StartStage()
    {
        isFinished = false;

        StartStage_Custom();
    }

    protected abstract void StartStage_Custom(); 


    /// <summary>
    /// 스테이지 종료 - 
    /// </summary>
    public void FinishStage()
    {
        isFinished  = true;
        //
        
        // GeneratePortal();       // 포탈생성 

        //
        FinishStage_Custom();
    }

    protected abstract void FinishStage_Custom();



    /// <summary>
    /// 스테이지 클리어인지.
    /// </summary>
    /// <returns></returns>
    // public abstract bool IsStageFinished();

    //=============================================================================




























    


    //=========================================================================================
    #region 좌표
    /// <summary>
    /// 해당 영역에서 임의의 좌표를 얻는다. 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomSpawnPoint()
    {
        Vector3 ret = Player.Instance.t.position;

        if (enemySpawnArea.Length>0)
        {
            int randIdx = Random.Range(0,enemySpawnArea.Length);
            BoxCollider area = enemySpawnArea[randIdx];

            Bounds bounds = area.bounds;

            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            ret = new Vector3(randomX, 0, randomZ);
        }

        return ret;
    }
    
    float spawnWidth = 40;
    float spawnHeight = 40;


    /// <summary>
    /// 소환 지점의 무작위성을 감소시킴.  - 현재 플레이어 위치와 너무 먼 곳에서 적이 소환되는 걸 막기위함. 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomNearbySpawnPoint()
    {
        Vector3 center = Player.Instance.t.position;

        Bounds bounds = navMeshSurface.navMeshData.sourceBounds;
        Vector3 offset = navCenterPos;

        //
        float halfWidth = spawnWidth*0.5f;
        float halfHeight = spawnHeight*0.5f;

        float xMin = offset.x + bounds.min.x + halfWidth;
        float xMax = offset.x + bounds.max.x - halfWidth;

        float zMin = offset.x + bounds.min.z + halfHeight;
        float zMax = offset.x + bounds.max.z - halfHeight;
        //

        center  = new Vector3( Mathf.Clamp(center.x, xMin, xMax), 0,   Mathf.Clamp(center.z, zMin, zMax)     );

        float randomX = Math.GetRandom(center.x - halfWidth, center.x + halfWidth);
        float randomZ = Math.GetRandom(center.z - halfHeight, center.z + halfHeight);


        return new Vector3( randomX, 0, randomZ );
    }
    
    
    
    
    public Color gizmoColor = Color.green; // 기즈모 색상

    void OnDrawGizmos()
    {
        if ( Player.Instance.t == null )
        {
            return;
        }
        
        Vector3 center = Player.Instance.t.position;

        Bounds bounds = navMeshSurface.navMeshData.sourceBounds;
        Vector3 offset = navCenterPos;

        //
        float halfWidth = spawnWidth*0.5f;
        float halfHeight = spawnHeight*0.5f;

        float xMin = offset.x + bounds.min.x + halfWidth;
        float xMax = offset.x + bounds.max.x - halfWidth;

        float zMin = offset.x + bounds.min.z + halfHeight;
        float zMax = offset.x + bounds.max.z - halfHeight;
        //


        center  = new Vector3( Mathf.Clamp(center.x, xMin, xMax), 0,   Mathf.Clamp(center.z, zMin, zMax)     );
        Gizmos.color = gizmoColor;

        // 네 꼭짓점을 계산
        Vector3 topLeft     = new Vector3(center.x - halfWidth  ,   0, center.z + halfHeight);
        Vector3 topRight    = new Vector3(center.x + halfWidth  ,   0, center.z + halfHeight);
        Vector3 bottomLeft  = new Vector3(center.x - halfWidth  ,   0, center.z - halfHeight);
        Vector3 bottomRight = new Vector3(center.x + halfWidth  ,   0, center.z - halfHeight);

        // 사각형의 네 변을 그리기
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
    
    
    
    
    
    
    /// <summary>
    /// targetPos로부터 dist 만큼의 거리가 있는 임의의 접근 가능한 지점을 반환한다. 
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="dist"></param>
    /// <returns></returns>
    public Vector3 GetRandomNearbyPoint(Vector3 targetPos, float dist = 3)
    {
        Vector3 randomSpawnPos = GetRandomSpawnPoint();
        Vector3 dir = (randomSpawnPos - targetPos).normalized;
        // Vector3 dir = new Vector3(50,0,0);
        Vector3 ret = targetPos + dir* dist;
        
        if( IsInBoundary(ret)== false)
        {
            ret =  ClampToBoundary(ret);
        }

        return ret;
    }


    // Helper method to check if a point is within the boundary
    private bool IsInBoundary(Vector3 point)
    {
        Bounds bounds = navMeshSurface.navMeshData.sourceBounds;
        Vector3 offset = navCenterPos;

        float offsetBonus = 5;

        float xMin = offset.x + bounds.min.x;
        float xMax = offset.x + bounds.max.x;

        float zMin = offset.x + bounds.min.z;
        float zMax = offset.x + bounds.max.z;

        //
        if( bounds.size.x >offsetBonus*2)
        {
            xMin +=offsetBonus;
            xMax -=offsetBonus;
            zMin +=offsetBonus;
            zMax -=offsetBonus; 
        }


    
        return point.x >= xMin && point.x <= xMax &&
            point.z >= zMin && point.z <= zMax;
    }

    // Helper method to clamp the point to the boundary
    private Vector3 ClampToBoundary(Vector3 point)
    {
        Bounds bounds = navMeshSurface.navMeshData.sourceBounds;
        Vector3 offset = navCenterPos;

        float offsetBonus = 5;

        float xMin = offset.x + bounds.min.x;
        float xMax = offset.x + bounds.max.x;

        float zMin = offset.x + bounds.min.z;
        float zMax = offset.x + bounds.max.z;

        //
        if( bounds.size.x >offsetBonus*2)
        {
            xMin +=offsetBonus;
            xMax -=offsetBonus;
            zMin +=offsetBonus;
            zMax -=offsetBonus; 
        }


        return new Vector3(
            Mathf.Clamp(point.x, xMin, xMax),
            point.y,
            Mathf.Clamp(point.z, zMin, zMax)
        );
    }


#endregion












    //=======================================================================
    #region 형태
    void InitByMapInfo()
    {
        float width = GameConfig.stageSize.x;
        float height = GameConfig.stageSize.y;
        
        ResizeNavMesh(width,height);

        // replace objects by offset;
        InitStageObjects(width, height);
        ResizeEnemySpawnArea(width,height);
        
        // init boundary
        AdjustBoundaryCollider();

        MainVCam.Instance.InitCameraConfiner( cameraConfiner, navCenterPos, width, height);
    }

    void InitStageObjects(float width, float height)
    {
        t_playerSpawnPoint.position = navCenterPos; 
        // t_portalPos.position = centerPos + new Vector3(0,0, height * 0.5f * 0.75f);
        t_portalPos.position = navCenterPos;
    }

    void ResizeNavMesh(float width, float height)
    {
        navMeshSurface.size = new Vector3(width, navMeshSurface.size.y, height);
        navMeshSurface.BuildNavMesh();
    }

    void ResizeEnemySpawnArea(float width, float height)
    {
        // BoxCollider 크기 설정 (로컬 좌표 기준)
        foreach(BoxCollider area in enemySpawnArea)
        {
            area.size = new Vector3(width, area.size.y, height);
            area.transform.position = navCenterPos +  new Vector3(navMeshSurface.center.x, -2, navMeshSurface.center.z) ;
        }     

    }


    void AdjustBoundaryCollider()
    {
        // NavMesh 경계 가져오기
        float thickness  = 3;
        float height = 10;
        Bounds bounds = navMeshSurface.navMeshData.sourceBounds;
        
        // 각 경계에 큐브 배치
        ResizeBoundaryCube("Top",       boundary[0], new Vector3(0, 0, bounds.max.z + thickness*0.5f), new Vector3(bounds.size.x + thickness, height, thickness  ));
        ResizeBoundaryCube("Bottom",    boundary[1], new Vector3(0, 0, bounds.min.z - thickness*0.5f), new Vector3(bounds.size.x + thickness, height, thickness  ));
        ResizeBoundaryCube("Left",      boundary[2], new Vector3(bounds.min.x - thickness*0.5f, 0, 0), new Vector3(thickness  , height, bounds.size.z + thickness));
        ResizeBoundaryCube("Right",     boundary[3], new Vector3(bounds.max.x + thickness*0.5f, 0, 0), new Vector3(thickness  , height, bounds.size.z + thickness));
    }

    void ResizeBoundaryCube(string name, BoxCollider collider, Vector3 position, Vector3 scale)
    {
        // 위치 및 스케일 설정
        collider.transform.position = navCenterPos + position;
        collider.size = scale;
    }
    #endregion


}
