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
    public int BasePower;

    public EnemyAttack(string name, string usagePrompt, int basePower)
    {
        Name = name;
        UsagePrompt = usagePrompt;
        BasePower = basePower;
    }
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemies/New Enemy")]
public class Enemy : ScriptableObject
{
    [Header("Enemy Options")]
    public int maxHealth;
    //public int attackPower;
    //public int roots;

    [Space]
    public Vector2 XPDropRange = new();

    [Space]
    public List<EnemyStat> Stats = new();

    [Space]
    public DropTable<Item> DropTable = new();

    [Space]
    public List<EnemyAttack> Attacks = new();

    public Item GetRandomDrop()
    => DropTable.GetDrop();

    public EnemyAttack GetRandomAttack()
    {
        if (Attacks.Count == 0) return new EnemyAttack("Punch", $"The {this.name} used punch!", 50);
        else return Attacks[Random.Range(0, Attacks.Count)];
    }

    public float GetAttributeModifier(EnemyAttribute attribute)
        {
            var total = 0f;
            Stats.ForEach( stat => {
                if(stat.attribute == attribute) total += stat.Modifier;
            });
            return total;
        }
}