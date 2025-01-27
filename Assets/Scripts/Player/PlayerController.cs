using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private ItemList _itemList;
    [SerializeField] private GameEvent _playerTakeDamageTrigger;
    [SerializeField] private GameEvent _useBombEvent;
    [SerializeField] private GameEvent _updatePlayerUI;
    [SerializeField] private GameEventListener<CustomEvent<int>> _buyItemEvent;
    [SerializeField] private GameEventListener<CustomEvent<int>> _playerScoredEvent;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private GameObject _shieldEffect;

    private bool _climbing = false;
    private bool _touchingLadder = false;
    private bool _isGrounded = false;
    private float _ladderClimb = 0;
    private Collider2D _closestLadder;
    private GameEventListener<CustomEvent> _updatePlayerUIListener;

    private void Start()
    {
        _updatePlayerUI.Raise();
    }

    private void Awake()
    {
        _playerStats.speed = _playerStats.baseSpeed;
        _playerStats.bombs = 0;
        _playerStats.canJump = false;
        _playerStats.hasShield = false;
        _buyItemEvent.AddListener<int>(BuyItem);
        _playerScoredEvent.AddListener<int>(PlayerScored);
        _updatePlayerUIListener = new GameEventListener<CustomEvent>
        {
            @event = _updatePlayerUI
        };
        _updatePlayerUIListener.AddListener(OnPlayerStatChange);
    }

    private void OnPlayerStatChange()
    {
        _shieldEffect.SetActive(_playerStats.hasShield);
    }

    private void OnDestroy()
    {
        _buyItemEvent.RemoveListener<int>(BuyItem);
        _playerScoredEvent.RemoveListener<int>(PlayerScored);
        _updatePlayerUIListener.RemoveListener(OnPlayerStatChange);
    }

    private void BuyItem(int itemIndex)
    {
        ItemSO item = _itemList.items[itemIndex];
        item.ApplyEffect(_playerStats);
        _updatePlayerUI.Raise();
    }

    private void Update()
    {
        var inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        CheckGrounded();

        if (Input.GetButtonDown("Jump") && _isGrounded && _playerStats.canJump)
        {
            Jump();
        }

        if (Input.GetButtonDown("Bomb") && _playerStats.bombs>0)
        {
            _useBombEvent.Raise();
            _playerStats.bombs--;
        }

        if (_touchingLadder)
        {
            _ladderClimb = Mathf.Clamp01(_ladderClimb + inputVector.y * Time.deltaTime);
            _climbing = _ladderClimb > 0 && _ladderClimb < 1;
        }

        if (!_climbing)
        {
            if (inputVector.x < 0)
                _spriteRenderer.flipX = true;
            else if (inputVector.x > 0)
                _spriteRenderer.flipX = false;

            var hits = Physics2D.BoxCastAll(transform.position, new Vector2(0.6f, 0.6f), 0, inputVector, 0f);
            _animator.SetBool("Walking", inputVector.magnitude > 0&&_isGrounded);
            _animator.SetBool("Climbing", false);
            if (!Array.Exists(hits, o => o.transform.CompareTag("Ground")))
                _rb.velocity = new Vector2(inputVector.x * _playerStats.speed, _rb.velocity.y);
        }
        else
        {
            if (_touchingLadder)
            {
                _animator.SetBool("Climbing", true);
                transform.position = new Vector3(_closestLadder.bounds.center.x, _closestLadder.bounds.min.y + _collider.bounds.size.y / 2 + _ladderClimb * _closestLadder.bounds.size.y);
            }
        }
    }

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
    }

    private void CheckGrounded()
    {
        // Use a small boxcast or overlap check to detect if the player is on the ground
        var bounds = _collider.bounds;
        Vector2 position = new Vector2(bounds.center.x, bounds.min.y);
        Vector2 size = new Vector2(bounds.size.x * 0.9f, 0.1f);

        _isGrounded = Physics2D.OverlapBox(position, size, 0f, _groundLayer) != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            _touchingLadder = true;
            _closestLadder = collision;
        }
        else if(collision.CompareTag("Enemy"))
        {
            //EventManager.OnUpdatePlayerBarsTrigger();
            _playerTakeDamageTrigger.Raise();
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            if (!_climbing)
                if (transform.position.y > _closestLadder.bounds.center.y)
                    _ladderClimb = 1;
                else
                    _ladderClimb = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            _touchingLadder = false;
        }
    }

    private void PlayerScored(int points)
    {
        _playerStats.score += points;
        _updatePlayerUI.Raise();
    }
}
