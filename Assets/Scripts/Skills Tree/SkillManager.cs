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

        public static SkillManager instance;

        #region Unity Methods

        private void Awake()
        {
            //"Singleton"
            if (instance != this)
                instance = this;
        }

        #endregion Unity Methods

        public void AddSkill(Skill skill) => m_skills.Add(skill);

        public float GetAttributeModifier(Attribute attribute)
        {
            var total = 0f;
            var attributes = m_skills.Where(x => x.Stat.attribute == attribute).ToList();
            attributes.ForEach(skill => { if (skill.Stat.attribute == attribute) total += skill.Stat.Modifier; });
            return total;
        }
    }
}