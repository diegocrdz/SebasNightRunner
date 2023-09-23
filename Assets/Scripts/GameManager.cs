using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("----------Float----------")]
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }
    private float score;

    [Header("----------Bool----------")]
    private bool isPaused;

    [Header("----------Menu----------")]
    public GameObject pauseMenu;
    public GameObject looseMenu;

    [Header("----------Score----------")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;

    [Header("----------GameObjects----------")]
    public GameObject player;
    public GameObject pauseButton;
    private Spawner spawner;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        spawner = FindObjectOfType<Spawner>();

        Time.timeScale = 1f;

        NewGame();
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles) {
            Destroy(obstacle.gameObject);
        }

        score = 0f;
        gameSpeed = initialGameSpeed;
        enabled = true;

        player.SetActive(true);
        spawner.gameObject.SetActive(true);

        pauseMenu.gameObject.SetActive(false);
        looseMenu.gameObject.SetActive(false);

        PlayAudio();
        UpdateHiscore();
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);

        isPaused = true;
        
        PauseAudio();
    }

    public void Continue()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        isPaused = false;

        PlayAudio();
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;
        Time.timeScale = 0f;

        player.SetActive(false);
        spawner.gameObject.SetActive(false);

        pauseButton.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        looseMenu.gameObject.SetActive(true);

        PauseAudio();
        UpdateHiscore();
    }

    private void Update()
    {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayerPrefs.SetFloat("hiscore", 0);
        }
    }

    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }

    public void PauseAudio()
    {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        foreach (AudioSource a in audios)
        {
            a.Pause();
        }
    }

    public void PlayAudio()
    {
        AudioSource[] audios = FindObjectsOfType<AudioSource>();

        foreach (AudioSource a in audios)
        {
            a.Play();
        }
    }
}
