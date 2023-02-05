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
        public int XpNeededToUnlock;
        public Mesh Icon;
        public AudioClip AudioClip;

        [Space, TextArea] public string Description;

        [Space, Header("Usage Promtps")]
        public List<string> SkillUsagePrompts = new();

        [Header("Stat Options")]

        public bool ShowStats;
        public List<Stat> Stats = new();

        public string GetRandomUsagePrompt()
        {
            if (SkillUsagePrompts.Count == 0) return $"The hero has used the [{Name}] skill!";
            else return SkillUsagePrompts[Random.Range(0, SkillUsagePrompts.Count)];
        }

        public float GetAttributeModifier(Attribute attribute) {
            var total = 0f;
            Stats.ForEach( stat => {
                if(stat.attribute == attribute) total += stat.Modifier;
            });
            return total;
        }

    }
}