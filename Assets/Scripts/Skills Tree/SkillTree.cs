using UnityEngine;
using System.Collections.Generic;

namespace Skills
{
    [System.Serializable]
    public class TreeTypeData
    {
        public Skill RootSkill;
        public List<Skill> Skills = new();
        public Vector3 StartPosition = Vector3.zero;
    }

    public class SkillTree : MonoBehaviour
    {
        [Header("Skill Tree Options")]
        [SerializeField] private List<TreeTypeData> Trees = new();

        [Space, SerializeField] private int MaxWidth = 4;

        public void GenerateTree(List<Skill> skills)
        {

        }
    }
}