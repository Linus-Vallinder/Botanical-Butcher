using System.Collections;
using UnityEngine;
using Skills;

public class EncounterManager : Singleton<EncounterManager>
{
    public Enemy CurrentEnemyType { get; private set; }

    int m_currentHealth;
    bool isAttacking = false;

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
        if (isAttacking) yield return null;
        isAttacking = true;
        yield return new WaitForSeconds(2f);
        m_console.AddLine($"{CurrentEnemyType.name} has attacked!");
        Hero.Instance.SetHerosTurn();
        isAttacking = false;
    }

    public void ReciveAttack(Skill skill)
    {

    }
}
