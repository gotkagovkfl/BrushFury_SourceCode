using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private float timeScale = 1f;
    private Animator animator;

    public int hash_movementSpeed = Animator.StringToHash("MovementSpeed");
    public int hash_basicAttack = Animator.StringToHash("BasicAttack");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void OnMove(float magnitude)
    {
        animator.SetFloat(hash_movementSpeed, magnitude);
    }

    public void OnBasicAttackStart()
    {
        animator.SetTrigger(hash_basicAttack);
    }

    // public void OnBasicAttackFinish()
    // {
    //     animator.SetBool(hash_basicAttack,false);
    // }

}
