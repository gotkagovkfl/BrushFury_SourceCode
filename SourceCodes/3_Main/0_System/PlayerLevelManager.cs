using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : Singleton<PlayerLevelManager>
{
    
    public GameObject expPrefab; // 경험치 아이템 프리팹
    public int maxExpObjects = 10; // 최대 생성 가능한 경험치 개수
    private List<GameObject> expList = new List<GameObject>(); // 현재 생성된 경험치 목록

    void SpawnExp(Vector3 position, int expValue)
    {
        if (expList.Count < maxExpObjects)
        {
            // 새 경험치 생성
            GameObject newExp = Instantiate(expPrefab, position, Quaternion.identity);

            expList.Add(newExp);
        }
    }






}
