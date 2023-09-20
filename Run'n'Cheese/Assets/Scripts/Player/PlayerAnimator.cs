using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController Player;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    [Header("Animation Names")]
    [SerializeField] string IdleAnimName;
    [SerializeField] string WalkAnimName;
    [SerializeField] string JumpAscentAnimName;
    [SerializeField] string JumpDescentAnimName;

    [Header("VFX")]
    [SerializeField] private GameObject _walkVFX;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        _spriteRenderer.flipX = !Player.Flipped;
        //_walkVFX.SetActive(false);

        switch (Player.MyState)
        {
            case PlayerController.PlayerState.Idle:
                _animator.Play(IdleAnimName);
                break;
            case PlayerController.PlayerState.Walk:
                _animator.Play(WalkAnimName);
                //_walkVFX.SetActive(true);
                //CustomEvent.Trigger(_walkVFX, "Run");
                break;
            case PlayerController.PlayerState.JumpAscent:
                _animator.Play(JumpAscentAnimName);
                break;
            case PlayerController.PlayerState.JumpDescent:
                _animator.Play(JumpDescentAnimName);
                break;
            default:
                _animator.Play(IdleAnimName);
                break;
        }
    }
}
