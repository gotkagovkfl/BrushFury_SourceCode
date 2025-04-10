using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using System;
using Unity.VisualScripting;
using BW;





public abstract class AreaIndicator : BwPoolObject
{   
    public override PoolType poolType => PoolType.AreaIndicator;
    
    
    
    
    
    [Header("Default Setting")]   
    Enemy enemy;
    [SerializeField] protected float param_0;
    [SerializeField] protected float param_1; 

    [SerializeField] protected float fillDuration;
    [SerializeField] protected float cleanUpDuration = 0.25f;

    [SerializeField] protected Material baseMaterial;
    [SerializeField] protected Material outlineMaterial;
    
    //
    [Space(40)]
    [Header("Form Setting")]  
    [SerializeField]  protected LineRenderer outlineRenderer;

    // 전체 영역
    [SerializeField]  protected MeshFilter   fullSectorFilter;
    [SerializeField]  protected MeshRenderer fullSectorRenderer;

    // 현재 영역
    [SerializeField]  protected MeshFilter   currSectorFilter;
    [SerializeField]  protected MeshRenderer currSectorRenderer;

    //
    [Space(40)]
    [Header("Color Setting")]
    [SerializeField]  protected Color outlineColor = Color.red;
    [SerializeField]  protected Color fullSectorColor = new Color(1, 0, 0, 0.4f); 
    [SerializeField]  protected Color currSectorColor = new Color(1, 0, 0, 0.8f); 


    [Header("머티리얼 컬러 프로퍼티 이름")]
    [Tooltip("URP/Lit은 _BaseColor, Legacy Standard는 _Color 등이 일반적")]
    [SerializeField]  protected string colorPropertyName = "_Color";

    // 내부적으로 관리할 메쉬
    protected Mesh _fullSectorMesh;   // 최대 범위용
    protected Mesh _currSectorMesh;   // 0→radius 애니메이션용

    // MPB
    protected MaterialPropertyBlock _mpbFullMesh;
    protected MaterialPropertyBlock _mpbCurrMesh;


    //
    bool destroyOnEnemyDeath;
    static WaitForFixedUpdate wffu = new();

    //=================================================================
    public override void OnCreatedInPool()
    {

    }

    public override void OnGettingFromPool()
    {
        
    }

    //===================================================================
    public void Init(Enemy enemy, float duration, bool destroyOnEnemyDeath,  float param_0, float param_1 = 0)
    {
        // 파라미터 초기화
        this.enemy = enemy;
        this.fillDuration = duration;
        this.destroyOnEnemyDeath = destroyOnEnemyDeath;
        this.param_0 = param_0;
        this.param_1 = param_1;
        

        // 우선 메쉬 생성 후 할당
        DrawOutline();
        
        _fullSectorMesh = new Mesh { name = "FullSectorMesh" };
        GenerateSectorMesh(_fullSectorMesh, 1);  
        _currSectorMesh = new Mesh { name = "CurrSectorMesh" };
        GenerateSectorMesh(_currSectorMesh, 0);


        if (fullSectorFilter)
            fullSectorFilter.mesh = _fullSectorMesh;

        if (currSectorFilter)
            currSectorFilter.mesh = _currSectorMesh;



        // 머티리얼 설정. 
        if (outlineRenderer)
        {
            outlineRenderer.sharedMaterial = outlineMaterial;
        }

        if (fullSectorRenderer)
        {
            fullSectorRenderer.sharedMaterial = baseMaterial;

            _mpbFullMesh = new MaterialPropertyBlock();
            _mpbFullMesh.SetColor(colorPropertyName, fullSectorColor);
            fullSectorRenderer.SetPropertyBlock(_mpbFullMesh);
        }
        if (currSectorRenderer)
        {
            currSectorRenderer.sharedMaterial = baseMaterial;

            _mpbCurrMesh = new MaterialPropertyBlock();
            _mpbCurrMesh.SetColor(colorPropertyName, currSectorColor);
            currSectorRenderer.SetPropertyBlock(_mpbCurrMesh);
        }

        // 채우기 루팅 시작.
        StartCoroutine(LifeRoutine());
    }

    IEnumerator LifeRoutine()
    { 
        yield return FillRoutine();
        yield return CleanUpRoutine();

        //
        PoolManager.Instance.areaIndicatorPoolManager.Return(this);
    }


    //내부 메쉬가 점점 채워짐. 
    IEnumerator FillRoutine()
    {
        float elapsed = 0f;
        while (elapsed < fillDuration)
        {
            //
            if( destroyOnEnemyDeath && enemy !=null && enemy.isAlive == false)
            {
                yield break;
            }
            
            //
            float progress = Mathf.Clamp01(elapsed / fillDuration);
            GenerateSectorMesh(_currSectorMesh, progress);
            
            //
            elapsed += Time.fixedDeltaTime;
            yield return wffu;
        }


    }

    // 투명해짐. 
    IEnumerator CleanUpRoutine()
    {
        float elapsed = 0f;
        while (elapsed <cleanUpDuration)
        {
            //
            float progress = Mathf.Clamp01( 1- elapsed*2 / fillDuration);

            // outline
            float targetAlpha = outlineRenderer.startColor.a * progress;
            Color targetColor = outlineRenderer.startColor.WithAlpha(targetAlpha);
            outlineRenderer.startColor = targetColor;
            outlineRenderer.endColor = targetColor;

            // full
            targetAlpha = fullSectorColor.a * progress;
            targetColor = fullSectorColor.WithAlpha(targetAlpha);
            _mpbFullMesh.SetColor(colorPropertyName, targetColor);
            fullSectorRenderer.SetPropertyBlock(_mpbFullMesh);

            // curr
            targetAlpha = currSectorColor.a * progress;
            targetColor = currSectorColor.WithAlpha(targetAlpha);
            _mpbCurrMesh.SetColor(colorPropertyName, targetColor);
            currSectorRenderer.SetPropertyBlock(_mpbCurrMesh);
            
            
            //
            elapsed += Time.fixedDeltaTime;
            yield return wffu;
        }
    
        //
    }


    protected abstract void GenerateSectorMesh(Mesh targetMesh, float progress);      // 매쉬 모양 생성 등. 
    protected abstract void DrawOutline();




    //===============================================================================================================
    public abstract void OnUpdateTargetPos(Vector3 targetPos);

    public void RotateTo(Vector3 targetPos)
    {
        Vector3 startPos = myTransform.position;
        Vector3 dir = (targetPos-startPos).WithFloorHeight().normalized; // 회전 벡터는 보통 정규화해서 사용
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, dir);
        myTransform.rotation = rot;
    }

    public void MoveTo(Vector3 targetPos)
    {
        myTransform.position = new Vector3(targetPos.x,initPos.y,targetPos.z);
    }
}
