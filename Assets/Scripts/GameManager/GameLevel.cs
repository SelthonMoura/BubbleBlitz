using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BubbleType { Bubble, Bubble2, Bubble4, Bubble8, GravBubble1, GravBubble2, GravBubble4, GravBubble8}
[System.Serializable]
public struct Spawn
{
    public BubbleType bubble;
    [Range(-1f, 1f)] public float positionX;
}
[System.Serializable]
public struct Wave
{
    public List<Spawn> spawns;
    public float secondsToNext;
}

[System.Serializable]
public class GameLevel
{
    public GameObject level;
    public Transform playerPosition;
    public List<Wave> waves;
    public int popsRequired;
}
