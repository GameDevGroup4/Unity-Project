using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private float currentElevation;
    private float maxElevation;

    public float time;
    
    // Start is called before the first frame update
    void Start()
    {
        currentElevation = 0;
        maxElevation = 5f;

        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        // currentElevation = ... get from player controller
        if (currentElevation >= maxElevation)
        {
            endGame(true);
        }
    }

    public void endGame(bool win)
    {
        if (win)
        {
            // Win screen
        }
        else
        {
            // Defeat animation/sound, lose screen
        }
        
        // Call endGame(false) in player controller when touching lava?
    }
}
