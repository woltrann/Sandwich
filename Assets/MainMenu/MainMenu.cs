using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Unity.Mathematics;


public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainMenuPanel;
    public GameObject GamePlayPanel;
    public GameObject PauseMenuPanel;
    public GameObject SettingsMenuPanel;
    public GameObject GameOverMenuPanel;
    public GameObject WinMenuPanel;
    public GameObject BlackScreen;
    public GameObject LoadingScreen;

    [Header("Sound")]
    public AudioMixer mixer;
    public Slider MusicSlider;
    public Slider SFXSlider;

    [Header("Language")]
    public Button turkish;
    public Button english;

    [Header("Spawn References")]
    public Transform player;
    public Transform playerSpawnPoint;
    public Transform enemy;
    public Transform enemySpawnPoint;
    public PlayerHealth playerHealth;

    public int dieCount = 0;
    public PlayerHealth health;

    void Start()
    {
        dieCount = 0;
        turkish.onClick.AddListener(() => SetLanguage("tr"));
        english.onClick.AddListener(() => SetLanguage("en"));
        Time.timeScale = 1;
    }

    void Update()
    {
        MusicSlider.onValueChanged.AddListener(value => mixer.SetFloat("BGMusicVolume", Mathf.Log10(value) * 20));
        SFXSlider.onValueChanged.AddListener(value => mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20));
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GamePause();  
        }
    }
    void SetLanguage(string localeCode)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);

    }
    public void GameStart()
    {
        GamePlayPanel.SetActive(true);
        GameOverMenuPanel.SetActive(false); 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        health.isDead = false;
        // === PLAYER RESET ===
        player.position = playerSpawnPoint.position;
        player.rotation = playerSpawnPoint.rotation;
        enemy.position = enemySpawnPoint.position;
        enemy.rotation = enemySpawnPoint.rotation;

        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.healthSlider.value = playerHealth.maxHealth;
    }
    public void Main()
    {
        GamePlayPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        PauseMenuPanel.SetActive(false);
        GameOverMenuPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void GamePause()
    {
        if(!GamePlayPanel.activeSelf) return;
        PauseMenuPanel.SetActive(!PauseMenuPanel.activeSelf);

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }
    }
    public void Settings() => SettingsMenuPanel.SetActive(!SettingsMenuPanel.activeSelf);
    public void GameOver()
    {
        dieCount++;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        if (dieCount >= 4)
        {
            Debug.Log("Animasyon Baþlar ve Oyuna Girer");
            Time.timeScale = 1;

        }
        else
        {
            GameOverMenuPanel.SetActive(!GameOverMenuPanel.activeSelf);

        }
        
    }
    public void Win() => MainMenuPanel.SetActive(!MainMenuPanel.activeSelf);
    public void Exit()
    {
        if (BlackScreen.activeSelf)
        {
            BlackScreen.SetActive(false);
            LoadingScreen.SetActive(true);
        }
        else
        {
            Application.Quit();
        }

    }
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    
}
