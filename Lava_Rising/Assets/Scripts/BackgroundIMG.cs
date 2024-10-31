using UnityEngine;

public class BackgroundIMG : MonoBehaviour
{
    public Camera mainCam;

    public GameObject backgroundIMG;
    // Start is called before the first frame update
    void Start()
    {
        Scale_Bg_To_Screen();
    }

    private void Scale_Bg_To_Screen()
    {
        print("Device Screen Size (W/H): " + Screen.width + "x" + Screen.height.ToString());
        
        float scrHeight = Screen.height;
        float scrWidth = Screen.width;
        float deviceScreenRatio = scrWidth / scrHeight;
        
        print("Device Screen Ratio: " + deviceScreenRatio);
        
        mainCam.aspect = deviceScreenRatio;

        float camHeight = 100.0f * mainCam.orthographicSize * 2.0f;
        float camWidth = camHeight * deviceScreenRatio;
        print("Camera Height: " + camHeight);
        print("Camera Width: " + camWidth);
        
        SpriteRenderer backgroundImg = backgroundIMG.GetComponent<SpriteRenderer>();
        float bgImgHeight = backgroundImg.sprite.rect.height;
        float bgImgWidth = backgroundImg.sprite.rect.width;
        
        print("BGImage Height" + bgImgHeight);
        print("BGImage Width" + bgImgWidth);
        
        float scaleRatioHeight = camHeight / bgImgHeight;
        float scaleRatioWidth = camWidth / bgImgWidth;
        
        backgroundIMG.transform.localScale = new Vector3(scaleRatioWidth, scaleRatioHeight, 1);
        
    }
}
