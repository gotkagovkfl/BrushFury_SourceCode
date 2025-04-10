using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

/// <summary>
/// 
/// </summary>
public class DropItemPoolSys : BwPoolSystem
{
    public const int MAX_EXP_COUNT = 50;
    public FastRemoveList<DI_000_Exp> existingExps = new();



    public DropItem GetDropItem(string poolId, float initValue, Vector3 initPos)
    {
        DropItem po = Get<DropItem>( poolId, initPos + new Vector3(0,0.2f,0) );
        po.Init(initValue);

        return po;
    }

    //===============================================================================================
    // 공짜 업그레이드 - 일단 능력치 강화창 열리게
    public DropItem GetLuckyPouch(float initValue, Vector3 initPos)
    {
        string poolId = $"{PoolType.DropItem}_004";
        return GetDropItem(poolId, initValue, initPos);
    }

    // 부적 - 맵 전체 폭발
    public DropItem GetAmulet(float initValue, Vector3 initPos)
    {
        string poolId = $"{PoolType.DropItem}_003";
        return GetDropItem(poolId, initValue, initPos);
    }



    // 자석 - 경험치 끌고옴.
    public DropItem GetMagnet(float initValue, Vector3 initPos)
    {
        string poolId = $"{PoolType.DropItem}_002";
        return GetDropItem(poolId, initValue, initPos);
    }


    // 체력 회복 
    public DropItem GetHpUp(float initValue, Vector3 initPos)
    {
        string poolId = $"{PoolType.DropItem}_001";
        return GetDropItem(poolId, initValue, initPos);
    }



    public DI_000_Exp GetExp(float initValue, Vector3 initPos)
    {
        // 스택조건
        if( existingExps.Count >= MAX_EXP_COUNT )
        {            
            //
            DI_000_Exp randomExp = existingExps.GetRandom();
            randomExp.StackExp( initValue );     // 기존 경험치에 값 합치기

            return randomExp;
        }
        // 일반 생성
        else
        {
            DI_000_Exp newExp = GetDropItem($"{PoolType.DropItem}_000",initValue, initPos)  as DI_000_Exp ;
            existingExps.Add(newExp);

            return newExp;
        }
    }
    
    //====================================================================

    public void RemoveExp( DI_000_Exp exp)
    {
        existingExps.Remove( exp );
    }
}






/// <summary>
/// 삽입, 삭제, 랜덤접근이 아주 빠르다. 
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class FastRemoveList<T>
{
    public int Count => list.Count;

    [SerializeField] private Dictionary<T, int> dic = new (); // 원소와 리스트의 인덱스 매치
    [SerializeField] private List<T> list = new (); // 빠른 랜덤 접근을 위한 리스트
    [SerializeField] private System.Random random = new ();

    //=======================================================================================
    public void Add(T item)
    {
        dic[item] = list.Count;
        list.Add(item);
    }

    /// <summary>
    /// 삭제할 원소를 리스트의 마지막 원소와 swap하여 삭제. (O(1) 보장)
    /// </summary>
    public bool Remove(T  item)
    {
        // 리스트에 없는 경우,
        if (dic.ContainsKey(item) == false)
        {
            return false;
        }
        
        // swap 대상 찾기
        int index = dic[item]; 
        int lastIndex = list.Count - 1;
        var lastItem = list[lastIndex]; 

        // swap
        list[index] = lastItem;
        dic[lastItem] = index;

        // remove
        list.RemoveAt(lastIndex);
        dic.Remove(item);

        return true;
    }

    public T GetRandom()
    {
        if (list.Count == 0)
            throw new InvalidOperationException("원소가 없는데 호출됨!");

        return list[random.Next(list.Count)];
    }

    public List<T> GetTotalItems()
    {
        return list;
    }



}