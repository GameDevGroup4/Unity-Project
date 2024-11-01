using UnityEngine;

public class BackgroundIMG : MonoBehaviour
{
    public Camera mainCam;

    public GameObject backgroundIMG;
    
    void Start()
    {
        Scale_Bg_To_Screen();
    }

    //Scale background to match the screen size
    private void Scale_Bg_To_Screen()
    {
        //Print the screen dimension
        print("Device Screen Size (W/H): " + Screen.width + "x" + Screen.height);
        
        //Device height and width
        float scrHeight = Screen.height;
        float scrWidth = Screen.width;
        
        //Calculate the ratio of screen
        float deviceScreenRatio = scrWidth / scrHeight;
        print("Device Screen Ratio: " + deviceScreenRatio);
        
        mainCam.aspect = deviceScreenRatio;
        
        //Calculate the Cam's viewable height
        float camHeight = 100.0f * mainCam.orthographicSize * 2.0f;
        float camWidth = camHeight * deviceScreenRatio;
        print("Camera Height: " + camHeight);
        print("Camera Width: " + camWidth);
        
        //Get the background img's height and width from the sprite
        SpriteRenderer backgroundImg = backgroundIMG.GetComponent<SpriteRenderer>();
        float bgImgHeight = backgroundImg.sprite.rect.height;
        float bgImgWidth = backgroundImg.sprite.rect.width;
        
        print("BGImage Height" + bgImgHeight);
        print("BGImage Width" + bgImgWidth);
        
        //calculation
        float scaleRatioHeight = camHeight / bgImgHeight;
        float scaleRatioWidth = camWidth / bgImgWidth;
        
        backgroundIMG.transform.localScale = new Vector3(scaleRatioWidth, scaleRatioHeight, 1);
        
    }
}
