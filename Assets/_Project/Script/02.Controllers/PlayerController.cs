using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [Header("Data")]
    public PlayerDataSO playerData;
    private float _currentHP;
    [Header("Component")]
    private Rigidbody _rb;
    private SpriteRenderer _sr;
    private Animator _anim;
    private PlayerInput _playerInput;
    private Vector3 _moveDir;

    private bool _isDashing = false;
    private bool _isAttacking = false;
    private bool _isDead = false;
    private bool _canDash = true;
    private bool IsMoving;
    
    public  bool IsDashing => _isDashing;
    public bool IsAttacking => _isAttacking;
    public  bool IsDead => _isDead;

    private void Awake()
    {
        Instance = this;
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        _currentHP = playerData.maxHP;
    }
    private void Update()
    {
        if (_isDead) return;

        Vector2 inputVec = _playerInput.actions["Move"].ReadValue<Vector2>();

        if (!_isAttacking && !_isDashing)
        {
            _moveDir = new Vector3(inputVec.x, 0, inputVec.y);
        }
        if (_playerInput.actions["Dash"].WasPerformedThisFrame())
        {
            TryDash();
        }
        if (_playerInput.actions["Attack"].WasPerformedThisFrame())
        {
            TryAttack();
        }

        UpdateVisuals();
        if (Keyboard.current.kKey.wasPressedThisFrame) OnDie();
    }
    private void FixedUpdate()
    {
        if (_isDead) return;
        Move();
    }
    private void Move()
    {
        if (_isDead) return;
        float currentSpeed = playerData.moveSpeed;
        if (_isDashing)
        {
            currentSpeed = playerData.dashSpeed;
        }
        else if (!_isDashing)
        {
            currentSpeed = playerData.moveSpeed;
        }
        _rb.MovePosition(_rb.position + _moveDir * currentSpeed * Time.fixedDeltaTime);
    }
    private void TryDash()
    {
        if (_isDashing || _isAttacking || !_canDash || _isDead) return;
        StartCoroutine(CoDash());
    }
    private IEnumerator CoDash()
    {
        _isDashing = true;
        _canDash = false;
        _anim.SetTrigger("Dash");
        yield return new WaitForSeconds(playerData.dashDuration);
        _isDashing = false;
        yield return new WaitForSeconds(playerData.dashCooldown);
        _canDash = true;
        yield return null;
    }
    private void TryAttack()
    {
        if (_isAttacking || _isDashing) return;
        StartCoroutine(CoAttack());
    }
    private IEnumerator CoAttack()
    {
        _isAttacking = true;
        _moveDir = Vector3.zero;
        _anim.SetTrigger("Attack");
        yield return new WaitForSeconds(playerData.attackCooldown);
        _isAttacking = false;
        yield return null; 
    }
    public void TakeDamage(float daamge)
    {
        if (_isDead || _isDashing) return;
        _currentHP -= daamge; ;
        Debug.Log($"남은 체력 : {_currentHP}");
        if(_currentHP <= 0)
        {
            OnDie();
        }
    }
    public void OnDie()
    {
        if (_isDead) return;
        _isDead = true;
        _anim.SetTrigger("Dead");
        Debug.Log("Player Dead!");
    }
    private void UpdateVisuals()
    {
        if (_isDead) return;
        IsMoving = _moveDir.magnitude > 0.01f;
        if(_anim != null)
        {
            _anim.SetBool("IsRun", IsMoving);
        }
        if (_moveDir.x > 0) _sr.flipX = false;
        else if (_moveDir.x < 0) _sr.flipX = true;
    }
}
