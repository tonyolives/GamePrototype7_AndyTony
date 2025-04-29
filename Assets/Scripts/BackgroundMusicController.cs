using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicController : MonoBehaviour
{

    public bool isPaused = false;
    public bool isPlaying = false;
    public AudioClip mainMenuMusic;
    public AudioClip levelMusic;
    public AudioSource _audio;
    public AudioLowPassFilter _filter;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            _filter.enabled = true;
        }
        else
        {
            _filter.enabled = false;
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case "Main Menu":
                if (!isPlaying)
                {
                    _audio.clip = mainMenuMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                else if (_audio.clip != mainMenuMusic)
                {
                    _audio.Stop();
                    _audio.clip = mainMenuMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                break;

            case "Level 1":
                if (!isPlaying)
                {
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                else if (_audio.clip != levelMusic)
                {
                    _audio.Stop();
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                break;

            case "Level 2":
                if (!isPlaying)
                {
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                else if (_audio.clip != levelMusic)
                {
                    _audio.Stop();
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                break;

            case "Level 3":
                if (!isPlaying)
                {
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                else if (_audio.clip != levelMusic)
                {
                    _audio.Stop();
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                break;

            case "Level 4":
                if (!isPlaying)
                {
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                else if (_audio.clip != levelMusic)
                {
                    _audio.Stop();
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                break;
            case "Tutorial1":
                if (!isPlaying)
                {
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                else if (_audio.clip != levelMusic)
                {
                    _audio.Stop();
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                break;
            case "Tutorial2":
                if (!isPlaying)
                {
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                else if (_audio.clip != levelMusic)
                {
                    _audio.Stop();
                    _audio.clip = levelMusic;
                    _audio.Play();
                    isPlaying = true;
                }
                break;
            default:
                break;
        }
    }

    
}
