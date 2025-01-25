using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private float _bounceHeight = 5f; // Desired maximum height
    [SerializeField] private float _gravity = -9.81f; // Simulated gravity
    [SerializeField] private Vector2 _initialVelocity = new Vector2(2f, 5f); // Initial velocity (x and y)
    [SerializeField] private GameEventListener<CustomEvent<object, Bullet>> bulletHit;
    [SerializeField] private GameEventListener<CustomEvent> bombUsed;
    [SerializeField] private GameEvent _removeBullet;
    [SerializeField] private GameEvent _bubbleSpawn;
    [SerializeField] private GameEvent _bubbleDeath;
    private bool _poppable;
    private bool _useGravity;
    private bool _popping;
    private Vector2 _velocity; // Current velocity

    private void Awake()
    {
        // Set the initial velocity
        _velocity = _initialVelocity;
        bulletHit.AddListener<object, Bullet>(PopBubble);
        bombUsed.AddListener(() => { StartCoroutine(PopBubbleSequence()); });
    }

    private IEnumerator InitialInvincibility()
    {
        _poppable = false;
        yield return new WaitForSeconds(0.2f);
        _poppable = true;
        _bubbleSpawn.Raise();
    }

    private void OnEnable()
    {
        _useGravity = true;
        StartCoroutine(InitialInvincibility());
    }

    private void OnDestroy()
    {
        bulletHit.RemoveListener<object, Bullet>(PopBubble);
        bombUsed.RemoveListener(() => { StartCoroutine(PopBubbleSequence()); });
    }

    private void OnBecameInvisible()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -13, 13), Mathf.Clamp(transform.position.y, -6, 6));
    }

    private void PopBubble(object hit, Bullet bullet)
    {
        if ((object)transform != hit) return;
        if (!_poppable) return;
        StartCoroutine(PopBubbleSequence(bullet));
    }

    private IEnumerator PopBubbleSequence(Bullet bullet = null)
    {
        _poppable = false;
        _popping = true;
        _velocity = Vector2.zero;
        _useGravity = false;
        _animator.SetTrigger("Pop");
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
                    childBubble.transform.Translate(child.initialVelocity/10f);
                    childBubble.SetVelocity(child.initialVelocity);
                }
                // Pass remaining damage to child bubbles
                childBubble.PopRecursively(damage - 1);
            }
        }
        if(_popping) return;
        // Destroy this bubble
        if (!_poppable)
            _bubbleSpawn.Raise();
        _bubbleDeath.Raise();
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        // Apply custom gravity
        if(_useGravity)
            _velocity += new Vector2(0, _gravity * Time.fixedDeltaTime);

        // Update the position based on velocity
        transform.position += (Vector3)(_velocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            // Get the contact point normal
            Vector2 collisionNormal = (collision.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;

            if (Mathf.Abs(collisionNormal.y) > Mathf.Abs(collisionNormal.x))
            {
                // Vertical collision (top or bottom of the ground)
                float bounceVelocity = Mathf.Sqrt(-2 * _gravity * _bounceHeight);
                _velocity = new Vector2(_velocity.x, -_velocity.normalized.y*bounceVelocity);
            }
            else
            {
                // Horizontal collision (side of the ground)
                _velocity = new Vector2(-_velocity.x, _velocity.y);
            }
        }
    }

    private void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }
}
