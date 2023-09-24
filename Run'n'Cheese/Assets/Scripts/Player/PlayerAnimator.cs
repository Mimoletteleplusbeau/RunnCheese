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
    [SerializeField] string WinAnimName;

    [Header("VFX")]
    [SerializeField] private GameObject _walkVFX;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
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
            case PlayerController.PlayerState.WinLevel:
                _animator.Play(WinAnimName);
                return;
            default:
                _animator.Play(IdleAnimName);
                break;
        }

        
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _spriteRenderer.flipX = Player.transform.position.x > mousePosition.x;
        //_walkVFX.SetActive(false);
    }

    public void WinAnimationEnd()
    {
        LevelsManager.Instance.GoToNextLevel();
    }
}
