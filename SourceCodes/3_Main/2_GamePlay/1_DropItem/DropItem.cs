using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent( typeof(SpriteEntity))]
public abstract class DropItem : BwPoolObject
{
    public DropItemDataSO data;
    public override PoolType poolType => PoolType.DropItem; 
    protected override string id =>data.id;  // 

    Vector3 originScale;

    SpriteEntity spriteEntity;
    Rigidbody rb;
    Collider itemCollider;


    [SerializeField] protected float value; // 해당 아이템 효과의 수치 ( ex, 경험치 or 회복량 등)

    // 속도 관련
    float currSpeed; // 아이템이 플레이어 쪽으로 이동하는 속도 
    readonly float initSpeed = 3;
    readonly float maxSpeed = 30;

    bool captured;   //

    bool inRange // 해당 아이템이 플레이어
    {
        get
        {
            float distSqr = Vector3.SqrMagnitude( Player.Instance.t.position - myTransform.position );
            float range = Player.Instance.status.finalPickUpRange;
            float rangeSqr = range * range ;  // 일단 그리기 반경
            //Debug.Log($" item  {distSqr} {rangeSqr}" );
            //
            if ( distSqr <= rangeSqr  && captured == false) 
            {
                // Debug.Log("캡처!");
                return true;
            }
            return false;

        }
    }


    //==============================================================================
    public override void OnCreatedInPool()
    {
        spriteEntity = GetComponent<SpriteEntity>();
        rb = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();

        originScale = transform.localScale;
        // Debug.Log("풀에서 생성");
    }

    public override void OnGettingFromPool()
    {
        captured = false;
        rb.velocity = Vector3.zero;

        transform.localScale = originScale;
        // Debug.Log("풀에서 재생성");
    }


    //====================================
    protected abstract void Init_Custom();

    protected abstract void Return_Custom();






    
    /// <summary>
    /// 초기화 - 
    /// </summary>
    /// <param name="itemData"></param> - 
    /// <param name="value"></param>    
    public void Init(float value) 
    {
        
        this.value = value;

        currSpeed = initSpeed;
        spriteEntity.Init(0.5f,0.5f);

        Vector3 dir = new Vector3( BW.Math.GetRandom(-1,1 ),0,BW.Math.GetRandom(-1,1 ) ).normalized;
        rb.AddForce(dir*3f, ForceMode.Impulse);

        Init_Custom();
    }


    void Update()
    {
        // 아이템이 범위 안인지
        if(inRange)
        {
            captured = true;
        }


        // 캡처시 플레이어쪽으로 이동
        if(captured)
        {
            
            // 방향구하기
            Vector3 dir = (Player.Instance.t.position - myTransform.position).normalized;
            
            rb.velocity = dir * currSpeed;

            currSpeed = Mathf.Lerp(currSpeed, maxSpeed,Time.deltaTime);  // 아이템 속도가 점점 빨라짐
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Player") )
        {
            PickUp();
        }
    }



    //================================
    public void PickUp()
    {
        data.PickUp(this,value);

        GenerateEffect();

        //
        SoundManager.Instance.Play(myTransform, data.sfx_pickup);
        PoolManager.Instance.dropItemPoolSys.Return(this);

        Return_Custom();
    }


    public void SetCaptured()
    {
        captured = true;
    }


    void GenerateEffect()
    {
        if( data.prefab_pickupEffect !=null)
        {
            Vector3 initPos = myTransform.position;
            PoolManager.Instance.effectPoolSys.GetEffect(data.prefab_pickupEffect,initPos);
        }
    }


}
