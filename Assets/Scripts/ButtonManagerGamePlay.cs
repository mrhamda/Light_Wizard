using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ButtonManagerGamePlay : MonoBehaviour
{
    PlayerMovoment player;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] Slider soundEffectSlider;

    [SerializeField] Slider songSlider;
    [SerializeField] GameObject helpMenu;
    float volumeSong;
    float volumeSound;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovoment>();
        //menu.SetActive(true);
        //helpMenu.SetActive(false);
       
        soundEffectSlider.value = PlayerPrefs.GetFloat("SoundEffect");
        songSlider.value = PlayerPrefs.GetFloat("Song");

    }
    private void LateUpdate()
    {
        volumeSound = soundEffectSlider.value;
        PlayerPrefs.SetFloat("SoundEffect", volumeSound);

        volumeSong = songSlider.value;
        PlayerPrefs.SetFloat("Song", volumeSong);
    }
    public void Resume()
    {
        player.pauseMenuAnim.SetBool("Dissolove", true);
        player.pauseMenu.SetActive(false);
        player.canMove = true;
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
    public void SettingsMenu()
    {
        settingsMenu.SetActive(true);
        helpMenu.SetActive(false);

    }
    public void Back()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
        helpMenu.SetActive(false);
    }
    public void Help()
    {
        helpMenu.SetActive(true);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
