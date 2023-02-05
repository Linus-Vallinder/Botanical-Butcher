using System.Collections;
using UnityEngine;
using Skills;

public class EncounterManager : Singleton<EncounterManager>
{
    public Enemy CurrentEnemyType { get; private set; }

    int m_currentHealth;
    public bool IsAttacking { get; private set; } = false;

    TextBox m_console;

    #region Unity Methods

    private void Awake()
    {
        m_console = FindObjectOfType<TextBox>();
    }

    #endregion

    public void InstantiateEnemy(Enemy enemy)
    {
        CurrentEnemyType = enemy;
        m_currentHealth = enemy.maxHealth;
        Attack();
    }

    public IEnumerator Attack()
    {
        if (IsAttacking) yield return null;
        IsAttacking = true;
        yield return new WaitForSeconds(3.5f);
        m_console.AddLine($"{CurrentEnemyType.name} has attacked!");
        Hero.Instance.SetHerosTurn();
        IsAttacking = false;
    }

    public void ReciveAttack(Skill skill)
    {
        m_console.AddLine($"- [{skill.GetRandomUsagePrompt()}] - ");
        m_currentHealth -= 100; //This will be influenced by the skills

        if (m_currentHealth <= 0)
        {
            Hero.Instance.EndCombat();
            Hero.Instance.AddXP(Mathf.RoundToInt(Random.Range(CurrentEnemyType.XPDropRange.x, CurrentEnemyType.XPDropRange.y)));
        }
    }
}
