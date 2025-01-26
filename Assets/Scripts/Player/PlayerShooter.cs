using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private WeaponStats _weaponStats;
    [SerializeField] private ItemList _itemList;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private GameEventListener<CustomEvent<int>> _weaponChangeEvent;
    private List<Bullet> _bullets = new List<Bullet>();
    private float _cooldown;

    private void Awake()
    {
        _weaponChangeEvent.AddListener<int>(ChangeWeapon);
    }

    private void OnDestroy()
    {
        _weaponChangeEvent.RemoveListener<int>(ChangeWeapon);
    }

    private void Update()
    {
        if(_weaponStats.aimed)
            AimAtMouse();
        if (_cooldown > 0)
            _cooldown -= Time.deltaTime;
        else if (Input.GetMouseButton(0))
        {
            var activeBullets = _bullets.FindAll(o => o.gameObject.activeSelf);
            if (_weaponStats.persistLine)
            {
                var wiredBullets = activeBullets.FindAll(o => o.GetWired());
                if(wiredBullets.Count > 0)
                    wiredBullets[0].gameObject.SetActive(false);
            }
            if (activeBullets.Count < _weaponStats.bulletLimit*_weaponStats.bulletAmount)
                Shoot();
        }
    }

    private void ChangeWeapon(int i)
    {
        _weaponStats = _itemList.weapons[i];
        if(!_weaponStats.aimed)
            transform.rotation = Quaternion.Euler(0, 0, 0);
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
        _playerAnimator.SetTrigger("Shoot");
        AudioSystem.Instance.PlaySFX(_weaponStats.shotSFX);
        _cooldown = 1f / _weaponStats.fireRate;
        for (var i = 0; i < _weaponStats.bulletAmount; i++)
        {
            var arrayAngle = 15f;
            var startingAngle = _weaponStats.bulletAmount <= 1 ? 0 : -arrayAngle * (_weaponStats.bulletAmount) / 2 + arrayAngle / 2;
            var rotationShift = startingAngle + i * arrayAngle;
            var rotation = transform.rotation * Quaternion.AngleAxis(rotationShift, Vector3.forward);
            var bullet = (Bullet)PoolManager.Instance.ReuseComponent(_weaponStats.bulletPrefab, transform.position, rotation);
            bullet.LineTarget = transform;
            if(!_bullets.Contains(bullet))
                _bullets.Add(bullet);
            bullet.gameObject.SetActive(true);
            bullet.SetDirection(bullet.transform.up * _weaponStats.shotSpeed);
            bullet.SetWeaponStats(_weaponStats);
        }
    }
}
