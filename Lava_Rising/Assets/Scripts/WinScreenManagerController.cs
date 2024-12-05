using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class WinScreenManagerController: MonoBehaviour
{
    private VisualElement _root;
    private Button _restartButton;
    
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _restartButton = _root.Q<Button>("restart");
        _restartButton.RegisterCallback<ClickEvent>(ev => LoadScreen());
    }
    
    private void LoadScreen()
    {
        if (LevelManager.instance.levelAS.isPlaying)
        {
            LevelManager.instance.levelAS.Stop();
        }
        Debug.Log("Loading screen");
        SceneManager.LoadScene("Scenes/Start");
    }
}