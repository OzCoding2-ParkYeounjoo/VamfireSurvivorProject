using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    [Header("Data")]
    public PlayerDataSO playerData;
    [HideInInspector] public float currentMaxHP;
    [HideInInspector] public float currentMoveSpeed;
    [HideInInspector] public float currentDamage;
    [HideInInspector] public float currentAttackCoolDown;
    private float _currentHP;
    public float CurrentHP => _currentHP;
    [Header("Cobat")]
    public GameObject weaponHitbox;
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
    private bool _isHit = false;
    private bool _isInvincible = false;
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
        InitializeStats();
    }
    private void Start()
    {
        if(weaponHitbox != null) weaponHitbox.SetActive(false);
    }
    void InitializeStats()
    {
        if (playerData == null)
        {
            Debug.LogError("PlayerDataSO가 연결 되어 있지 않습니다.");
            return;
        }
        currentMaxHP = playerData.maxHP;
        currentMoveSpeed = playerData.moveSpeed;
        currentDamage = playerData.damage;
        currentAttackCoolDown = playerData.attackCooldown;

        _currentHP = currentMaxHP;
    }
    private void Update()
    {
        if (_isDead) return;
        if (_isHit)
        {
            _moveDir = Vector3.zero;
        }

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
        if (_isDead || _isHit) return;
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
        if(weaponHitbox != null) weaponHitbox.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (weaponHitbox != null) weaponHitbox.SetActive(false);
        yield return new WaitForSeconds(playerData.attackCooldown);
        _isAttacking = false;
        yield return null; 
    }
    public void TakeDamage(float damage)
    {
        if (_isDead || _isDashing) return;
        _currentHP -= damage; ;
        Debug.Log($"남은 체력 : {_currentHP}");
        if(_currentHP <= 0)
        {
            OnDie();
        }
        else
        {
            StartCoroutine(CoHIt());
        }
    }
    private IEnumerator CoHIt()
    {
        _isHit = true;
        _isInvincible = true;
        _anim.SetTrigger("Hit");
        _sr.color = Color.red;

        _isAttacking = false;
        _isDashing = false;
        yield return new WaitForSeconds(0.4f);
        _isHit = false;
        _sr.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(0.6f);
        _sr.color = Color.white;
        _isInvincible = false;
    }
    public void OnDie()
    {
        if (_isDead) return;
        _isDead = true;
        StopAllCoroutines();
        _anim.SetTrigger("Dead");
        _sr.color = Color.white;
        _rb.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null) spawner.enabled = false;
        var enimies = FindObjectsOfType<EnemyController>();
        foreach (var e in enimies) e.enabled = false;
        Debug.Log("Player Dead!");
        StartCoroutine(CoDead());
    }
    private IEnumerator CoDead()
    {
        yield return new WaitForSeconds(0.4f);
        GameManager.Instance.OnGameOver();
        gameObject.SetActive(false);
    }
    private void UpdateVisuals()
    {
        if (_isDead) return;
        IsMoving = _moveDir.magnitude > 0.01f;
        if(_anim != null)
        {
            _anim.SetBool("IsRun", IsMoving);
        }
        if (_moveDir.x > 0)
        {
            _sr.flipX = false;
            if (weaponHitbox != null)
            { weaponHitbox.transform.localRotation = Quaternion.Euler(0, 0, 0); }
        }
        else if (_moveDir.x < 0)
        {
            _sr.flipX = true;
            if (weaponHitbox != null)
            {weaponHitbox.transform.localRotation = Quaternion.Euler(0, 180, 0); }
        }
    }
    public void FullRecovery()
    {
        _currentHP = currentMaxHP;
        Debug.Log("플레이어 체력 완전 회복");
    }
}
