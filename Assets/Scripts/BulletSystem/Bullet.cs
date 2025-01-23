using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private float castRadius;
    [SerializeField] private GameEventListener<CustomEvent<object>> removeBullet;
    [SerializeField] private GameEvent bulletHit;
    [SerializeField] private LineRenderer lineRenderer;
    private Transform lineTarget;
    private int _pierced;
    private WeaponStats _weaponStats;
    private Transform _lastObjectHit;
    private bool _collided;

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

    private void OnEnable()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, LineTarget.position);
    }

    private void OnBecameInvisible()
    {
        _lastObjectHit = null;
        gameObject.SetActive(false);
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
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, LineTarget.position);
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
