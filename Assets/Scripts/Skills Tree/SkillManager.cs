using System;
using System.Collections.Generic;
using UnityEngine;

namespace Skills
{
    public enum Attribute
    {
        Strength,
        Dexterity,
        Constitution,
        Intelligence,
        Wisdom,
        Luck
    }

    [System.Serializable]
    public class Stat
    {
        [Header("Stat Options")]
        public string name;

        [Space] public Attribute attribute;

        public Stat(string name, Attribute attribute, float modifier)
        {
            this.name = name;
            this.attribute = attribute;
            Modifier = modifier;
        }

        [Space, Range(-100f, 100f)]
        public float Modifier;
    }

    public class SkillManager : Singleton<SkillManager>
    {
        public List<Skill> m_skills = new();

        public static Action<Skill> OnSkillAdded;

        TextBox m_console;

        #region Unity Methods

        private void Awake()
        {
            m_console = FindObjectOfType<TextBox>();
        }

        private void Start()
        {
            m_skills.Clear();
            OnSkillAdded += AddSkill;
        }

        private void OnDestroy()
        {
            OnSkillAdded -= AddSkill;
        }

        #endregion Unity Methods

        public void AddSkill(Skill skill)
        => m_skills.Add(skill);

        public Skill GetRandomSkill()
        {
            if(m_skills.Count == 0) return null;
            
            return m_skills[UnityEngine.Random.Range(0, m_skills.Count)];
        }

        public bool CanUnlock(SkillTreeItem skillItem)
        {
            var skills = skillItem.GetPrerequisite();

            if (Hero.Instance.XP < skillItem.GetSkill().XpNeededToUnlock) return false;

            foreach (var skill in skills)
            {
                if (!m_skills.Contains(skill.GetSkill())) return false;
            }

            return true;
        }

        public float GetAttributeModifier(Attribute attribute)
        {
            var total = 0f;
            m_skills.ForEach(skill => {
                total += skill.GetAttributeModifier(attribute);
            });
            return total;
        }
    }
}