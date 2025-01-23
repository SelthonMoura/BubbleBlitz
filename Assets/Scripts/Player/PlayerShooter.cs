using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private WeaponStats _weaponStats;
    private List<Bullet> _bullets = new List<Bullet>();
    private float _cooldown;

    private void Update()
    {
        AimAtMouse();
        if (_cooldown > 0)
            _cooldown -= Time.deltaTime;
        else if (Input.GetMouseButton(0))
        {
            var activeBullets = _bullets.FindAll(o => o.gameObject.activeSelf);
            if (activeBullets.Count < _weaponStats.BulletLimit*_weaponStats.BulletAmount)
                Shoot();
        }
    }

    private void AimAtMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    private void Shoot()
    {
        if (_cooldown > 0) return;
        _cooldown = 1f / _weaponStats.FireRate;
        for (var i = 0; i < _weaponStats.BulletAmount; i++)
        {
            var arrayAngle = 15f;
            var startingAngle = _weaponStats.BulletAmount <= 1 ? 0 : -arrayAngle * (_weaponStats.BulletAmount) / 2 + arrayAngle / 2;
            var rotationShift = startingAngle + i * arrayAngle;
            var rotation = transform.rotation * Quaternion.AngleAxis(rotationShift, Vector3.forward);
            var bullet = (Bullet)PoolManager.Instance.ReuseComponent(_bulletPrefab, transform.position, rotation);
            if(!_bullets.Contains(bullet))
                _bullets.Add(bullet);
            bullet.gameObject.SetActive(true);
            bullet.SetDirection(bullet.transform.up * _weaponStats.ShotSpeed);
            bullet.SetWeaponStats(_weaponStats);
        }
    }
}
