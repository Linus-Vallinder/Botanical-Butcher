using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyAttack
{
    [Header("Attack Options")]
    public string Name;
    
    [Space]
    public string UsagePrompt;
    
    [Space]
    public int Power;
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemies/New Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Enemy Options")]
    public string name;
    [Space]
    public int maxHealth;
    public int attackPower;

    [Space]
    public Vector2 XPDropRange = new();

    [Space]
    public List<Item> DropTable = new();

    [Space]
    public List<EnemyAttack> Attacks = new();
}