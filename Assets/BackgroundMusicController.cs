using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicController : MonoBehaviour
{

    public bool isPaused = false;
    public bool isPlaying = true;
    public AudioClip mainMenuMusic;
    public AudioClip levelMusic;
    public AudioSource _audio;
    public AudioLowPassFilter _filter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (isPaused)
        {
            _filter.enabled = true;
        }
        else
        {
            _filter.enabled = false;
        }

        if (sceneName == "Main Menu" && isPlaying == false)
        {
            _audio.clip = levelMusic;
            _audio.Play();
            isPlaying = true;
        }
        else if (sceneName != "Main Menu" && isPlaying == false)
        {
            _audio.clip = mainMenuMusic;
            _audio.Play();
            isPlaying = true;
        }
    }

    private static BackgroundMusicController _instance;

    public static BackgroundMusicController Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
