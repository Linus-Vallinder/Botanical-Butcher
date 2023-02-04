using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using static UnityEngine.EventSystems.EventTrigger;

enum HeroState
{
    Idle,
    Travel,
    Wander,
    Encounter
}

public class Hero : MonoBehaviour
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

    bool inAction = false;
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
                break;
            case HeroState.Encounter:
                break;
        }
    }

    #endregion

    public Location GetRandomLocation(List<Location> locations)
    => locations[UnityEngine.Random.Range(0, locations.Count)];

    private void InitHero()
    {
        CurrentHealth = MaxHealth;
        CurrentLocation = GetRandomLocation(StartingLocations); 
    }

    void HeroIdle()
    {
        inAction = true;
        StartCoroutine(ChangeState(3, HeroState.Travel));
    }

    void HeroTravel()
    {
        inAction = true;
        CurrentLocation = CurrentLocation.GetRandomAccessibleLocation();
        m_console.AddLine($"The hero has traveled to {CurrentLocation.Name}");
        StartCoroutine(ChangeState(3, HeroState.Idle));
    }

    IEnumerator ChangeState(float time, HeroState state)
    {
        yield return new WaitForSeconds(time);
        m_currentState = state;
        inAction = false;
    }


    //public IEnumerator DoRandomAction()
    //{
    //    inAction = true;
    //    yield return new WaitForSeconds(UnityEngine.Random.Range(1.1f, 1.5f));
    //    if (m_actionsLeft < 1) Move(CurrentLocation.GetRandomAccessibleLocation());

    //    var encounterCounter = UnityEngine.Random.Range(0f, 100f);
    //    if (encounterCounter <= CurrentLocation.EncounterProbability)
    //    {
    //        StartCoroutine(StartEncounter(CurrentLocation.GetRandomEnemy()));
    //        inAction= false;
    //        OnActionComplete?.Invoke();
    //        yield return null;
    //    }

    //    int selectedAction = UnityEngine.Random.Range(0, 0);
    //    switch (selectedAction)
    //    {
    //        case 0:
    //            StartCoroutine(WanderLocation());
    //            break;
    //        case 1:
    //            break;
    //        case 2:
    //            break;
    //        case 3:
    //            break;
    //        case 4:
    //            break;
    //        case 5:
    //            break;
    //        default:
    //            break;
    //    }

    //    m_actionsLeft--;
    //    inAction = false;
    //}

    //private IEnumerator StartEncounter(Enemy enemy)
    //{
    //    inAction= true;
    //    Debug.Log($"You have Encountered a {enemy}");
    //    m_console.AddLine($"You have Encountered a {enemy}");
    //    yield return new WaitForSeconds(UnityEngine.Random.Range(4, 8));
    //    inAction = false;
    //}

    //private IEnumerator WanderLocation()
    //{
    //    inAction = true;
    //    var encounterCounter = UnityEngine.Random.Range(0f, 100f);
    //    if (encounterCounter <= CurrentLocation.EncounterProbability)
    //    {
    //        StartCoroutine(StartEncounter(CurrentLocation.GetRandomEnemy()));
    //        OnActionComplete?.Invoke();
    //        yield return null; //Return to stay inAction then handle encounter to go out of action state
    //    }

    //    Debug.Log($"The hero wanders around {CurrentLocation.Name} taking in the sights!");
    //    m_console.AddLine($"The hero wanders around {CurrentLocation.Name} taking in the sights!");
    //    yield return new WaitForSeconds(UnityEngine.Random.Range(4, 8));
    //    inAction = false;
    //    OnActionComplete?.Invoke();
    //}

    public void NewActionAmount()
    => m_actionsLeft = UnityEngine.Random.Range(m_minMaxLocationActions.x, m_minMaxLocationActions.y);

    public void Move(Location location)
    {
        CurrentLocation = location;
        Debug.Log($"The Hero has Traveled to the {location.Name}");
        NewActionAmount();
    }
}
