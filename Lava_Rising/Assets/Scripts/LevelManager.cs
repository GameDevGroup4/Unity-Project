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
    [SerializeField] private AudioClip secretLevelMusic;

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

    public void NextLevel(GameObject interactingObject = null)
    {
        StopMusic();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex == 3)
        {
            // Level 1: Transition to Level 2 when interacting with the door
            if (interactingObject != null && interactingObject.CompareTag("Door"))
            {
                LoadScene("Scenes/Level2");
            }
        }
        else if (currentSceneIndex == 4)
        {
            if (interactingObject != null)
            {
                if (interactingObject.CompareTag("Door"))
                {
                    // Level 2: Transition to Level 3
                    LoadScene("Scenes/Level3");
                }
            }
        }
        else if (currentSceneIndex == 5)
        {
            // Level 3: Trigger win condition when interacting with the gate
            if (interactingObject != null && interactingObject.CompareTag("Gate"))
            {
                endGame(true);
            }
        }
        else
        {
            Debug.LogWarning("NextLevel logic not defined for this scene.");
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
    void Start()
    {
        levelAS = GetComponent<AudioSource>();
        
        if (SceneManager.GetActiveScene().buildIndex == 6) // Secret Level
        {
            levelAS.Stop();
            levelAS.clip = secretLevelMusic;
            levelAS.loop = true;
            levelAS.Play();
        }
        time = 0;
    }
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
                player.backgroundMusicSource.Stop();
            }
        }
        levelAS.Stop();
        levelAS.clip = hopefulClip;
        levelAS.volume = 0.5f;
        levelAS.Play();
    }
    
    public void StopMusic()
    {
        if (levelAS.isPlaying)
        {
            levelAS.Stop();
        }
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
