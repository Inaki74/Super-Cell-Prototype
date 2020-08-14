using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public float respawnTime;
    public ParticleSystem deathParticles;
    public bool _isDead;
    public int coinCount;
    public Text coinText;
    public AudioSource coinSound;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Sprite heart_f;
    public Sprite heart_h;
    public Sprite heart_e;
    public int maxHealth;
    public int currentHealth;
    public Text livesText;
    public int lives;
    public int livesCounter;
    public GameObject gameOverMusic;
    public GameObject gameOverScreen;
    public GameObject gameMusic;
    public GameObject blackScreen;

    public Text healthChargesText;

    public GameObject redKeyHolder;
    public GameObject blueKeyHolder;
    public GameObject greenKeyHolder;
    public GameObject yellowKeyHolder;

    [SerializeField]
    private Text redKeyText;
    [SerializeField]
    private Text blueKeyText;
    [SerializeField]
    private Text greenKeyText;
    [SerializeField]
    private Text yellowKeyText;

    private int redKeyCount = 0;
    private int greenKeyCount = 0;
    private int yellowKeyCount = 0;
    private int blueKeyCount = 0;

    private ResetOnRespawn[] objectsToReset;
    private int coinCountToExtraLife;
    private PlayerController _player;

    public delegate void onLevelEnd();
    public event onLevelEnd onEnd;

    ///
    private void Update()
    {
        if (currentHealth <= 0 && !_isDead)
        {
            _isDead = true;
            Respawn();
        }

        if(coinCountToExtraLife >= 100)
        {
            addLives(1);
            coinCountToExtraLife = coinCount % 100;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        objectsToReset = FindObjectsOfType<ResetOnRespawn>();
        _player = FindObjectOfType<PlayerController>();

        SetPlayerPrefs();

        coinText.text = "Coins: " + coinCount;
        currentHealth = maxHealth;
        livesCounter = lives;
        livesText.text = "Lives x " + livesCounter;
        blackScreen.SetActive(true);

        redKeyHolder.SetActive(false);
        blueKeyHolder.SetActive(false);
        greenKeyHolder.SetActive(false);
        yellowKeyHolder.SetActive(false);
    }

    private void SetPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            coinCount = PlayerPrefs.GetInt("Coins");
        }

        if (PlayerPrefs.HasKey("Lives"))
        {
            livesCounter = PlayerPrefs.GetInt("Lives");
        }

    }

    public void Hurt(int damage, float damagerCenterX, float playerCenterX)
    {
        if (!_player.invincible)
        {
            currentHealth -= damage;
            _player.hurtSound.Play();
            if(currentHealth > 0)
            {
                _player.Knockback(damagerCenterX, playerCenterX);
            }
            
           
            updateHeartUI();
        }
    }

    public void updateHeartUI()
    {
        switch (currentHealth)
        {
            case 6:
                updateHearts(heart_f, heart_f, heart_f);
                break;
            case 5:
                updateHearts(heart_f, heart_f, heart_h);
                break;
            case 4:
                updateHearts(heart_f, heart_f, heart_e);
                break;
            case 3:
                updateHearts(heart_f, heart_h, heart_e);
                break;
            case 2:
                updateHearts(heart_f, heart_e, heart_e);
                break;
            case 1:
                updateHearts(heart_h, heart_e, heart_e);
                break;
            default:
                updateHearts(heart_e, heart_e, heart_e);
                break;
        }
    }

    public void updateHealthChargesUI()
    {
        healthChargesText.text = "x " + _player.healCharges;
    }

    public void updateHearts(Sprite place1, Sprite place2, Sprite place3)
    {
        heart1.sprite = place1;
        heart2.sprite = place2;
        heart3.sprite = place3;
    }

    public void Respawn()
    {
        if(_isDead)
        {
            livesCounter -= 1;
            livesText.text = "Lives x " + livesCounter;

            if (livesCounter > 0)
            {
                StartCoroutine(DeathRespawnWait(respawnTime));
            }
            else
            {
                _player.gameObject.SetActive(false);
                gameMusic.SetActive(false);
                gameOverMusic.SetActive(true);
                gameOverScreen.SetActive(true);
            }

            addCoins(-20);
        }
        else
        {
            StartCoroutine(FallenRespawnWait(respawnTime));
        }
        
    }

    private IEnumerator DeathRespawnWait(float t)
    {
        _player.gameObject.SetActive(false);

        Instantiate(deathParticles, _player.transform.position, _player.transform.rotation);

        yield return new WaitForSeconds(t);

        _player.MyRigidBody.velocity = Vector2.zero;
        _player.transform.position = _player.GetComponent<PlayerController>().checkpoint;
        _player.gameObject.SetActive(true);
        currentHealth = maxHealth;
        updateHeartUI();
        resetLevel();
        _isDead = false;
    }

    private IEnumerator FallenRespawnWait(float t)
    {
        _player.gameObject.SetActive(false);

        Instantiate(deathParticles, _player.transform.position, _player.transform.rotation);

        yield return new WaitForSeconds(t);

        _player.MyRigidBody.velocity = Vector2.zero;
        _player.transform.position = _player.GetComponent<PlayerController>().fallingCheckpoint;
        _player.gameObject.SetActive(true);
    }

    private void resetLevel()
    {
        foreach(var respawn in objectsToReset)
        {

            respawn.gameObject.SetActive(true);
            respawn.ResetObject();
        }
    }

    public void addCoins(int coinsToAdd)
    {
        if(coinsToAdd > 0)
        {
            coinSound.Play();
        }
        coinCount += coinsToAdd;
        coinCountToExtraLife += coinsToAdd;
        coinText.text = "Coins: " + coinCount;
    }

    public void addLives(int livesToAdd)
    {
        coinSound.Play();
        livesCounter += livesToAdd;
        livesText.text = "Lives x " + livesCounter; 
    }

    public void addHealth(int healthToAdd)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healthToAdd;
            updateHeartUI();
        }
    }

    public void addHealCharge(int chargesToAdd)
    {
        _player.healCharges += chargesToAdd;
        updateHealthChargesUI();
    }

    public void addRedKey(int amount)
    {
        if(amount > 0)
            coinSound.Play();

        redKeyCount += amount;

        if (redKeyCount > 0)
            redKeyHolder.SetActive(true);
        else redKeyHolder.SetActive(false);

        redKeyText.text = "" + redKeyCount;
    }
    public void addGreenKey(int amount)
    {
        if (amount > 0)
            coinSound.Play();

        greenKeyCount += amount;

        if (greenKeyCount > 0)
            greenKeyHolder.SetActive(true);
        else greenKeyHolder.SetActive(false);

        greenKeyText.text = "" + greenKeyCount;
    }
    public void addBlueKey(int amount)
    {
        if (amount > 0)
            coinSound.Play();

        blueKeyCount += amount;

        if (blueKeyCount > 0)
            blueKeyHolder.SetActive(true);
        else blueKeyHolder.SetActive(false);

        blueKeyText.text = "" + blueKeyCount;
    }
    public void addYellowKey(int amount)
    {
        if (amount > 0)
            coinSound.Play();

        yellowKeyCount += amount;

        if (yellowKeyCount > 0)
            yellowKeyHolder.SetActive(true);
        else yellowKeyHolder.SetActive(false);

        yellowKeyText.text = "" + yellowKeyCount;
    }

    public int getKeyCount(string color)
    {
        switch (color)
        {
            case "Red":
                return redKeyCount;
            case "Green":
                return greenKeyCount;
            case "Yellow":
                return yellowKeyCount;
            case "Blue":
                return blueKeyCount;
            default:
                return 0;
        }
    }

    public void endLevel(string levelToLoad, string levelToUnlock)
    {
        onEnd?.Invoke();

        gameMusic.SetActive(false);
        gameOverMusic.SetActive(true);
        PlayerPrefs.SetInt("Coins", coinCount);
        PlayerPrefs.SetInt("Lives", livesCounter);
        PlayerPrefs.SetInt(levelToUnlock, 1);
        StartCoroutine(LevelExitCo(levelToLoad));
    }

    private IEnumerator LevelExitCo(string levelToLoad)
    {
        yield return new WaitForSeconds(3.5f);

        blackScreen.GetComponentInChildren<Fade>().fadeIn = true;

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(levelToLoad);
    }
}
