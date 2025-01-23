using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLine : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GameEvent bulletHit;

    private void Update()
    {
        var origin = _lineRenderer.GetPosition(1);
        var direction = _lineRenderer.GetPosition(0);
        var hits = Physics2D.LinecastAll(origin, direction);
        foreach(var hit in hits)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                bulletHit.Raise(hit.transform, _bullet);
                break;
            }
        }
    }
}
