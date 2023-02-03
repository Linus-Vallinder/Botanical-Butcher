using UnityEngine;
using UnityEngine.EventSystems;

namespace Skills
{
    public class SkillTreeItem : MonoBehaviour, IPointerClickHandler
    {
        [Header("Skill Tree Item Options")]
        [SerializeField] private Skill m_skill;
        [Space, SerializeField] private Sprite m_icon;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Adding skill!");
            SkillManager.OnSkillAdded(m_skill);
        }
    }
}