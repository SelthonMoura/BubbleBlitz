using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private int _lives;
    [SerializeField] private float _speed;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameEvent _playerTakeDamageTrigger;

    private bool _climbing = false;
    private bool _touchingLadder = false;
    private float _ladderClimb = 0;
    private Collider2D _closestLadder;

    private void Update()
    {
        var inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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
            if(Physics2D.Raycast(transform.position, inputVector, 0.8f, 7))
                _rb.velocity = new Vector2(inputVector.x * _speed, _rb.velocity.y);
        }
        else
        {
            if(_touchingLadder)
                transform.position = new Vector3(_closestLadder.bounds.center.x, _closestLadder.bounds.min.y + _collider.bounds.size.y/2 + _ladderClimb*_closestLadder.bounds.size.y);
        }
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
}
