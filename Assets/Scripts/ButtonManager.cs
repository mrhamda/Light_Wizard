using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    PlayerMovoment player;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject helpMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] Slider songEffectSlider;
    [SerializeField] AudioSource audioSourceMusicInMenu;
    [SerializeField] GameObject aboutMeMenu;
    [SerializeField] Slider soundEffectSlider;
    float valueSliderSong;
    float valueSliderSoundEffect;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovoment>();
        //menu.SetActive(true);s
        //helpMenu.SetActive(false);
        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffect");
        songEffectSlider.value = PlayerPrefs.GetFloat("Song");

    }
    private void Update()
    {
        //print(PlayerPrefs.GetFloat("SoundEffect"));
        valueSliderSoundEffect = soundEffectSlider.value;
        PlayerPrefs.SetFloat("SoundEffect", valueSliderSoundEffect);

        valueSliderSong = songEffectSlider.value;
        PlayerPrefs.SetFloat("Song", valueSliderSong);
        audioSourceMusicInMenu.volume = valueSliderSoundEffect;

    }
    public void Resume()
    {
        player.pauseMenuAnim.SetBool("Dissolove", true);
        player.pauseMenu.SetActive(false);
        player.canMove = true;
    }
    public void AboutMe()
    {
        aboutMeMenu.SetActive(true);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(false);
        helpMenu.SetActive(false);
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void Home()
    {
        SceneManager.LoadScene("Home");
    }
    
   public void HelpButton()
    {
        helpMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void BackButton()
    {
        helpMenu.SetActive(false);
        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        aboutMeMenu.SetActive(false);
    }
    public void SettingsButton()
    {
        helpMenu.SetActive(false);
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
}
