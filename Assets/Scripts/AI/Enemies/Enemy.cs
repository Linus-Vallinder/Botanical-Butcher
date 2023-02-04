using System.Collections.Generic;
using UnityEngine;

public enum EnemyAttribute
{
    Thorny,
    Poisonous,
    Lust,
    Size
}

[System.Serializable]
public class EnemyStat
{
    [Header("Stat Options")]
    public string name;

    [Space] public EnemyAttribute attribute;

    public EnemyStat(string name, EnemyAttribute attribute, float modifier)
    {
        this.name = name;
        this.attribute = attribute;
        Modifier = modifier;
    }

    [Space, Range(-100f, 100f)]
    public float Modifier;
}

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
    public List<EnemyStat> Stats = new();

    [Space]
    public List<Item> DropTable = new();

    [Space]
    public List<EnemyAttack> Attacks = new();
}