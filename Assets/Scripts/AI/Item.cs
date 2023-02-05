using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [TextArea] public string Description;
    [Space] public int value;
}
