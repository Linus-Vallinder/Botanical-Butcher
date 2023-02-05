using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        [SerializeField] private List<TreeTypeData> m_trees = new();

        [SerializeField] private SkillTreeItem m_skillTreeItemPrefab;
        [Space, SerializeField] private int MaxWidth = 4;
        [SerializeField] private int MinWdith = 2;
        [Space, SerializeField] private Vector2 m_skillOffset;
        [SerializeField] private float RandomSpreadX = 2;
        [SerializeField] private float RandomSpreadY = 2;

        #region Unity Methods

        private void Start()
        {
            GenerateTree(m_trees[0]);
        }

        #endregion Unity Methods

        public void GenerateTree(TreeTypeData tree)
        {
            SkillTreeItem SpawnItem(Skill skill, Vector3 pos)
            {
                var item = Instantiate(m_skillTreeItemPrefab);
                item.transform.position = pos;
                item.InitSkill(skill);

                return item;
            }

            Skill GetRandomSkill(IEnumerable<Skill> skill)
            => skill.ToList()[Random.Range(0, skill.Count())];

            var powerLevels = tree.Skills.Select(x => x.PowerLevel).Distinct().Count();
            var items = new List<SkillTreeItem>();

            if (tree.RootSkill)
            {
                var root = SpawnItem(tree.RootSkill, tree.StartPosition);
                items.Add(root);
            }
            else if (tree.Skills.Count != 0)
            {
                var root = SpawnItem(tree.Skills[0], tree.StartPosition);
                items.Add(root);
            }
            else Debug.LogWarning("Can not generate tree! Skills are required!");

            for (int i = 0; i < powerLevels; i++)
            {
                var skills = tree.Skills.Where(x => x.PowerLevel == i + 1).ToList();
                var layerAmounts = 0;
                if (skills.Count() >= MinWdith) layerAmounts = Random.Range(MinWdith, MaxWidth);
                else layerAmounts = Random.Range(1, skills.Count());

                for (int j = 0; j < layerAmounts; j++)
                {
                    //Get Spawn Position
                    var posX = (j * m_skillOffset.x);

                    //posX *= m_skillOffset.x;
                    var posY = (i + 1) * m_skillOffset.y;
                    
                    //Spawn Skill
                    var skill = GetRandomSkill(skills);
                    var item = SpawnItem(skill, new Vector3(posX, -posY) + tree.StartPosition + new Vector3(Random.Range(-RandomSpreadX, RandomSpreadX), Random.Range(-RandomSpreadY, RandomSpreadY), 0.0f));

                    items.Add(item);
                    skills.Remove(skill);

                    //Add prerequisites
                    var skillsAbove = new List<SkillTreeItem>();
                    if (skill.PowerLevel == 0) continue;
                    skillsAbove = items.Where(x => x.GetSkill().PowerLevel == skill.PowerLevel - 1).ToList();
                    if (skillsAbove.Count == 1) item.SetPrerequisite(skillsAbove[0]);
                    else
                    {
                        for (int l  = 0; l < Random.Range(1, 2); l++)
                        {
                            var randomSkill = skillsAbove[Random.Range(0, skillsAbove.Count)];
                            item.SetPrerequisite(randomSkill);
                            skillsAbove.Remove(randomSkill);
                        }
                    }

                    item.DrawLines();
                }
            }
        }
    }
}