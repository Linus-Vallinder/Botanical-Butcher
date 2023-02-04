using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

namespace Skills
{
    public class SkillTreeItem : MonoBehaviour, IPointerClickHandler
    {
        [Header("Skill Tree Item Options")]
        [SerializeField] private Skill m_skill;
        [Space, SerializeField] private TextMeshPro m_name;
        [SerializeField] private TextMeshPro m_description;
        [SerializeField] private SpriteRenderer m_iconRenderer;

        [Space, SerializeField] private List<SkillTreeItem> m_prerequisite = new();

        public bool Unlocked { get; private set; } = false;

        public Skill GetSkill() => m_skill;
        public List<SkillTreeItem> GetPrerequisite() => m_prerequisite;
        public void SetPrerequisite(SkillTreeItem item) => m_prerequisite.Add(item);

        public void InitSkill(Skill skill)
        {
            m_skill = skill;

            m_name.text = m_skill.Name;
            m_description.text = m_skill.Description;
            m_iconRenderer.sprite = m_skill.Icon;
            DrawLines();
        }

        private void DrawLines()
        {
            //Draw Lines to Prerequisite skills
            foreach (var preSkill in m_prerequisite)
            {
                var line = new GameObject("line");
                line.transform.SetParent(transform, false);
                var lineComp = line.AddComponent<LineRenderer>();
                lineComp.SetPositions(new Vector3[2] { transform.position, preSkill.transform.position });
                lineComp.startWidth = .1f;
                lineComp.endWidth = .1f;
            }
        }

        #region IPointer Interface Implementation

        public void OnPointerClick(PointerEventData eventData) 
        => Unlock();

        #endregion IPointer Interface Implementation

        private void Unlock()
        {
            if (Unlocked || !SkillManager.Instance.CanUnlock(this))
            {
                Debug.LogWarning("Cannot unlock this skill, it is either already unlocked or you do not have the right prerequisites");
                return;
            }

            SkillManager.OnSkillAdded(m_skill);
            Unlocked = true;

            Debug.Log($"You have unlocked the [{m_skill.Name}] skill!");
        }
    }
}