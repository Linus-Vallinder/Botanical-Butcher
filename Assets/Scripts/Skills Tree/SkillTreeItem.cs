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
        [SerializeField] private MeshFilter m_iconRenderer;
        [SerializeField] private AudioClip m_audioClip;

        [Space, SerializeField] private List<SkillTreeItem> m_prerequisite = new();

        [Header("Effects")]
        [SerializeField] private LineParticles m_linePrefab;

        public bool Unlocked { get; private set; } = false;

        public Skill GetSkill() => m_skill;
        public List<SkillTreeItem> GetPrerequisite() => m_prerequisite;
        public void SetPrerequisite(SkillTreeItem item) => m_prerequisite.Add(item);

        private SkillButtonAnimations m_animations;

        private TextBox m_console;

        private LineParticles line;

        #region Unity Methods

        private void Awake()
        {
            m_console = FindObjectOfType<TextBox>();
            m_animations = GetComponentInChildren<SkillButtonAnimations>();    
        }

        #endregion

        public void InitSkill(Skill skill)
        {
            m_skill = skill;

            m_name.text = m_skill.Name;
            m_description.text = m_skill.Description;
            m_iconRenderer.sharedMesh = m_skill.Icon;
            m_audioClip = m_skill.AudioClip;
        }

        public void DrawLines()
        {
            //Draw Lines to Prerequisite skills
            foreach (var preSkill in m_prerequisite)
            {
                line = Instantiate(m_linePrefab);
                line.transform.parent = transform;
                line.SetLine(new Vector3[] { preSkill.transform.position, transform.position });
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

            if ( Unlocked ||
                 !SkillManager.Instance.CanUnlock(this) ||
                 Hero.Instance.XP < m_skill.XpNeededToUnlock)
            {
                Debug.LogWarning("Cannot unlock this skill, it is either already unlocked or you do not have the right prerequisites");
                return;
            }

            SkillManager.OnSkillAdded(m_skill);
            Hero.Instance.RemoveXP(m_skill.XpNeededToUnlock);
            m_animations.UnlockSkill();
            if(line)
            { 
                line.Activate();
            }
            if(m_audioClip)
            {
                Invoke("PlaySound", 1.6f);
            }
            Unlocked = true;

            m_console.AddLine($"You have unlocked the [{m_skill.Name}] skill!");
        }

        void PlaySound()
        {
            AudioManager.Instance.PlaySound(m_audioClip);
        }
    }
}