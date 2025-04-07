using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainRestart : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}

