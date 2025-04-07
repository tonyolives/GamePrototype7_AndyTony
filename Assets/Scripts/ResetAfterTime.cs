using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class ResetAfterTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float TimetoReset = 10.0f;
    void Start()
    {
        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator waiter()
    {
        //Wait for x seconds
        yield return new WaitForSeconds(TimetoReset);
        RestartGame();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
}
