using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class TransformUI : MonoBehaviour
{
    private PlayerController player;
    public GameObject playerUI;
    public Image selectedTransformation;
    public Image normalImage;
    public Image quickImage;
    public Image jumpyImage;
    public Image stickyImage;

    public ParticleSystem normalTransformParticles;
    public ParticleSystem quickTransformParticles;
    public ParticleSystem jumpyTransformParticles;
    public ParticleSystem stickyTransformParticles;

    private int _selected;
    private int _firstSelected;
    private bool _chosen;
    private float _currentCooldown;
    private bool _onCooldown;

    public float maxCooldown;
    public float lowAlpha;
    public float nonSelectedAlpha;

    [SerializeField] public float TIME;
    
    public bool started = false;
    public bool didSetup;

    private float ha;
    private float va;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        selectedTransformation.sprite = normalImage.sprite;
        playerUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!PauseMenu0.paused && !_onCooldown && (player.quickUnlocked || player.jumpyUnlocked || player.stickyUnlocked))
        {
            CheckInput();
        }

        if (_onCooldown)
        {
            _currentCooldown -= Time.deltaTime;
        }
        if (_currentCooldown <= 0f)
        {
            _onCooldown = false;
            _currentCooldown = maxCooldown;
        }
    }

    private void CheckInput()
    {
        ha = CrossPlatformInputManager.GetAxis(Constants.T_HORIZONTAL);
        va = CrossPlatformInputManager.GetAxis(Constants.T_VERTICAL);

        // Setup
        //Input.GetKeyDown(KeyCode.LeftShift)
        if (ha != 0 || va != 0 || Input.GetKeyDown(KeyCode.LeftShift))
        {
            _firstSelected = SetFirstSelected();
            CheckActivated(lowAlpha, nonSelectedAlpha);
            playerUI.SetActive(true);
            didSetup = true;
        }

        // Loop
        //Input.GetKey(KeyCode.LeftShift)
        if ((ha != 0 || va != 0 || Input.GetKey(KeyCode.LeftShift)) && didSetup)
        {
            SlowTime();
            KeepFirstSelectedAlphaLow(lowAlpha);
            CheckUnlocked();
            player.canMove = false;
            ManageUI();
        }

        // Exit
        //Input.GetKeyUp(KeyCode.LeftShift)
        if (((ha == 0 && va == 0) || Input.GetKeyUp(KeyCode.LeftShift)) && _selected >= 0 && _selected <= 3 && didSetup)
        {
            if(_selected != _firstSelected)
            {
                player.ExitTransformation(_selected);
                SelectedEffects(_selected);
                _onCooldown = true;
            }
            playerUI.SetActive(false);
            player.canMove = true;
            Time.timeScale = 1f;
            TIME = 1f;
            started = false;
            didSetup = false;
        }
    }

    private void SelectedEffects(int id)
    {
        switch (id)
        {
            case 0:
                Instantiate(normalTransformParticles, player.transform.position, player.transform.rotation);
                selectedTransformation.sprite = normalImage.sprite;
                break;
            case 1:
                Instantiate(quickTransformParticles, player.transform.position, player.transform.rotation);
                selectedTransformation.sprite = quickImage.sprite;
                break;
            case 2:
                Instantiate(jumpyTransformParticles, player.transform.position, player.transform.rotation);
                selectedTransformation.sprite = jumpyImage.sprite;
                break;
            case 3:
                Instantiate(stickyTransformParticles, player.transform.position, player.transform.rotation);
                selectedTransformation.sprite = stickyImage.sprite;
                break;
        }
    }

    private void KeepFirstSelectedAlphaLow(float alphaLow)
    {
        switch (_firstSelected)
        {
            case 1:
                SetAlphaImage(quickImage, alphaLow);
                break;
            case 0:
                SetAlphaImage(normalImage, alphaLow);
                break;
            case 2:
                SetAlphaImage(jumpyImage, alphaLow);
                break;
            case 3:
                SetAlphaImage(stickyImage, alphaLow);
                break;
        }
    }

    private void SlowTime()
    {
         if (!started)
         {
            Time.timeScale = 0.001f;
            TIME = 0.001f;
         }
         started = true;
         if (TIME < 1f)
         {
            Time.timeScale += 0.001f;
            TIME += 0.001f;
         }
    }
    //0.7843 -> 200a

    private void CheckActivated(float alphaLow, float alphaNotSelected)
    {
        switch (_firstSelected)
        {
            case 1:
                SetAlphaImage(quickImage, alphaLow);
                SetAlphaImage(normalImage, alphaNotSelected);
                SetAlphaImage(jumpyImage, alphaNotSelected);
                SetAlphaImage(stickyImage, alphaNotSelected);
                break;
            case 0:
                SetAlphaImage(normalImage, alphaLow);
                SetAlphaImage(quickImage, alphaNotSelected);
                SetAlphaImage(jumpyImage, alphaNotSelected);
                SetAlphaImage(stickyImage, alphaNotSelected);
                break;
            case 2:
                SetAlphaImage(jumpyImage, alphaLow);
                SetAlphaImage(quickImage, alphaNotSelected);
                SetAlphaImage(normalImage, alphaNotSelected);
                SetAlphaImage(stickyImage, alphaNotSelected);
                break;
            case 3:
                SetAlphaImage(jumpyImage, alphaNotSelected);
                SetAlphaImage(quickImage, alphaNotSelected);
                SetAlphaImage(normalImage, alphaNotSelected);
                SetAlphaImage(stickyImage, alphaLow);
                break;
        }

        CheckUnlocked();
    }

    private void CheckUnlocked()
    {
        if (!player.quickUnlocked)
        {
            SetAlphaImage(quickImage, 0f);
        }
        if (!player.jumpyUnlocked)
        {
            SetAlphaImage(jumpyImage, 0f);
        }
        if (!player.stickyUnlocked)
        {
            SetAlphaImage(stickyImage, 0f);
        }
    }

    private int SetFirstSelected()
    {
        switch (player.STransformation)
        {
            case "NormalTransform":
                return 0;
            case "QuickTransform":
                return 1;
            case "JumpyTransform":
                return 2;
            case "StickyTransform":
                return 3;
            default:
                return -1;
        }
    }

    private void SetAlphaImage(Image image, float alpha)
    {
        var tempColorQ = image.color;
        tempColorQ.a = alpha;
        image.color = tempColorQ;
    }

    private void ManageUI()
    {
        //Input.GetKeyDown(KeyCode.LeftArrow)
        //Quick
        if ((ha < -0.75 || Input.GetKeyDown(KeyCode.LeftArrow)) && _firstSelected != 1 && player.quickUnlocked)
        {
            SetAlphaImage(quickImage, 1f);
            SetAlphaImage(normalImage, nonSelectedAlpha);
            if (player.jumpyUnlocked) SetAlphaImage(jumpyImage, nonSelectedAlpha);
            if (player.stickyUnlocked) SetAlphaImage(stickyImage, nonSelectedAlpha);
            KeepFirstSelectedAlphaLow(lowAlpha);
            _selected = 1;
            _chosen = true;
        }
        else
        //Normal
        if ((va > 0.75 || Input.GetKeyDown(KeyCode.UpArrow)) && _firstSelected != 0) //Input.GetKeyDown(KeyCode.UpArrow)
        {
            if (player.quickUnlocked) SetAlphaImage(quickImage, nonSelectedAlpha);
            SetAlphaImage(normalImage, 1f);
            if (player.jumpyUnlocked) SetAlphaImage(jumpyImage, nonSelectedAlpha);
            if (player.stickyUnlocked)  SetAlphaImage(stickyImage, nonSelectedAlpha);
            KeepFirstSelectedAlphaLow(lowAlpha);
            _selected = 0;
            _chosen = true;
        }
        else
        //Jumpy
        if ((ha > 0.75 || Input.GetKeyDown(KeyCode.RightArrow)) && _firstSelected != 2 && player.jumpyUnlocked) //Input.GetKeyDown(KeyCode.RightArrow)
        {
            if (player.quickUnlocked) SetAlphaImage(quickImage, nonSelectedAlpha);
            SetAlphaImage(normalImage, nonSelectedAlpha);
            SetAlphaImage(jumpyImage, 1f);
            if (player.stickyUnlocked) SetAlphaImage(stickyImage, nonSelectedAlpha);
            KeepFirstSelectedAlphaLow(lowAlpha);
            _selected = 2;
            _chosen = true;
        }
        else
        //Sticky
        if ((va < -0.75 || Input.GetKeyDown(KeyCode.DownArrow)) && _firstSelected != 3 && player.stickyUnlocked) //Input.GetKeyDown(KeyCode.DownArrow)
        {
            if (player.quickUnlocked) SetAlphaImage(quickImage, nonSelectedAlpha);
            SetAlphaImage(normalImage, nonSelectedAlpha);
            if (player.jumpyUnlocked) SetAlphaImage(jumpyImage, nonSelectedAlpha);
            SetAlphaImage(stickyImage, 1f);
            KeepFirstSelectedAlphaLow(lowAlpha);
            _selected = 3;
            _chosen = true;
        }
        else if (!_chosen) _selected = -1;

    }

}
