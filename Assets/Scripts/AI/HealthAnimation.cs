using UnityEngine;

public class HealthAnimation : Singleton<HealthAnimation>
{
    [SerializeField] private AudioClip[] hurtClips;
    [SerializeField] private bool debug;
    [SerializeField] private Transform healthBar;
    [SerializeField] private float animSpeed;
    [SerializeField] private float shakeAmpunt;
    private float targetValue = 1.0f;
    private float currentValue = 1.0f;
    private Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = transform.position;
    }

    private void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                SetHealth(0.8f);
            }
        }

        if (!Mathf.Approximately(targetValue, currentValue))
        {
            transform.position = defaultPosition + Random.insideUnitSphere * shakeAmpunt * Mathf.Abs(targetValue - currentValue);
            currentValue = Mathf.Lerp(currentValue, targetValue, Time.deltaTime * animSpeed);
            currentValue = Mathf.Clamp01(currentValue);
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
        newValue = Mathf.Clamp01(newValue);
        AudioManager.Instance.PlayRandomSoundFromArray(hurtClips);
        targetValue = newValue;
    }
}