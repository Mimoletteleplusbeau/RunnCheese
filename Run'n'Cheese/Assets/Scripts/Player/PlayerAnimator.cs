using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.VFX;

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
    [SerializeField] private VisualEffect _walkVFX;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Player.OnStateChange += CheckRunVFX;
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

        
        Vector2 mousePosition = Input.mousePosition;
        Vector2 playerPosition = Camera.main.WorldToScreenPoint(Player.transform.position);
        _spriteRenderer.flipX = playerPosition.x > mousePosition.x;
    }

    public void WinAnimationEnd()
    {
        LevelEndUIManager.Instance.ShowEndUITransition();
    }

    private void CheckRunVFX()
    {
        if (Player.MyState != PlayerController.PlayerState.Walk) _walkVFX.SendEvent("StopRun");
        else _walkVFX.SendEvent("StartRun");
    }
}
