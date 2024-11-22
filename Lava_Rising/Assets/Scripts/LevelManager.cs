using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private AudioSource levelAS;

    [SerializeField] private AudioClip hopefulClip;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip loseClip;

    private float targetElevation = 10f;
    
    public float time;
    
    // Start is called before the first frame update
    void Start()
    {
        levelAS = GetComponent<AudioSource>();
        time = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    public float getTarget()
    {
        return targetElevation;
    }

    public void intensifyMusic()
    {
        levelAS.Stop();
        levelAS.clip = hopefulClip;
        levelAS.Play();
    }

    public void endGame(bool win)
    {
        if (win)
        {
            levelAS.Stop();
            levelAS.clip = winClip;
            levelAS.Play();
            SceneManager.LoadScene("Win");
        }
        else
        {
            levelAS.Stop();
            levelAS.clip = loseClip;
            levelAS.Play();
            SceneManager.LoadScene("Lose");
        }
    }
}
