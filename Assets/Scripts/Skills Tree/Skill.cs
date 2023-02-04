using UnityEngine;
using System.Collections.Generic;

namespace Skills
{
    [CreateAssetMenu(fileName = "Skill", menuName = "Skill Tree/New Skill", order = 1)]
    public class Skill : ScriptableObject
    {
        [Header("Skill Options")]
        public string Name;
        public int PowerLevel; //Used for random generation of skill tree
        public Sprite Icon;

        [Space, TextArea] public string Description;

        [Space, Header("Usage Promtps")]
        public List<string> SkillUsagePrompts = new();

        [Header("Stat Options")]

        public bool ShowStats;
        [Space] public Stat Stat;
    }
}