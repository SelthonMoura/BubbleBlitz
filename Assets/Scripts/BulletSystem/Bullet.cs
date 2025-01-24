using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float castRadius;
    [SerializeField] private float groundRadius = 0.1f;
    [SerializeField] private GameEventListener<CustomEvent<object>> removeBullet;
    [SerializeField] private GameEvent bulletHit;
    private Transform lineTarget;
    private int _pierced;
    private WeaponStats _weaponStats;
    private Transform _lastObjectHit;
    private bool _collided;
    private bool _wired;

    public Transform LineTarget { get => lineTarget; set => lineTarget = value; }

    private void Awake()
    {
        LineTarget = transform;
        removeBullet.AddListener<object>(DestroyBullet);
    }

    private void OnDestroy()
    {
        removeBullet.RemoveListener<object>(DestroyBullet);
    }

    private void OnBecameInvisible()
    {
        _lastObjectHit = null;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _wired = false;
        _lastObjectHit = null;
    }

    private void DestroyBullet(object bullet)
    {
        if (bullet != (object)this) return;
        _pierced--;
        if (_pierced < 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_wired) return;
        var hits = Physics2D.CircleCastAll(transform.position, groundRadius, Vector2.zero, float.PositiveInfinity);
        foreach (var groundHit in hits)
            if (groundHit.transform.CompareTag("Ground"))
            {
                if(_weaponStats.persistLine)
                {
                    body.velocity = Vector2.zero;
                    _wired = true;
                }
                else
                {
                    gameObject.SetActive(false);
                }
                return;
            }
        var hit = Physics2D.CircleCast(transform.position, castRadius, Vector2.zero, float.PositiveInfinity);
        if (!hit)
        {
            _collided = false;
            return;
        }
        if (_collided && hit.transform.Equals(_lastObjectHit)) return;
        _collided = true;
        _lastObjectHit = hit.transform;
        bulletHit.Raise(hit.transform, this);
    }

    public void SetDirection(Vector2 direction)
    {
        body.velocity = direction;
    }

    public WeaponStats GetWeaponStats()
    {
        return _weaponStats;
    }

    public void SetWeaponStats(WeaponStats weaponStats)
    {
        _weaponStats = weaponStats;
        _pierced = weaponStats.pierce;
    }

    public bool GetWired()
    {
        return _wired;
    }
}
