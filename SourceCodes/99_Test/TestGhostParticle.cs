using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGhostParticle : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr_player;
    [SerializeField] ParticleSystem ps;

    [SerializeField ]  ParticleSystem.TextureSheetAnimationModule  textureSheetAnimation;


    void Start()
    {
        sr_player = Player.Instance.GetComponentInChildren<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();

        textureSheetAnimation = ps.textureSheetAnimation;
        textureSheetAnimation.enabled = true; // 활성화
        textureSheetAnimation.mode = ParticleSystemAnimationMode.Sprites; 
        textureSheetAnimation.SetSprite(0, sr_player.sprite); // 스프라이트 적용

    }

    void Update()
    {
        
    }
}
