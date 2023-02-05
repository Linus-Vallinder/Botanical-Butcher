using Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum HeroState
{
    Idle,
    Travel,
    Wander,
    Heal,
    Pockets,
    Sell,
    Encounter,
    Dead,
    None
}

public class Hero : Singleton<Hero>
{
    [SerializeField] private List<Location> StartingLocations = new();

    public Location CurrentLocation { get; private set; }

    public int Gold { get; private set; } = 50;

    public int MaxHealth { get; private set; } = 100;

    public float CurrentHealth
    {
        get => health;
        private set
        {
            health = value;
            HealthAnimation.Instance.SetHealth(health / MaxHealth);
        }
    }

    private float health;

    public int XP { get; private set; }

    private Vector2Int m_minMaxLocationActions = new(2, 5);
    private int m_actionsLeft;

    private TextBox m_console;
    private Enemy currentTargetEnemy;

    private bool inAction = false;
    private bool inCombat = false;
    private bool attacking = false;

    private bool isHerosTurn = false;

    public HeroState m_currentState;

    #region Unity Methods

    private void Awake()
    {
        m_console = FindObjectOfType<TextBox>();
        InitHero();
        NewActionAmount();
    }

    private void Start()
    {
        m_currentState = HeroState.Idle;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.P))
            AddXP(1000);
#endif

        if (CurrentHealth <= 0)
        {
            inCombat = false;
            inAction = false;
            m_currentState = HeroState.Dead;
        }

        if (inCombat && m_currentState == HeroState.Encounter)
        {
            if (isHerosTurn && !attacking)
            {
                StartCoroutine(Attack());
                return;
            }
            else if (!EncounterManager.Instance.IsAttacking && !attacking)
            {
                StartCoroutine(EncounterManager.Instance.Attack());
            }

            return;
        }

        if (inAction) return;

        switch (m_currentState)
        {
            case HeroState.Idle:
                HeroIdle();
                break;

            case HeroState.Travel:
                HeroTravel();
                break;

            case HeroState.Wander:
                HeroWander();
                break;

            case HeroState.Encounter:
                HeroEncounter();
                break;

            case HeroState.Dead:
                HeroDead();
                break;

            case HeroState.None:
                Debug.Log("Nothing Happens!");
                break;

            case HeroState.Heal:
                HeroHeal();
                break;

            case HeroState.Sell:
                HeroSell();
                break;
            case HeroState.Pockets:
                HeroPockets();
                break;
        }
    }

    #endregion Unity Methods

    public Location GetRandomLocation(List<Location> locations)
    => locations[UnityEngine.Random.Range(0, locations.Count)];

    public void SetHerosTurn()
    => isHerosTurn = true;

    private void InitHero()
    {
        health = MaxHealth;
        CurrentLocation = GetRandomLocation(StartingLocations);
    }

    private void HeroPockets()
    {
        inAction = true;
        StartCoroutine(CheckPockets());
    }

    private IEnumerator CheckPockets()
    {
        m_console.AddLine($"The hero unsure of his wealth checked all of his pockets and has a total of {Gold} gold.");
        yield return new WaitForSeconds(3f);
        StartCoroutine(ChangeState(1, HeroState.Idle));
    }

    private void HeroHeal()
    {
        inAction = true;
        StartCoroutine(Heal());
    }

    private IEnumerator Heal()
    {
        var notHere = false;
        if (CurrentLocation.HasChurch) m_console.AddLine("The hero went to the church looking to be healed");
        else
        {
            m_console.AddLine("The hero is looking to be healed but has not found a healer nearby");
            notHere = true;
        }
        yield return new WaitForSeconds(3f);

        if (CurrentLocation.HasChurch && Gold >= 15 && notHere == false)
        {
            m_console.AddLine("The hero made a small donation to the church and in return he has been healed.");
            CurrentHealth = Mathf.Clamp(CurrentHealth, CurrentHealth + (MaxHealth * .25f), MaxHealth);
            Gold -= 15;
        }
        else if(notHere == false)
            m_console.AddLine("The hero lacks funds in order to recivce healing from the church and has to find another method");
        yield return new WaitForSeconds(3f);

        StartCoroutine(ChangeState(1, HeroState.Idle));
    }

    private void HeroSell()
    {
        inAction = true;
        StartCoroutine(Sell());
    }

    private IEnumerator Sell()
    {
        MapAnimation.Instance.SetMerchantVisible(true);
        m_console.AddLine("The hero encountered a local merchant and has decided to try and sell his roots");
        yield return new WaitForSeconds(3f);
        MapAnimation.Instance.AnimateMerchant();
        m_console.AddLine($"The hero has sold his items for {Inventory.Instance.GetTotalWorth()} Gold!");
        Gold += Inventory.Instance.GetTotalWorth();
        Inventory.Instance.RemoveAll();
        yield return new WaitForSeconds(4.5f);
        MapAnimation.Instance.SetMerchantVisible(false);

        StartCoroutine(ChangeState(.5f, HeroState.Idle));
    }

    public void EndCombat()
    {
        MapAnimation.Instance.SetEnemyVisible(false);
        m_currentState = HeroState.Idle;
        inAction = false;
        inCombat = false;
        isHerosTurn = false;
        EncounterManager.Instance.IsAttacking = false;
        EncounterManager.Instance.CurrentEnemyType = null;
    }

    public void ReciveAttack(EnemyAttack attack)
    {
        CurrentHealth -= attack.BasePower;
    }

    private IEnumerator Attack()
    {
        MapAnimation.Instance.AnimatePlayer();
        attacking = true;
        var skill = SkillManager.Instance.GetRandomSkill();
        if (skill == null)
        {
            m_console.AddLine("The hero lacks any skills and is unable to attack!");
            yield return new WaitForSeconds(2f);
            yield return null;
        }
        else EncounterManager.Instance.ReciveAttack(skill);
        yield return new WaitForSeconds(4f);
        isHerosTurn = false;
        attacking = false;
    }

    private void HeroDead()
    {
        inAction = true;
        StartCoroutine(Lose());
    }

    private IEnumerator Lose()
    {
        MapAnimation.Instance.KillPlayer();
        m_console.AddLine("The hero has reached his final moments, and is DEAD!");
        yield return new WaitForSeconds(3.5f);
        inAction = false;
        m_currentState = HeroState.None;
    }

    private void HeroIdle()
    {
        var selectedAction = Random.Range(0, 4);
        inAction = true;
        if (m_actionsLeft > 0)
        {
            m_actionsLeft--;

            if(CurrentHealth < MaxHealth * .25f && CurrentLocation.HasChurch)
            {
                StartCoroutine(ChangeState(1f, HeroState.Heal));
                return;
            }

            var chance = Random.Range(0, 100);
            if (CurrentLocation.EncounterProbability >= chance)
            {
                currentTargetEnemy = CurrentLocation.GetRandomEnemy();
                StartCoroutine(ChangeState(1f, HeroState.Encounter));
                return;
            }

            switch (selectedAction)
            {
                case 0:
                    StartCoroutine(ChangeState(1, HeroState.Wander));
                    return;
                case 1:
                    StartCoroutine(ChangeState(1, HeroState.Sell));
                    return;
                case 2:
                    StartCoroutine(ChangeState(1, HeroState.Heal));
                    return;
                case 3:
                    StartCoroutine(ChangeState(1, HeroState.Pockets));
                    return;

                default: return;
            }
        }
        else
        {
            StartCoroutine(ChangeState(3, HeroState.Travel));
            return;
        }
    }

    private void HeroTravel()
    {
        MapAnimation.Instance.AnimatePlayer();
        inAction = true;
        CurrentLocation = CurrentLocation.GetRandomAccessibleLocation();
        m_console.AddLine($"The hero has traveled to {CurrentLocation.Name}");
        NewActionAmount();
        StartCoroutine(ChangeState(3, HeroState.Idle));
    }

    private void HeroEncounter()
    {
        if (inAction || inCombat || m_currentState != HeroState.Encounter) return;
        inAction = true;
        StartCoroutine(StartCombat());
    }

    private IEnumerator StartCombat()
    {
        Debug.Log("Encountered an enemy!");
        MapAnimation.Instance.SetEnemyVisible(true);
        m_console.AddLine($"The hero has encounterd a {currentTargetEnemy.name}");
        yield return new WaitForSeconds(3.5f);
        inCombat = true;
        EncounterManager.Instance.InstantiateEnemy(currentTargetEnemy);
    }

    public void AddXP(int amountToAdd)
    {
        XP += amountToAdd;
        CounterAnimation.Instance.SetCounter(XP);
        Debug.Log("New XP: " + XP);
    }

    public void RemoveXP(int amountToRemove)
    {
        XP -= amountToRemove;
        CounterAnimation.Instance.SetCounter(XP);
        Debug.Log("New XP: " + XP);
    }

    private void HeroWander()
    {
        MapAnimation.Instance.AnimatePlayer();
        inAction = true;
        m_console.AddLine($"The hero is taking a stroll around {CurrentLocation.Name} and taking in the sights");
        StartCoroutine(ChangeState(Random.Range(3, 5), HeroState.Idle));
    }

    private IEnumerator ChangeState(float time, HeroState state)
    {
        yield return new WaitForSeconds(time);
        m_currentState = state;
        inAction = false;
    }

    public void NewActionAmount()
    => m_actionsLeft = UnityEngine.Random.Range(m_minMaxLocationActions.x, m_minMaxLocationActions.y);
}