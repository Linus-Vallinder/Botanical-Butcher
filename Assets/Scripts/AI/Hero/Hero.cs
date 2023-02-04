using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;

enum HeroState
{
    Idle,
    Travel,
    Wander,
    Encounter
}

public class Hero : Singleton<Hero>
{
    [SerializeField] private List<Location> StartingLocations = new();

    public Location CurrentLocation { get; private set; }

    public int MaxHealth { get; private set; } = 100;
    public int CurrentHealth { get; private set; }
    
    public int XP { get; private set; }
    public int Gold { get; private set; }

    private Vector2Int m_minMaxLocationActions = new(2, 5);
    private int m_actionsLeft;

    private TextBox m_console;
    private Enemy currentTargetEnemy;

    bool inAction = false;
    bool inCombat = false;
    bool attacking = false;

    bool isHerosTurn = false;

    HeroState m_currentState;

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
        if (inCombat)
        {
            if (isHerosTurn && !attacking)
            {
                StartCoroutine(Attack());
            }
            else if(!EncounterManager.Instance.IsAttacking)
            {
                StartCoroutine(EncounterManager.Instance.Attack());
            }
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
        }
    }

    private IEnumerator Attack()
    {
        attacking = true;
        yield return new WaitForSeconds(2f);
        isHerosTurn = false;
        attacking = false;
    }

    #endregion

    public Location GetRandomLocation(List<Location> locations)
    => locations[UnityEngine.Random.Range(0, locations.Count)];

    public void SetHerosTurn()
    => isHerosTurn = true;

    private void InitHero()
    {
        CurrentHealth = MaxHealth;
        CurrentLocation = GetRandomLocation(StartingLocations); 
    }

    void HeroIdle()
    {
        var selectedAction = Random.Range(0, 0);
        inAction = true;
        if(m_actionsLeft > 0)
        {
            m_actionsLeft--;

            var chance = Random.Range(0, 100);
            if(CurrentLocation.EncounterProbability >= chance)
            {
                currentTargetEnemy = CurrentLocation.GetRandomEnemy();
                StartCoroutine(ChangeState(1, HeroState.Encounter));
                return;
            }

            switch (selectedAction)
            {
                case 0:
                    StartCoroutine(ChangeState(1, HeroState.Wander));
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

    void HeroTravel()
    {
        inAction = true;
        CurrentLocation = CurrentLocation.GetRandomAccessibleLocation();
        m_console.AddLine($"The hero has traveled to {CurrentLocation.Name}");
        NewActionAmount();
        StartCoroutine(ChangeState(3, HeroState.Idle));
    }

    void HeroEncounter()
    {
        Debug.Log($"You have Encountered a {enemy}");
        yield return new WaitForSeconds(2.5f);
        AddXP(13);
        inAction = false;
    }

    public void AddXP(int amountToAdd)
	{
		XP += amountToAdd;
		CounterAnimation.Instance.SetCounter(XP);
		Debug.Log("New XP: " + XP);
	}

    private IEnumerator WanderLocation()
        inAction = true;
        inCombat = true;
        m_console.AddLine($"The hero has encounterd a {currentTargetEnemy.name}");
        EncounterManager.Instance.InstantiateEnemy(currentTargetEnemy);
        //currentTargetEnemy = null;
        //StartCoroutine(ChangeState(Random.Range(3, 5), HeroState.Idle));
    }

    void HeroWander()
    {
        inAction = true;
        m_console.AddLine($"The hero is taking a stroll around {CurrentLocation.Name} and taking in the sights");
        StartCoroutine(ChangeState(Random.Range(3, 5), HeroState.Idle));
    }

    IEnumerator ChangeState(float time, HeroState state)
    {
        yield return new WaitForSeconds(time);
        m_currentState = state;
        inAction = false;
    }

    public void NewActionAmount()
    => m_actionsLeft = UnityEngine.Random.Range(m_minMaxLocationActions.x, m_minMaxLocationActions.y);
}
