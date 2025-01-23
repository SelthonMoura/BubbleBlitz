using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float castRadius;
    [SerializeField] private GameEventListener<CustomEvent<object>> removeBullet;
    [SerializeField] private GameEvent bulletHit;
    private int _pierced;
    private WeaponStats _weaponStats;
    private Transform _lastObjectHit;
    private bool _collided;

    private void Start()
    {
        removeBullet.AddListener<object>(DestroyBullet);
    }

    private void OnDestroy()
    {
        removeBullet.RemoveListener<object>(DestroyBullet);
    }

    private void OnBecameInvisible()
    {
        DestroyBullet(this);
    }

    private void DestroyBullet(object bullet)
    {
        if (bullet != (object)this) return;
        _pierced--;
        if (_pierced < 0)
        {
            _lastObjectHit = null;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
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
        if (hit.transform.CompareTag("Ground"))
        {
            _lastObjectHit = null;
            gameObject.SetActive(false);
        }
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
        _pierced = weaponStats.Pierce;
    }

}
