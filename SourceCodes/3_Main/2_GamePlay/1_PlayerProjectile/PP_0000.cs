using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using BW;
using UnityEngine;

public class PP_0000 : PlayerProjectile
{
    protected override string id => "0000";
    

    [Header("Setting")]
    
    [SerializeField] float radius;
    [SerializeField] float attackDetectionAngle;
    
    [SerializeField] ParticleSystem  ps;

    // [SerializeField] GameObject slashEffect;

    private List<Collider> detectedTargets=new(); // 감지된 충돌체 저장


    //===================================================================
    protected override void Init_Custom()
    {
        radius = ps.main.startSize.constant ;
        attackDetectionAngle = 180;     // 이건 평타마다 설정해야함. 
        
        
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg - 90f;
        myTransform.rotation =  Quaternion.Euler(0, angle, 0);
        
        StartCoroutine(SlashRoutine());
        StartCoroutine(DestroyRoutine( ps.main.duration));
    }


    IEnumerator SlashRoutine()
    {
        yield return new WaitForSeconds(0.05f);

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, GameConstants.enemyLayer); // 반경 내의 모든 충돌체 탐색

        detectedTargets.Clear();
        int hitCount = 0;
        foreach (Collider hit in hits)
        {
            if( hit.TryGetComponent(out Enemy enemy))
            {
                Vector3 toTarget = (enemy.myTransform.position - myTransform.position).normalized; // 타겟까지의 방향 벡터
                float dot = Vector3.Dot(myTransform.right, toTarget); // 기준 방향과 타겟 방향의 내적
                float targetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg; // 내적을 각도로 변환

                if (targetAngle <= attackDetectionAngle *0.5f)  // 부채꼴 범위 내에 있는지 확인
                {
                    detectedTargets.Add(hit);
                    // Debug.Log($"Detected: {hit.name}");

                    Vector3 hitPoint = hit.ClosestPoint( Player.Instance.t.position );
                    enemy.GetDamaged( hitPoint, finalDmg);

                    // var e = Instantiate( slashEffect, hitPoint,Quaternion.identity);
                    // Vector3 direction = Player.Instance.t.position - transform.position;
                    
                    // 방향을 반전하여 -Z가 바라보도록 수정
                    // e.transform.rotation = Quaternion.LookRotation(-direction);



                    hitCount++;
                }
            }
            
        }

                //
        if (hitCount > 0)
        {
            Player.Instance.RecoverInkWithBasicAttack(hitCount);
        }
    }







    // ✅ 기즈모로 부채꼴 시각화
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
          if (!Application.isPlaying) return;

        Gizmos.color = Color.yellow;

        // 부채꼴을 이루는 점들 계산
        int arcResolution = 20; // 부채꼴의 부드러움 정도
        Vector3 startDirection = Quaternion.Euler(0, -attackDetectionAngle * 0.5f, 0) * transform.right;
        Vector3 prevPoint = transform.position + startDirection * radius;

        for (int i = 1; i <= arcResolution; i++)
        {
            float lerpFactor = (float)i / arcResolution;
            float currentAngle = Mathf.Lerp(-attackDetectionAngle * 0.5f, attackDetectionAngle * 0.5f, lerpFactor);
            Vector3 nextDirection = Quaternion.Euler(0, currentAngle, 0) * transform.right;
            Vector3 nextPoint = transform.position + nextDirection * radius;

            Gizmos.DrawLine(prevPoint, nextPoint); // 곡선 연결
            prevPoint = nextPoint;
        }

        // 부채꼴의 양쪽 경계선
        Vector3 leftBoundary = Quaternion.Euler(0, -attackDetectionAngle * 0.5f, 0) * transform.right * radius;
        Vector3 rightBoundary = Quaternion.Euler(0, attackDetectionAngle * 0.5f, 0) * transform.right * radius;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);

        // 감지된 대상 강조 표시
        Gizmos.color = Color.red;
        foreach (Collider target in detectedTargets)
        {
            Gizmos.DrawSphere(target.transform.position, 0.2f);
        }
    }
#endif
}
