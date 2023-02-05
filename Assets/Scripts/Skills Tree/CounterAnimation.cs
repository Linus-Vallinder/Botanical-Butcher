using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounterAnimation : Singleton<CounterAnimation>
{
    [SerializeField] bool debug;
    [SerializeField] TextMeshPro counter;
    [SerializeField] List<SkinnedMeshRenderer> roots = new List<SkinnedMeshRenderer>();
    [SerializeField] AnimationCurve[] rootAnimCurve;
    [SerializeField] AnimationCurve scaleAnimCurve;
    [SerializeField] float rootsAnimSpeed;
    [SerializeField] float animScaleMultiplier;
    [SerializeField] float counterAnimDelay;
    float animValue = 1.0f;
    float counterTimer;
    int amount;
    int targetAmount;
    float startScale;

    void Start()
    {
        counter.text = amount.ToString();
        startScale = transform.localScale.x;
    }

    void Update()
    {
        if(debug)
        {
            if(Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetCounter(5);
            }
        }

        if(Time.time > counterTimer && amount != targetAmount)
        {
            counterTimer = Time.time + counterAnimDelay;
            if (targetAmount > amount)
            {
                amount++;
            }
            else if(targetAmount < amount)
            {
                amount--;
            }
            counter.text = amount.ToString();
        }

        if(animValue < 1.0f)
        {
            animValue += Time.deltaTime * rootsAnimSpeed;
            for (int i = 0; i < roots.Count; ++i)
            {
                roots[i].SetBlendShapeWeight(0, rootAnimCurve[i].Evaluate(animValue) * 100.0f);
            }
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, startScale * animScaleMultiplier, scaleAnimCurve.Evaluate(animValue));
        }
    }

    public void SetCounter(int newAmount)
    {
        if(newAmount < 25)
        {
            AudioManager.Instance.PlaySound("RootXpShort");
        }
        else if (newAmount < 50)
        {
            AudioManager.Instance.PlaySound("RootXpMedium");
        }
        else
        {
            AudioManager.Instance.PlaySound("RootXpLong");
        }
        
        targetAmount = newAmount;
        counterTimer = Time.time;
        if (newAmount > amount)
        { 
            animValue = 0.0f;
        }
    }
}
