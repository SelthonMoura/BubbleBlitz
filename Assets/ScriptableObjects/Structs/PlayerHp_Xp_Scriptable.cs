using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Hp & Xp Values", menuName = "ScriptableObjets/PlayerHp&XpValues")]
public class PlayerHp_Xp_Scriptable : ScriptableObject
{
    public int maxHp;
    public int currentHp;
}
