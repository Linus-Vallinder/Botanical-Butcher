using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAnimation : Singleton<HealthAnimation>
{
    [SerializeField] AudioClip[] hurtClips;
    [SerializeField] bool debug;
    [SerializeField] Transform healthBar;
    [SerializeField] float animSpeed;
    [SerializeField] float shakeAmpunt;
    float targetValue = 1.0f;
    float currentValue = 1.0f;
    Vector3 defaultPosition;

    void Start()
    {
        defaultPosition = transform.position;
    }

    void Update()
    {
        if(debug)
        {
            if(Input.GetKeyDown(KeyCode.Alpha8))
            {
                SetHealth(0.8f);
            }
        }

        if(!Mathf.Approximately(targetValue, currentValue))
        {
            transform.position = defaultPosition + Random.insideUnitSphere * shakeAmpunt * Mathf.Abs(targetValue - currentValue);
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * animSpeed);
            healthBar.localScale = new Vector3(1.0f, currentValue, 1.0f);
        }
        else
        {
            transform.position = defaultPosition;
        }
    }

    /// <summary>
    /// Set HP between 0 and 1
    /// </summary>
    /// <param name="newValue">1 = 100%</param>
    public void SetHealth(float newValue)
    {
        AudioManager.Instance.PlayRandomSoundFromArray(hurtClips);
        targetValue = newValue;
    }
}
