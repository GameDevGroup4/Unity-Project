using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class GUIManager : MonoBehaviour
{
    private GroupBox timerGB;
    private Label time;
    
    private Slider altimeter;
    private Transform player;
    
    private LevelManager levelManager;

    private float seconds;
    private float minutes;
    
    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        timerGB = root.Q<GroupBox>("TimerGB");
        time = root.Q<Label>("Time");

        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        altimeter = GameObject.Find("Canvas").GetComponentInChildren<Slider>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        altimeter.value = player.position.y;
        
        if (Time.frameCount % 10 == 0)
        {
            minutes = Mathf.Floor(levelManager.time / 60f);
            seconds = levelManager.time % 60;

            time.text = minutes.ToString("0") + ":" + seconds.ToString("00.000");
        }
    }
}
