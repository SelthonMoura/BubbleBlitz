using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[System.Serializable]
public struct ChildBubble
{
    public GameObject prefab;
    public Vector2 initialVelocity;
}

public class Bubble : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ChildBubble[] _childBubbles; // Prefabs for child bubbles
    [SerializeField] private float _gravity = -9.81f; // Simulated gravity
    [SerializeField] private float _bounceHeight = 5f; // Desired maximum height
    [SerializeField] private Vector2 _initialVelocity = new Vector2(2f, 5f); // Initial velocity (x and y)
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private GameEventListener<CustomEvent<object, Bullet>> bulletHit;
    [SerializeField] private GameEventListener<CustomEvent> bombUsed;
    [SerializeField] private GameEvent _removeBullet;
    [SerializeField] private GameEvent _bubbleDeath;
    [SerializeField] private GameEvent _playerScored;
    [SerializeField] private int points;
    private bool _spawning;
    private bool _poppable;
    private bool _useGravity;
    private bool _popping;
    private Vector2 _velocity; // Current velocity

    private void Awake()
    {
        // Set the initial velocity
        _velocity = _initialVelocity;
        bulletHit.AddListener<object, Bullet>(PopBubble);
        bombUsed.AddListener(() => { if (gameObject.activeSelf)  StartCoroutine(PopBubbleSequence()); });
    }

    private IEnumerator InitialInvincibility()
    {
        _poppable = false;
        yield return new WaitForSeconds(0.2f);
        _poppable = true;
    }

    private void OnEnable()
    {
        _useGravity = true;
        transform.tag = "Enemy";
        StartCoroutine(InitialInvincibility());
    }

    private void OnDestroy()
    {
        bulletHit.RemoveListener<object, Bullet>(PopBubble);
        bombUsed.RemoveListener(() => { if (gameObject.activeSelf) StartCoroutine(PopBubbleSequence()); });
    }

    private void OnBecameInvisible()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -13, 13), Mathf.Clamp(transform.position.y, -6, 6));
    }

    private void PopBubble(object hit, Bullet bullet)
    {
        if ((object)transform != hit) return;
        if (!_poppable) return;
        if (_spawning) return;
        StartCoroutine(PopBubbleSequence(bullet));
    }

    private IEnumerator PopBubbleSequence(Bullet bullet = null)
    {
        _poppable = false;
        _popping = true;
        transform.tag = "Untagged";
        _velocity = Vector2.zero;
        _useGravity = false;
        _animator.SetTrigger("Pop");
        AudioSystem.Instance.PlaySFX("BubblePop");
        // Recursive popping based on bullet damage
        if (bullet != null)
        {
            PopRecursively(bullet.GetWeaponStats().damage);
            _removeBullet.Raise(bullet);
        }
        else
        {
            PopRecursively(1);
        }
        yield return new WaitForSeconds(0.5f);
        // Destroy this bubble
        _bubbleDeath.Raise();
        _playerScored.Raise(points);
        gameObject.SetActive(false);
    }

    private void PopRecursively(int damage)
    {
        if (damage <= 0) return;

        // If there are child bubbles, spawn them
        if (_childBubbles.Length > 0)
        {
            foreach (ChildBubble child in _childBubbles)
            {
                Bubble childBubble = (Bubble)PoolManager.Instance.ReuseComponent(child.prefab, transform.position, Quaternion.identity);
                if (child.initialVelocity != Vector2.zero)
                {
                    childBubble.transform.Translate(child.initialVelocity.normalized/2f);
                    childBubble.SetVelocity(child.initialVelocity);
                }
                // Pass remaining damage to child bubbles
                childBubble.PopRecursively(damage - 1);
            }
        }
        if(_popping) return;
        // Destroy this bubble
        _bubbleDeath.Raise();
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_spawning)
        {
            transform.position += 1.5f * Time.fixedDeltaTime * Vector3.down;
            if (transform.position.y < 6f) {
                _spawning = false;
                if(transform.position.x<0)
                    _velocity = _initialVelocity;
                else
                    _velocity = new Vector2(-_initialVelocity.x, _initialVelocity.y);
            }
            return;
        }
        // Apply custom gravity
        if(_useGravity)
            _velocity += new Vector2(0, _gravity * Time.fixedDeltaTime * _speedMultiplier);

        // Update the position based on velocity
        transform.position += (Vector3)(_velocity * Time.fixedDeltaTime * _speedMultiplier);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_spawning) return;
        if (collision.CompareTag("Ground"))
        {
            // Get the contact point normal
            Vector2 collisionNormal = (collision.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
            AudioSystem.Instance.PlaySFX("BubbleBounce");
            if (Mathf.Abs(collisionNormal.y) > Mathf.Abs(collisionNormal.x))
            {
                // Vertical collision (top or bottom of the ground)
                if (_gravity == 0)
                    _velocity = new Vector2(_velocity.x, -_velocity.y);
                else
                {
                    float bounceVelocity = Mathf.Sqrt(-2 * _gravity * _bounceHeight);
                    _velocity = new Vector2(_velocity.x, -_velocity.normalized.y * bounceVelocity);
                }
            }
            else
            {
                // Horizontal collision (side of the ground)
                _velocity = new Vector2(-_velocity.x, _velocity.y);
            }
        }
    }

    public void SetSpawning(bool spawning)
    {
        _spawning = spawning;
    }

    private void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }
}
