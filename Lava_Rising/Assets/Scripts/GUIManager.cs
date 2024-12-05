using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

public class GUIManager : MonoBehaviour
{
    private Label time;
    
    private Slider altimeter;
    private Image sliderFill;
    private Transform player;
    
    private LevelManager levelManager;

    private float seconds;
    private float minutes;
    private bool closeToWin;
    private bool blink;
    
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        time = root.Q<Label>("Time");

        levelManager = GameObject.Find("Level Manager").GetComponent<LevelManager>();
        altimeter = GameObject.Find("Canvas").GetComponentInChildren<Slider>();
        sliderFill = GameObject.Find("Fill").GetComponentInChildren<Image>();
        altimeter.maxValue = levelManager.getTarget();
        
        player = GameObject.Find("Player").GetComponent<Transform>();
    }
    
    void Update()
    {
        if (player.position.y < altimeter.minValue)
        {
            altimeter.minValue = player.position.y;
        }
        altimeter.value = player.position.y;

        if (SceneManager.GetActiveScene().buildIndex == 5 && altimeter.value >= altimeter.maxValue * 0.75f && !closeToWin)
        {
            closeToWin = true;
            InvokeRepeating("blinkSlider", 0f, 0.5f);
        }
        
        if (Time.frameCount % 10 == 0)
        {
            minutes = Mathf.Floor(levelManager.time / 60f);
            seconds = levelManager.time % 60;

            time.text = minutes.ToString("0") + ":" + seconds.ToString("00.000");
        }
    }

    private void blinkSlider()
    {
        if (blink)
        {
            sliderFill.color = new Color(0.9411765f, 0.6196079f, 0.227451f, 1f);
        }
        else
        {
            sliderFill.color = Color.cyan;
        }
        
        blink = !blink;
    }
}
