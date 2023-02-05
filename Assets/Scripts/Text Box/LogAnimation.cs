using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogAnimation : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 localHoverPos;
    Vector3 normalPos;
    bool droppingDown = false;

    void Start()
    {
        normalPos = transform.localPosition;
    }

    void Update()
    {
        if (Input.mousePosition.y > Screen.height * 0.8f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, localHoverPos, Time.deltaTime * speed);

            if(!droppingDown)
            {
                droppingDown = true;
                AudioManager.Instance.PlaySound("SkillDescriptionVariation1");
            }
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, normalPos, Time.deltaTime * speed);

            if (droppingDown)
            {
                droppingDown = false;
                AudioManager.Instance.PlaySound("SkillDescriptionClose");
            }
        }
    }
}
