using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    public int maxHp;
    public int currentHp;
    public int extraLives;
    public float speed;
    public bool canJump;
    public int bombs;
    public bool hasShield;
}
