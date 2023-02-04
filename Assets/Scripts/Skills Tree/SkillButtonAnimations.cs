using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillButtonAnimations : MonoBehaviour
{
    [SerializeField] bool debug;

    [SerializeField] TextMeshPro skillName;
    [SerializeField] TextMeshPro skillInfo;

    [SerializeField] List<SkinnedMeshRenderer> roots = new List<SkinnedMeshRenderer>();
    [SerializeField] SkinnedMeshRenderer infoPanel;

    [SerializeField] AnimationCurve[] rootOpenCurve;
    [SerializeField] float rootOpenSpeed;
    [SerializeField] AnimationCurve infoPanelOpenCurve;
    [SerializeField] float infoPanelOpenSpeed;

    float rootTarget = 1.0f;
    float rootValue = 1.0f;
    float infoTarget = 1.0f;
    float infoValue = 1.0f;

    void Start()
    {
        foreach (SkinnedMeshRenderer root in roots)
        {
            root.SetBlendShapeWeight(0, 100.0f);
        }
        infoPanel.SetBlendShapeWeight(0, 100.0f);
        skillInfo.alpha = 0.0f;
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
            infoPanel.SetBlendShapeWeight(0, infoPanelOpenCurve.Evaluate(infoValue) * 100.0f);
            skillInfo.alpha = 1.0f - infoValue;
        }
        else if (infoTarget < infoValue)
        {
            infoValue -= Time.deltaTime * infoPanelOpenSpeed;
            infoValue = Mathf.Clamp01(infoValue);
            infoPanel.SetBlendShapeWeight(0, infoPanelOpenCurve.Evaluate(infoValue) * 100.0f);
            skillInfo.alpha = 1.0f - infoValue;
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
        }
    }

    public void Init(string skill, string info)
    {
        skillName.text = skill;
        skillInfo.text = info;
    }

    public void OpenInfoPanel()
    {
        Debug.Log("Open info panel");
        infoTarget = 0.0f;
    }

    public void CloseInfoPanel()
    {
        Debug.Log("Close info panel");
        infoTarget = 1.0f;
    }

    public void UnlockSkill()
    {
        Debug.Log("Unlock skill");
        rootTarget = 0.0f;
    }

    public void LockSkill()
    {
        Debug.Log("Lock skill");
        rootTarget = 1.0f;
    }


}
