using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class GhostTrailEffect : SpineEffect
{
    Transform t_camera;

    protected override string id => "GhostTrail";

    public override float lifeTime => 0.5f;

    static string name_mainText = "_MainTex";
    static string name_alpha = "_Alpha";

    //============================================================================================== 
    protected override void Init_Custom()
    {
        t_camera = Camera.main.transform;
    
        SpineEntity playerSpineEntity = Player.Instance.spineEntity;
        myTransform.position = playerSpineEntity.transform.position;
        myTransform.localScale = playerSpineEntity.transform.localScale;

        mf.mesh = playerSpineEntity.mf.mesh;
        //
        var propertyBlock = new MaterialPropertyBlock();
        mr.GetPropertyBlock(propertyBlock);

        propertyBlock.SetTexture(name_mainText, playerSpineEntity.currTexture);
        mr.SetPropertyBlock(propertyBlock);

        float startAlpha = mr.material.GetFloat(name_alpha); 
        DOTween.To(
            () => startAlpha,
            newAlpha => 
            {
                propertyBlock.SetFloat(name_alpha, newAlpha);
                mr.SetPropertyBlock(propertyBlock);
            },
            0f,        
            lifeTime   
        ).Play();
    }

    void Update()
    {
        Billboard();
    }



     /// <summary>
    /// 스프라이트가 항상 카메라를 정면으로 보도록 회전시킴. 
    /// </summary>
    void Billboard()
    {
        myTransform.rotation = Quaternion.LookRotation(myTransform.position - t_camera.position);
        myTransform.rotation = Quaternion.Euler(myTransform.rotation.eulerAngles.x,0,0);
    }
}
