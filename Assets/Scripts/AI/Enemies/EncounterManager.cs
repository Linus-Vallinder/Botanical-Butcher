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
        yield return new WaitForSeconds(2f);
        m_console.AddLine($"{CurrentEnemyType.name} has attacked!");
        Hero.Instance.SetHerosTurn();
        IsAttacking = false;
    }

    public void ReciveAttack(Skill skill)
    {

    }
}