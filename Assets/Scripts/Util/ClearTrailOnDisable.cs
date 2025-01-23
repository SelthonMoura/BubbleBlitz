using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrailOnDisable : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trail;

    private void OnDisable()
    {
        _trail.Clear();
    }
}
