using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Location", menuName = "Hero/New Location", order = 1)]
public class Location : ScriptableObject
{
    [Header("Location Options")]
    public string Name;
    public bool HasChurch;

    [Space]
    public List<Enemy> SpawnPool = new();
    [Range(0f, 100f)] public float EncounterProbability = 15f;

    [Space]
    public List<Location> AccessibleLocations = new();
    
    public Enemy GetRandomEnemy()
    => SpawnPool[Random.Range(0, SpawnPool.Count)];

    public Location GetRandomAccessibleLocation()
    => AccessibleLocations[Random.Range(0, AccessibleLocations.Count)]; 
}