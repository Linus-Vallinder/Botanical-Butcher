using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

namespace Skills
{
    public class SkillTreeItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Skill Tree Item Options")]
        [SerializeField] private Skill m_skill;
        [Space, SerializeField] private TextMeshPro m_name;
        [SerializeField] private TextMeshPro m_description;
        [SerializeField] private SpriteRenderer m_iconRenderer;

        [Space, SerializeField] private List<SkillTreeItem> m_prerequisite = new();

        [Header("Effects")]
        [SerializeField] private LineParticles m_linePrefab;

        public bool Unlocked { get; private set; } = false;

        public Skill GetSkill() => m_skill;
        public List<SkillTreeItem> GetPrerequisite() => m_prerequisite;
        public void SetPrerequisite(SkillTreeItem item) => m_prerequisite.Add(item);

        private SkillButtonAnimations m_animations;

        #region Unity Methods

        private void Awake()
        {
            m_animations = GetComponentInChildren<SkillButtonAnimations>();    
        }

        #endregion

        public void InitSkill(Skill skill)
        {
            m_skill = skill;

            m_name.text = m_skill.Name;
            m_description.text = m_skill.Description;
            m_iconRenderer.sprite = m_skill.Icon;
        }

        public void DrawLines()
        {
            //Draw Lines to Prerequisite skills
            foreach (var preSkill in m_prerequisite)
            {
                var line = Instantiate(m_linePrefab);
                line.transform.parent = transform;
                var lineComp = line.GetComponent<LineRenderer>();
                lineComp.SetPositions(new Vector3[] { transform.position, preSkill.transform.position });
            }
        }

        #region IPointer Interface Implementation

        public void OnPointerClick(PointerEventData eventData) 
        => Unlock();

        public void OnPointerEnter(PointerEventData eventData)
        => m_animations.OpenInfoPanel();

        public void OnPointerExit(PointerEventData eventData)
        => m_animations.CloseInfoPanel();

        #endregion IPointer Interface Implementation

        private void Unlock()
        {
            Debug.LogWarning(Unlocked);

            if (Unlocked || !SkillManager.Instance.CanUnlock(this))
            {
                Debug.LogWarning("Cannot unlock this skill, it is either already unlocked or you do not have the right prerequisites");
                return;
            }

            SkillManager.OnSkillAdded(m_skill);
            m_animations.UnlockSkill();
            Unlocked = true;

            Debug.Log($"You have unlocked the [{m_skill.Name}] skill!");
        }
    }
}