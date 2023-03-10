using Skills;
using System.Collections;
using UnityEngine;

public class EncounterManager : Singleton<EncounterManager>
{
    public Enemy CurrentEnemyType { get; set; }

    private int m_currentHealth;
    public bool IsAttacking { get; set; } = false;

    private TextBox m_console;

    #region Unity Methods

    private void Awake()
    {
        m_console = FindObjectOfType<TextBox>();
    }

    #endregion Unity Methods

    public void InstantiateEnemy(Enemy enemy)
    {
        CurrentEnemyType = enemy;
        m_currentHealth = enemy.maxHealth;
    }

    public IEnumerator Attack()
    {
        if (Hero.Instance.m_currentState != HeroState.Encounter)
        {
            CurrentEnemyType = null;
            IsAttacking = false;
            yield return null;
        }

        if (IsAttacking) yield return null;
        IsAttacking = true;

        if (m_currentHealth <= 0)
        {
            MapAnimation.Instance.KillEnemy();
            m_console.AddLine($"The hero has defeated the {CurrentEnemyType.name} Monster!");
            yield return new WaitForSeconds(3.5f);

            Hero.Instance.AddXP(Mathf.RoundToInt(Random.Range(CurrentEnemyType.XPDropRange.x, CurrentEnemyType.XPDropRange.y)));

            var drop = CurrentEnemyType.GetRandomDrop();
            Inventory.Instance.Add(drop);
            m_console.AddLine($"{CurrentEnemyType.name} has dropped a {drop.name}!");
            yield return new WaitForSeconds(2.5f); 
            Hero.Instance.EndCombat();
            IsAttacking = false;

            IsAttacking = false;
            yield return null;
        }

        if (IsAttacking)
        {
            MapAnimation.Instance.AnimateEnemy();

            m_console.AddLine($"{CurrentEnemyType.name} is attacking!");

            yield return new WaitForSeconds(3.5f);

            var attack = CurrentEnemyType.GetRandomAttack();
            //Do Damage
            int damageDealt = Hero.Instance.ReciveAttack(attack, CurrentEnemyType);
            m_console.AddLine($"{attack.UsagePrompt} {damageDealt} damage recivied!");

            yield return new WaitForSeconds(3.5f);

            Hero.Instance.SetHerosTurn();
            IsAttacking = false;
        }
    }

    public void ReciveAttack(Skill skill)
    {
        var weightedAdvantage = 0f;
        skill.Stats.ForEach(stat =>  {
            switch (stat.attribute)
            {
                case Attribute.Constitution:
                    weightedAdvantage += stat.Modifier;
                    weightedAdvantage -= CurrentEnemyType.GetAttributeModifier(EnemyAttribute.Thorny);
                    weightedAdvantage -= CurrentEnemyType.GetAttributeModifier(EnemyAttribute.Poisonous);
                    break;
                case Attribute.Wisdom:
                    weightedAdvantage += stat.Modifier;
                    weightedAdvantage -= CurrentEnemyType.GetAttributeModifier(EnemyAttribute.Lust);
                    break;
                case Attribute.Strength:
                    weightedAdvantage += stat.Modifier;
                    weightedAdvantage -= CurrentEnemyType.GetAttributeModifier(EnemyAttribute.Size);
                    break;
                case Attribute.Luck:
                    weightedAdvantage += stat.Modifier;
                    break;
                default:
                    break;
            }
        });
        
        int attackValue = 20 + (int)weightedAdvantage;

        m_console.AddLine($"{skill.GetRandomUsagePrompt()} {attackValue} damage dealt!");
        m_currentHealth -= attackValue;
    }
}