using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;


public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject MainMenuPanel;
    public GameObject GamePlayPanel;
    public GameObject PauseMenuPanel;
    public GameObject SettingsMenuPanel;
    public GameObject GameOverMenuPanel;

    [Header("Sound")]
    public AudioMixer mixer;
    public Slider MusicSlider;
    public Slider SFXSlider;

    [Header("Language")]
    public Button turkish;
    public Button english;

    void Start()
    {
        turkish.onClick.AddListener(() => SetLanguage("tr"));
        english.onClick.AddListener(() => SetLanguage("en"));
    }

    void Update()
    {
        MusicSlider.onValueChanged.AddListener(value => mixer.SetFloat("BGMusicVolume", Mathf.Log10(value) * 20));
        SFXSlider.onValueChanged.AddListener(value => mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20));
    }
    void SetLanguage(string localeCode)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);

    }
    public void GameStart()
    {
        GamePlayPanel.SetActive(true);
        MainMenuPanel.SetActive(false);

    }
    public void GamePause() => PauseMenuPanel.SetActive(!PauseMenuPanel.activeSelf);
    public void Settings() => SettingsMenuPanel.SetActive(!SettingsMenuPanel.activeSelf);
    public void GameOver() => GameOverMenuPanel.SetActive(!GameOverMenuPanel.activeSelf);
    public void Exit() => Application.Quit();
    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    
}
