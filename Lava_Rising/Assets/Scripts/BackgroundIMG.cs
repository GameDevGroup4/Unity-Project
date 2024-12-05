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
        float scrHeight = Screen.height;
        float scrWidth = Screen.width;
        
        float deviceScreenRatio = scrWidth / scrHeight;
        
        mainCam.aspect = deviceScreenRatio;
        
        //Calculate the Cam's viewable height
        float camHeight = 100.0f * mainCam.orthographicSize * 2.0f;
        float camWidth = camHeight * deviceScreenRatio;
        
        //Get the background img's height and width from the sprite
        SpriteRenderer backgroundImg = backgroundIMG.GetComponent<SpriteRenderer>();
        float bgImgHeight = backgroundImg.sprite.rect.height;
        float bgImgWidth = backgroundImg.sprite.rect.width;
        
        //calculation
        float scaleRatioHeight = camHeight / bgImgHeight;
        float scaleRatioWidth = camWidth / bgImgWidth;
        
        backgroundIMG.transform.localScale = new Vector3(scaleRatioWidth, scaleRatioHeight, 1);
        
    }
}
