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
    private AudioSource startMusic;
    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        startButton = root.Q<Button>("start");
        muteButton = root.Q<Button>("mute");
        
        mainCamera = Camera.main;
        startMusic = mainCamera.GetComponent<AudioSource>();
        startMusic.Play();
        
        InvokeRepeating("blinkStartButton", 1.0f, 0.5f);
        startButton.RegisterCallback<ClickEvent>(ev => LoadScreen());
        muteButton.RegisterCallback<ClickEvent>(ev => muteSound());
    }
    
    void LoadScreen()
    {
        startMusic.Stop();
        SceneManager.LoadScene("Scenes/Main");
    }
    
    void blinkStartButton()
    {
        startButton.visible = !startButton.visible;
    }

    void muteSound()
    {
        if (startMusic.isPlaying)
        { 
            startMusic.Pause();
        } else if (!startMusic.isPlaying)
        {
            startMusic.Play();
        }
    }
}