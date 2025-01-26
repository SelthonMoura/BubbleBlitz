using UnityEngine;

[CreateAssetMenu(menuName = "ItemSO")]
public class ItemSO: ScriptableObject
{
    public string itemName;
    public Sprite icon;

    // Stat modifications
    public int extraLives;
    public float speedModifier;
    public bool canJump;
    public int bombs;
    public bool hasShield;

    public void ApplyEffect(PlayerStats playerStats)
    {
        playerStats.currentHp += extraLives;
        playerStats.speed += speedModifier;
        playerStats.canJump = canJump || playerStats.canJump; // Keeps jump ability if true
        playerStats.bombs += bombs;
        playerStats.hasShield = hasShield || playerStats.hasShield; // Keeps shield if true
    }
}
