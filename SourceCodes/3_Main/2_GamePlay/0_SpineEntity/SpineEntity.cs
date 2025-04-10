using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class SpineEntity : MonoBehaviour
{ 
    // public SpriteRenderer spriteRenderer;
    public SkeletonAnimation sa;
    public MeshFilter mf;
    public MeshRenderer mr;
    public Texture currTexture;

    [SerializeField] Transform t_;
    [SerializeField] Transform t_camera;


    public void Init()
    {
        sa = GetComponent<SkeletonAnimation>();
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        currTexture = mr.sharedMaterial.GetTexture("_MainTex");

        // mr.sortingLayerName = "SpriteEntity";
        // mr.sortingOrder = 1;

        t_ = transform;
        t_camera = Camera.main.transform;
    }

    /// <summary>
    /// 지정된 크기로 초기화 ( 스프라이트는 컴파일 때의 스프라이트로 사용 ) - 플레이어 초기화용
    /// </summary>
    /// <param name="entityWidth"></param>
    public void Init(float entityWidth, float entityHeight = 0)
    {
        Init();

        t_.localPosition = new Vector3(0,0,-entityWidth);
    }

    void Update()
    {              
        Billboard();
    }

    //==========================================================================================================

    /// <summary>
    /// 스프라이트가 항상 카메라를 정면으로 보도록 회전시킴. 
    /// </summary>
    void Billboard()
    {
        if (t_ == null)
        {
            return;
        }
        t_.rotation = Quaternion.LookRotation(t_.position - t_camera.position);
        t_.rotation = Quaternion.Euler(t_.rotation.eulerAngles.x,0,0);
    }

    /// <summary>
    /// dirX 방향으로 뒤집는다.  ( 오른쪽이 기본값 )
    /// </summary>
    /// <param name="dirX"></param>
    public void Flip(float dirX)
    {
        if(GameManager.isPaused)
        {
            return;
        } 
        if( dirX!=0)
        {
            sa.initialFlipX = dirX<0;
        }
        
    }
}
