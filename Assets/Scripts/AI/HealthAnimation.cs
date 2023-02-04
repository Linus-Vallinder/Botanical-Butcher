using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAnimation : Singleton<HealthAnimation>
{
    [SerializeField] bool debug;
    [SerializeField] Transform healthBar;
    [SerializeField] float animSpeed;
    float targetValue = 1.0f;
    float currentValue = 1.0f;

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
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * animSpeed);
            healthBar.localScale = new Vector3(1.0f, currentValue, 1.0f);
        }
    }

    /// <summary>
    /// Set HP between 0 and 1
    /// </summary>
    /// <param name="newValue">1 = 100%</param>
    public void SetHealth(float newValue)
    {
        targetValue = newValue;
    }
}
