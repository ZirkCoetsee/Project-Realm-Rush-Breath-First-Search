using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 0.5f;

    [Header("Confirmation Icon")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Level To Load")]
    public string _levelToLoad;
    // [SerializeField] private GameObject noSavedGameDialog = null;
    bool isDisplaying = false;
    bool isPaused = false;

    public void NewGame()
    {
        MusicPlayer musicplayer = FindObjectOfType<MusicPlayer>();
        Destroy(musicplayer);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    //If you wanted to load saved games
    // public void LoadGameDialogYes(){
    //     if(PlayerPrefs.HasKey("SavedLevel")){
    //         levelToLoad = PlayerPrefs.GetString("SavedLevel");
    //         SceneManager.LoadScent(levelToLoad);
    //     }
    //      else
    //     {
    //          noSavedGameDialog.SetActive(true);
    //     }
    // }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume",AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public IEnumerator ConfirmationBox(){

        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);

    }

    public void ResetButton(string MenuType)
    {
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
        }
        StartCoroutine(ConfirmationBox());
    }

    public void DisplayPause()
    {
        if(isDisplaying){
            isDisplaying = false;
            PauseGame();
        }else{
            isDisplaying = true;
            PauseGame();
        }
    }

    void PauseGame(){
        if(isPaused){
        Time.timeScale = 1;
        AudioListener.pause = false;
        isPaused = false;
        Debug.Log("Resume Game");

        }else{
        Time.timeScale = 0;
        AudioListener.pause = true;
        isPaused = true;
        Debug.Log("Pause Game");


        }
    }

    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
