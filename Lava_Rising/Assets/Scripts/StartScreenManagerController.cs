using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartScreenManagerController : MonoBehaviour
{
    private VisualElement root;
    private Button startButton;
    private Button muteButton;
    private Button avaButton;
    private Button harryButton;

    private Label avaText;
    private Label harryText;
    private Label startText;
    
    private AudioSource audioSource;
    
    private Camera mainCamera;

    private bool mute;

    public static string selectedMusic;
    
    

    [SerializeField] private AudioClip[] selectMusic;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        selectedMusic = "";
        mute = false;
        root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("start");
        muteButton = root.Q<Button>("mute");
        avaButton = root.Q<Button>("Ava");
        harryButton = root.Q<Button>("Harry");
        avaText = root.Q<Label>("AvaText");
        harryText = root.Q<Label>("HarryText");
        startText = root.Q<Label>("StartText");
        
        mainCamera = Camera.main;
        audioSource = mainCamera.GetComponent<AudioSource>();
        avaButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(0.5f, 1.0f, 0.5f));
        avaText.style.color = new StyleColor(new Color(0.5f, 1.0f, 0.5f));
        harryButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(1.0f, 0.5f, 0.5f));
        harryText.style.color = new StyleColor(new Color(1.0f, 0.5f, 0.5f));
        
        
        startButton.RegisterCallback<ClickEvent>(ev => LoadScreen());
        muteButton.RegisterCallback<ClickEvent>(ev => muteSound());
        avaButton.RegisterCallback<ClickEvent>(ev => SelectAvaMusic());
        harryButton.RegisterCallback<ClickEvent>(ev => SelectHarryMusic());
        
        if (PlayerPrefs.HasKey("selectedMusic"))
        {
            selectedMusic = PlayerPrefs.GetString("selectedMusic");
            if (selectedMusic == "Harry")
            {
                audioSource.clip = selectMusic[1];
            }
            else
            {
                selectedMusic = "Ava";
                audioSource.clip = selectMusic[0];
            }
        }
        else
        {
            selectedMusic = "Ava";
            audioSource.clip = selectMusic[0];
        }

        audioSource.Play();

        InvokeRepeating("blinkStartButton", 1.0f, 0.5f);
    }
    
    
    private void LoadScreen()
    {
        audioSource?.Stop();
        SceneManager.LoadScene("Scenes/Main");
    }
    
    private void blinkStartButton()
    {
        startText.visible = !startText.visible;
    }

    private void muteSound()
    {
        var muteText = root.Q<Label>("Mute");
        mute = !mute;
        if (mute)
        {
            audioSource?.Pause();
            muteButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(0.5f, 1.0f, 0.5f));
            muteText.style.color = new StyleColor(new Color(0.5f, 1.0f, 0.5f));
        }
        else
        {
            audioSource?.Play();
            muteButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(1.0f, 0.5f, 0.5f));
            muteText.style.color = new StyleColor(new Color(1.0f, 0.5f, 0.5f));
        }
    }

    private void SelectAvaMusic()
    {
        selectedMusic = "Ava";
        PlayerPrefs.SetString("selectedMusic", selectedMusic);
        audioSource.clip = selectMusic[0];
        audioSource.Play();
        UpdateButtonColors();
        LevelManager.chooseMusic = selectedMusic;
    }

    private void SelectHarryMusic()
    {
        selectedMusic = "Harry";
        PlayerPrefs.SetString("selectedMusic", selectedMusic);
        audioSource.clip = selectMusic[1];
        audioSource.volume = 0.4f;
        audioSource.Play();
        UpdateButtonColors();
        LevelManager.chooseMusic = selectedMusic;
    }

    private void UpdateButtonColors()
    {
        // Access the text elements
        var avaText = root.Q<Label>("AvaText");
        var harryText = root.Q<Label>("HarryText");
        

        if (selectedMusic == "Ava")
        {
            //Ava button
            avaButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(0.5f, 1.0f, 0.5f));
            avaText.style.color = new StyleColor(new Color(0.5f, 1.0f, 0.5f));

            //Harry button
            harryButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(1.0f, 0.5f, 0.5f));
            harryText.style.color = new StyleColor(new Color(1.0f, 0.5f, 0.5f));
        }
        else if (selectedMusic == "Harry")
        {
            //Ava button
            avaButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(1.0f, 0.5f, 0.5f));
            avaText.style.color = new StyleColor(new Color(1.0f, 0.5f, 0.5f));

            //Harry button
            harryButton.style.unityBackgroundImageTintColor = new StyleColor(new Color(0.5f, 1.0f, 0.5f));
            harryText.style.color = new StyleColor(new Color(0.5f, 1.0f, 0.5f));
        }
    }
}