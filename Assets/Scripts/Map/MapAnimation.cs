using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAnimation : Singleton<MapAnimation>
{
    [SerializeField] AnimationCurve animCurve;
    [SerializeField] float scaleMultiplier;
    [SerializeField] float speed;
    [SerializeField] float scrollSpeed;
    [SerializeField] float scrollTime;
    [SerializeField] Transform player;
    [SerializeField] Transform enemy;
    [SerializeField] Transform merchant;
    [SerializeField] Material enemyMaterial;
    [SerializeField] Material deathMaterial;
    [SerializeField] MeshRenderer mapScroll;

    Transform activeTransform;
    Vector3 defaultScale;
    float animValue = 1.0f;
    Vector2 offset = new Vector2(0f, 0f);
    float scrollTimer;

    void Start()
    {
        defaultScale = player.localScale;
        activeTransform = player;
        SetEnemyVisible(false);
        SetMerchantVisible(false);
    }

    void Update()
    {
        if(animValue < 1.0f)
        {
            animValue += Time.deltaTime;
            activeTransform.localScale = Vector3.Lerp(defaultScale, defaultScale * scaleMultiplier, animCurve.Evaluate(animValue));
        }

        if(Time.time < scrollTimer)
        { 
            offset += Vector2.up * scrollSpeed * Time.deltaTime;
            mapScroll.material.SetTextureOffset("_MainTex", offset);
        }
    }

    public void AnimatePlayer()
    {
        InitAnim();
        activeTransform = player;
    }

    public void AnimateEnemy()
    {
        InitAnim();
        activeTransform = enemy;
    }

    public void AnimateMerchant()
    {
        InitAnim();
        activeTransform = merchant;
    }

    void InitAnim()
    {
        activeTransform.localScale = defaultScale;
        animValue = 0.0f;
    }

    public void ScrollMap()
    {
        scrollTimer = Time.time + scrollTime;
    }

    public void SetEnemyVisible(bool visible)
    {
        enemy.gameObject.SetActive(visible);
        enemy.GetComponent<MeshRenderer>().material = enemyMaterial;
    }

    public void SetMerchantVisible(bool visible)
    {
        merchant.gameObject.SetActive(visible);
    }

    public void KillEnemy()
    {
        enemy.GetComponent<MeshRenderer>().material = deathMaterial;
    }

    public void KillPlayer()
    {
        player.GetComponent<MeshRenderer>().material = deathMaterial;
    }
}
