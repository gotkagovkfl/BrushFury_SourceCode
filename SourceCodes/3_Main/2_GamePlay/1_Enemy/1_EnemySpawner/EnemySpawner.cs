using System.Collections;
using System.Collections.Generic;
using BW;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemySpawner : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer sr;
    public void Init(float radius)
    {
        transform.localPosition = new Vector3(0,0.01f, -radius);
        
        if( sr==null)
            sr = GetComponent<SpriteRenderer>();
    }

    public abstract IEnumerator SpawnEffect();
}
