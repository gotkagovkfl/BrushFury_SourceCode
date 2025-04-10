using System.Collections;
using System.Collections.Generic;
using BW;
using UnityEngine;

public class PP_005_1_DroneProjectile : PlayerProjectile
{
    protected override string id => "005_1";

    protected float arrivalTime_proj;
    protected float curveIntensity;


    protected Vector3 controlPoint;

    [SerializeField] TrailRenderer tr;

    //=============================================================================
    public override void OnCreatedInPool()
    {
        tr = GetComponentInChildren<TrailRenderer>();    
        tr.autodestruct = false; 
    }

    public override void OnGettingFromPool()
    {
        // tr.enabled = true; 
    }

    protected override void Init_Custom()
    {
        if (baseData is PA_005_Drone droneData )
        {
            arrivalTime_proj = droneData.arrivalTime_proj;
            curveIntensity = droneData.curveIntensity_proj;
        }



 
        //
        controlPoint = GetControlPoint(initPos,targetPos, curveIntensity );
        StartCoroutine(BezierMove(arrivalTime_proj ));
    }

    IEnumerator BezierMove(float duration, float accelerationPower = 1.3f)
    {
        float elapsed = 0;

        // yield return null;  

        while (elapsed < duration)
        {
            // 0~1 사이의 t 값 계산 (가속 적용)
            float t = Mathf.Pow(elapsed / duration, accelerationPower); 

            // 현재 위치 & 다음 위치 예측
            Vector3 currentPosition = BezierPoint(t, initPos, controlPoint, targetPos);
            float nextT = Mathf.Pow(Mathf.Min((elapsed + Time.fixedDeltaTime) / duration, 1f), accelerationPower);
            Vector3 nextPosition = BezierPoint(nextT, initPos, controlPoint, targetPos);

            // 이동 방향 계산
            Vector3 direction = (nextPosition - currentPosition).normalized;
            if (direction != Vector3.zero)
            {
                // 오른쪽(transform.right)이 이동 방향을 바라보게 회전
                myTransform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
            }

            // 위치 업데이트
            myTransform.position = currentPosition;
            elapsed += Time.fixedDeltaTime;

            yield return GameConstants.waitForFixedUpdate;
        }

        // 마지막 위치 보정
        myTransform.position = targetPos;
        myTransform.rotation = Quaternion.FromToRotation(Vector3.right, targetPos - controlPoint);
        
        
        StartCoroutine(Explode());
    }


    private Vector3 GetControlPoint(Vector3 start, Vector3 end, float intensity)
    {
        Vector3 midPoint = (start + end) *0.5f; 
        Vector3 randomOffset = new Vector3(
            Math.GetRandom(-intensity, intensity),
            Math.GetRandom(0, intensity),               // 음수가 되면 땅 아래에 박힘.
            Math.GetRandom(-intensity, intensity)
        ); 
        return midPoint + randomOffset; 
    }

    private Vector3 BezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        return (uu * p0) + (2 * u * t * p1) + (tt * p2);;
    }

    //=================================================================================================

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.TryGetComponent(out Enemy enemy))
        {
            StartCoroutine(Explode());
        }
    }
    

    IEnumerator Explode()
    {
        // Debug.Log("펑");
        
        yield return null;  // 나중에 폭발 이펙트 이후에 파괴할거임. 
        tr.Clear();
        DestroyProjectile();
    }

}
