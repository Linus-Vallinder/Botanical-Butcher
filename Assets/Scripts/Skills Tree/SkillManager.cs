using System;
using System.Collections.Generic;
using System.Linq;
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

    public class SkillManager : MonoBehaviour
    {
        private List<Skill> m_skills = new();

        public static SkillManager Instance;
        public static Action<Skill> OnSkillAdded;

        #region Unity Methods

        private void Awake()
        {
            //"Singleton"
            if (Instance != this)
                Instance = this;
        }

        private void Start()
        {
            OnSkillAdded += AddSkill;
        }

        private void OnDestroy()
        {
            OnSkillAdded -= AddSkill;
        }

        #endregion Unity Methods

        public void AddSkill(Skill skill)
        {
            if (skill) m_skills.Add(skill);
            else Debug.LogWarning($"Tried to add {skill.Name} and failed!");
        }

        public bool CanUnlock(SkillTreeItem skillItem)
        {
            bool result = true;
            var skills = skillItem.GetPrerequisite();

            foreach (var skill in skills)
            {
                if (!m_skills.Contains(skill.GetSkill())) result = false;
            }

            return result;
        }

        public float GetAttributeModifier(Attribute attribute)
        {
            var total = 0f;
            var attributes = m_skills.Where(x => x.Stat.attribute == attribute).ToList();
            attributes.ForEach(skill => { if (skill.Stat.attribute == attribute) total += skill.Stat.Modifier; });
            return total;
        }
    }
}