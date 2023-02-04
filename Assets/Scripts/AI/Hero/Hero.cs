using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Hero : MonoBehaviour
{
    [SerializeField] private List<Location> StartingLocations = new();

    public Location CurrentLocation { get; private set; }

    public int MaxHealth { get; private set; } = 100;
    public int CurrentHealth { get; private set; }
    
    public int XP { get; private set; }
    public int Gold { get; private set; }

    private Vector2Int minMaxLocationActions = new(2, 5);
    private int actionsLeft;

    private bool inAction = false;

    #region Unity Methods

    private void Awake()
    {
        InitHero();
        NewActionAmount();
    }

    private void Update()
    {
        if(!inAction)
            StartCoroutine(DoRandomAction());
    }

    #endregion

    public Location GetRandomLocation(List<Location> locations)
    => locations[Random.Range(0, locations.Count)];

    private void InitHero()
    {
        CurrentHealth = MaxHealth;
        CurrentLocation = GetRandomLocation(StartingLocations); 
    }

    public IEnumerator DoRandomAction()
    {
        inAction = true;
        yield return new WaitForSeconds(Random.Range(1.1f, 1.5f));
        if (actionsLeft < 1) Move(CurrentLocation.GetRandomAccessibleLocation());

        var encounterCounter = Random.Range(0f, 100f);
        if (encounterCounter <= CurrentLocation.EncounterProbability)
        {
            StartCoroutine(StartEncounter(CurrentLocation.GetRandomEnemy()));
            yield return null; //Return to stay inAction then handle encounter to go out of action state
        }

        int selectedAction = Random.Range(0, 0);
        switch (selectedAction)
        {
            case 0:
                StartCoroutine(WanderLocation());
                yield return null;
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                break;
        }

        actionsLeft--;
        inAction = false;
    }

    private IEnumerator StartEncounter(Enemy enemy)
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
    {
        var encounterCounter = Random.Range(0f, 100f);
        if (encounterCounter <= CurrentLocation.EncounterProbability)
        {
            StartCoroutine(StartEncounter(CurrentLocation.GetRandomEnemy()));
            yield return null; //Return to stay inAction then handle encounter to go out of action state
        }

        Debug.Log($"The hero wanders around {CurrentLocation.Name} taking in the sights!");
        yield return new WaitForSeconds(4f);
        inAction = false;
    }

    public void NewActionAmount()
    => actionsLeft = Random.Range(minMaxLocationActions.x, minMaxLocationActions.y);

    public void Move(Location location)
    {
        CurrentLocation = location;
        Debug.Log($"The Hero has Traveled to the {location.Name}");
        NewActionAmount();
    }
}
