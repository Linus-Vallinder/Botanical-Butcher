using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillButtonAnimations : MonoBehaviour
{
    [SerializeField] bool debug;

    [SerializeField] TextMeshPro skillName;
    [SerializeField] TextMeshPro skillInfo;
    [SerializeField] TextMeshPro skillCost;

    [SerializeField] List<SkinnedMeshRenderer> roots = new List<SkinnedMeshRenderer>();
    [SerializeField] SkinnedMeshRenderer infoPanel;
    [SerializeField] MeshRenderer skillIcon;

    [SerializeField] AnimationCurve[] rootOpenCurve;
    [SerializeField] AnimationCurve rootOpenScaleCurve;
    [SerializeField] float rootOpenSpeed;
    [SerializeField] float rootOpenScale;
    [SerializeField] AnimationCurve infoPanelOpenCurve;
    [SerializeField] float infoPanelOpenSpeed;
    [SerializeField] float infoPanelOpenScale;

    [SerializeField] ParticleSystem[] growParticles;

    [SerializeField] Material cyanMaterial;
    [SerializeField] Material goldMaterial;

    [SerializeField] AudioClip[] chimes;

    float rootTarget = 1.0f;
    float rootValue = 1.0f;
    float infoTarget = 1.0f;
    float infoValue = 1.0f;

    float startScale;

    Color gold = new Color(0.8f, 0.63f, 0.13f);
    Color cyan = new Color(0.18f, 0.74f, 0.57f);

    void Start()
    {
        foreach (SkinnedMeshRenderer root in roots)
        {
            root.SetBlendShapeWeight(0, 100.0f);
        }
        infoPanel.SetBlendShapeWeight(0, 100.0f);
        skillInfo.alpha = 0.0f;
        skillCost.alpha = skillInfo.alpha;
        startScale = transform.localScale.x;
        skillName.color = cyan;
    }

    void Update()
    {
        if(debug)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                OpenInfoPanel();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CloseInfoPanel();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                UnlockSkill();
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                LockSkill();
            }
        }

        if (infoTarget > infoValue)
        {
            infoValue += Time.deltaTime * infoPanelOpenSpeed;
            infoValue = Mathf.Clamp01(infoValue);
            float curveValue = infoPanelOpenCurve.Evaluate(infoValue);
            infoPanel.SetBlendShapeWeight(0, curveValue * 100.0f);
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, startScale * infoPanelOpenScale, 1.0f - curveValue);
            skillInfo.alpha = 1.0f - infoValue;
            skillCost.alpha = skillInfo.alpha;
        }
        else if (infoTarget < infoValue)
        {
            infoValue -= Time.deltaTime * infoPanelOpenSpeed;
            infoValue = Mathf.Clamp01(infoValue);
            float curveValue = infoPanelOpenCurve.Evaluate(infoValue);
            infoPanel.SetBlendShapeWeight(0, curveValue * 100.0f);
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, startScale * infoPanelOpenScale, 1.0f - curveValue);
            skillInfo.alpha = 1.0f - infoValue;
            skillCost.alpha = skillInfo.alpha;
        }

        if (rootTarget > rootValue)
        {
            rootValue += Time.deltaTime * rootOpenSpeed;
            rootValue = Mathf.Clamp01(rootValue);
            for(int i = 0; i < roots.Count; ++i)
            {
                roots[i].SetBlendShapeWeight(0, rootOpenCurve[i].Evaluate(rootValue) * 100.0f);
            }
        }
        else if (rootTarget < rootValue)
        {
            rootValue -= Time.deltaTime * rootOpenSpeed;
            rootValue = Mathf.Clamp01(rootValue);
            for (int i = 0; i < roots.Count; ++i)
            {
                roots[i].SetBlendShapeWeight(0, rootOpenCurve[i].Evaluate(rootValue) * 100.0f);
            }
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, startScale * rootOpenScale, rootOpenScaleCurve.Evaluate(rootValue));
        }
    }

    public void Init(string skill, string info)
    {
        skillName.text = skill;
        skillInfo.text = info;
    }

    public void OpenInfoPanel()
    {
        AudioManager.Instance.PlaySound("SkillDescription");
        //Debug.Log("Open info panel");
        infoTarget = 0.0f;
    }

    public void CloseInfoPanel()
    {
        AudioManager.Instance.PlaySound("SkillDescriptionClose");
        //Debug.Log("Close info panel");
        infoTarget = 1.0f;
    }

    public void UnlockSkill()
    {
        //Debug.Log("Unlock skill");
        rootTarget = 0.0f;
        CloseInfoPanel();
        for(int i = 0; i < growParticles.Length; ++i)
        { 
            growParticles[i].Play();
        }
        skillName.color = gold;
        skillIcon.material = goldMaterial;
        AudioManager.Instance.PlaySound("SkillClick");
        AudioManager.Instance.PlayRandomSoundFromArray(chimes);
    }

    public void LockSkill()
    {
        //Debug.Log("Lock skill");
        rootTarget = 1.0f;
        skillName.color = cyan;
        skillIcon.material = cyanMaterial;
    }
}
