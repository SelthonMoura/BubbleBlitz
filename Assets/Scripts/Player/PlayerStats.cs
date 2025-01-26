using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    public int score;
    public int maxHp;
    public float baseSpeed;

    public int currentHp;
    public float speed;
    public int extraLives;
    public int bombs;
    public bool canJump;
    public bool hasShield;

    public void ResetStats()
    {
        currentHp = maxHp;
        speed = baseSpeed;
        canJump = false;
        hasShield = false;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
