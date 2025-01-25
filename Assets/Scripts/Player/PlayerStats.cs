using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    public int maxHp;
    public float baseSpeed;

    public int currentHp;
    public float speed;
    public int extraLives;
    public int bombs;
    public bool canJump;
    public bool hasShield;
}
