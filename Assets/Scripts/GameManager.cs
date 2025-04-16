using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int collectedCoins = 0;
    public int totalCoins = 8;
    private PlayerScript playerScript;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    public void CoinCollected()
    {
        collectedCoins++;
        if (collectedCoins >= totalCoins)
        {
            playerScript.StartFlashingAndRestart();
        }
    }
}
