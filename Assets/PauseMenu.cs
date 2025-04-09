using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameisPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadGunScene()
    {
        Resume();
        SceneManager.LoadScene("TonyGunScene");
    }

    public void LoadAmariScene()
    {
        Resume();
        SceneManager.LoadScene("AmariTestingScene");
    }

    public void LoadAndyScene()
    {
        Resume();
        SceneManager.LoadScene("AndyScene");
    }

    public void LoadTutorialScene()
    {
        Resume();
        SceneManager.LoadScene("MainScene");
    }

    public void RestartGame()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}
