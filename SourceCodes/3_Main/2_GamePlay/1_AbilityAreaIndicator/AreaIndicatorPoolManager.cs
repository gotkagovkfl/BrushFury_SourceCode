using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;

public class AreaIndicatorPoolManager : BwPoolSystem
{
    [SerializeField] ConeAreaIndicator prefab_cone;
    [SerializeField] CircleAreaIndicator prefab_circle;
    [SerializeField] SquareAreaIndicator prefab_square;


    Vector3 offset =  new Vector3(0,0.11f,0);

    // IEnumerator Start()
    // {
        // while(true)
        // {
        //     Vector3 randomStartPos = new Vector3( Random.Range(-10,10) , 0.01f, Random.Range(-10,10));
        //     Vector3 randomEndPos  = new Vector3( Random.Range(-10,10) , 0.01f, Random.Range(-10,10));

        //     Vector3 dir = randomEndPos - randomStartPos;
            
        //     GetCone(null, randomStartPos, dir, 1f, 2, 30);

        //     yield return new WaitForSeconds(3f);
        // }

    // }

    Vector3 InitPos(Vector3 inputPos)
    {
        return inputPos.WithFloorHeight() + offset;
    }


    public ConeAreaIndicator GetCone(Enemy enemy, Vector3 initPos, Vector3 dir, float duration, float radius, float angle, bool destroyOnEnemyDeath = true)
    {
        dir = dir.WithFloorHeight().normalized; // 회전 벡터는 보통 정규화해서 사용
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, dir);

        ConeAreaIndicator  cone = Get<ConeAreaIndicator>(prefab_cone.poolId, InitPos( initPos) );
        cone.myTransform.rotation = rot;
        cone.Init(enemy, duration, destroyOnEnemyDeath, radius, angle);


        return cone;

    }

    public CircleAreaIndicator GetCircle(Enemy enemy, Vector3 initPos, float duration, float radius, bool destroyOnEnemyDeath = true)
    {
        CircleAreaIndicator circle = Get<CircleAreaIndicator>(prefab_circle.poolId, InitPos(initPos));
        circle.Init(enemy, duration, destroyOnEnemyDeath, radius );

        return circle;
    }

    public SquareAreaIndicator GetSquare(Enemy enemy, Vector3 initPos, Vector3 dir, float duration, float width, float height , bool destroyOnEnemyDeath = true)
    {
        dir = dir.WithFloorHeight().normalized; // 회전 벡터는 보통 정규화해서 사용
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, dir);


        SquareAreaIndicator  square = Get<SquareAreaIndicator>(prefab_square.poolId,InitPos(initPos));
        square.myTransform.rotation = rot;
        square.Init(enemy, duration,destroyOnEnemyDeath, width, height);

        return square;
    }
}
