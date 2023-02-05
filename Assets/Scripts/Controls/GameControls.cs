using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControls : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            Time.timeScale = 5.0f;
        }
        else if(!Mathf.Approximately(Time.timeScale, 1.0f))
        {
            Time.timeScale = 1.0f;
        }
    }
}
