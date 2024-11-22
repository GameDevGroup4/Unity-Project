using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public AudioSource levelAS;

    [SerializeField] private AudioClip hopefulClip;
    [SerializeField] private AudioClip winClipAva;
    [SerializeField] private AudioClip loseClipAva;
    [SerializeField] private AudioClip winClipHarry;
    [SerializeField] private AudioClip loseClipHarry;

    private float targetElevation = 10f;
    
    public static LevelManager instance;
    
    public float time;

    public static string chooseMusic = StartScreenManagerController.selectedMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        levelAS = GetComponent<AudioSource>();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    public float getTarget()
    {
        return targetElevation;
    }

    public void intensifyMusic()
    {
        if (chooseMusic == "Harry")
        {
            Player player = FindObjectOfType<Player>();
            if (player != null && player.backgroundMusicSource.isPlaying)
            {
                player.backgroundMusicSource.Stop(); // Stop Harry's BGM
            }
        }
    
        // Play the hopeful music regardless of the current background music
        levelAS.Stop();
        levelAS.clip = hopefulClip;
        levelAS.volume = 0.5f;
        levelAS.Play();
    }

    public void endGame(bool win)
    {
        levelAS.Stop();
        if (win)
        {
            if (chooseMusic == "Ava")
            {
                levelAS.clip = winClipAva;
            }
            else
            {
                levelAS.clip = winClipHarry;
            }
            levelAS.Play();
            SceneManager.LoadScene("Win");
        }
        else
        {
            if (chooseMusic == "Harry")
            {
                levelAS.clip = loseClipHarry;
            }
            else
            {
                levelAS.clip = loseClipAva;
            }
            levelAS.Play();
            SceneManager.LoadScene("Lose");
        }
    }
}
