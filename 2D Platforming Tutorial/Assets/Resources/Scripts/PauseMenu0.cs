using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

public class PauseMenu0 : MonoBehaviour
{
    public string levelSelect;
    public string mainMenu;
    public GameObject pauseMenu;
    public GameObject mobileControls;
    public List<Image> mobileControlsImages;

    public static bool paused = false;

    private LevelManager _levelManager;

    private void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();

        GetImageComponents();

        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || CrossPlatformInputManager.GetButtonDown(Constants.START))
        {
            if (paused)
            {
                ResumeGame();
                paused = false;
            }
            else
            {
                pauseMenu.SetActive(true);
                DisableMobileControls();
                Time.timeScale = 0f;
                paused = true;
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        EnableMobileControls();
        paused = false;
    }

    public void LevelSelect()
    {
        PlayerPrefs.SetInt("Coins", _levelManager.coinCount);
        PlayerPrefs.SetInt("Lives", _levelManager.lives);

        Time.timeScale = 1f;
        paused = false;

        SceneManager.LoadScene(levelSelect);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        paused = false;
        SceneManager.LoadScene(mainMenu);
    }

    private void EnableMobileControls()
    {
        foreach(Image i in mobileControlsImages)
        {
            i.enabled = true;
        }
    }

    private void DisableMobileControls()
    {
        foreach (Image i in mobileControlsImages)
        {
            i.enabled = false;
        }
    }

    private void GetImageComponents()
    {
        Transform[] mobileControlsTransforms = mobileControls.GetComponentsInChildren<Transform>();
        foreach (Transform t in mobileControlsTransforms)
        {
            Image x = t.gameObject.GetComponent<Image>();
            if (x != null)
            {
                mobileControlsImages.Add(x);
            }
        }
    }
}
