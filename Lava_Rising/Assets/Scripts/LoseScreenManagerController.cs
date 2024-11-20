using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoseScreenManagerController: MonoBehaviour
{
    private VisualElement _root;
    private Button _restartButton;
    
    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _restartButton = _root.Q<Button>("restart");
        _restartButton.RegisterCallback<ClickEvent>(ev => LoadScreen());
    }
    
    private void LoadScreen()
    {
        Debug.Log("Loading screen");
        SceneManager.LoadScene("Scenes/Start");
    }
}