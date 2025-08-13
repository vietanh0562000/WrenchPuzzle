using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DxCoder;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI Music;

    public string RateURL, MoreURL;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        int CurrentLevel = PlayerPrefs.GetInt("Level", 0);
        LevelText.text = " LEVEL " + (CurrentLevel);
        SoundManager.Instance.PlayMusic(SoundManager.Instance.Menu);

    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void RateUS()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

        Application.OpenURL(RateURL);
    }
   
    public void MoreGames()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

        Application.OpenURL(MoreURL);

    }

    public void startLevel()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel() {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Level");

    }
    public void MuteSound()
    {
        if (SoundManager.Instance.IsMuted())
        {
            Music.text = "MUSIC ON";
        }
        else
        {
            Music.text = "MUSIC OFF";



        }
        SoundManager.Instance.ToggleMute();

    }
}
