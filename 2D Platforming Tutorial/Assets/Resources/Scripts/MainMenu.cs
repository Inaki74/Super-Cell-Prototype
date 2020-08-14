using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public string levelSelect;
    public int startingLives;
    public string[] lockedLevels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(firstLevel);

        foreach(var level in lockedLevels)
        {
            PlayerPrefs.SetFloat(level, 0);

            PlayerPrefs.SetInt("Coins", 0);
            PlayerPrefs.SetInt("Lives", startingLives);
        }
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
