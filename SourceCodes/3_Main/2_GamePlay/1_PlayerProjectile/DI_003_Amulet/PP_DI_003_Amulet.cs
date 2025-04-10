using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BW;
using DG.Tweening;

public class PP_DI_003_Amulet: PlayerProjectile
{
    protected override string id => "DI_003";
    

    [Header("Setting")]
    SphereCollider _collider;
    
    [SerializeField] float startRadius = 5f;
    [SerializeField] float finalRadius = 50f;
    [SerializeField] float lifeTime;

    [SerializeField] ParticleSystem  ps;

    Sequence seq_explosion;

    //===================================================================
    protected override void Init_Custom()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.radius = startRadius;

        ps = GetComponent<ParticleSystem>();
        lifeTime = ps.main.startLifetime.constant;
        Explode();
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.TryGetComponent(out Enemy enemy))
        {
            Vector3 hitPoint = other.ClosestPoint(myTransform.position);
            enemy.GetDamaged(hitPoint, finalDmg);
        }
    }

    void Explode()
    {
        if( seq_explosion !=null && seq_explosion.IsActive())
        {
            seq_explosion.Kill();
        }


        seq_explosion = DOTween.Sequence()
        .Append( DOTween.To( () => _collider.radius,
            x => 
            {
                _collider.radius = x;
            },
            finalRadius,        // target alpha
            lifeTime   // 트위닝에 걸리는 시간
            )
        )
        .AppendCallback( DestroyProjectile)
        .Play();
    }


}
