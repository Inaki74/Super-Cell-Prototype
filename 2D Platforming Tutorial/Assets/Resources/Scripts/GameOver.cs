using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public string levelSelect;
    public string mainMenu;

    public void Restart()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("Lives", 3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LevelSelect()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("Lives", 3);
        SceneManager.LoadScene(levelSelect);
    }

    public void Quit()
    {
        SceneManager.LoadScene(mainMenu);
    }
}