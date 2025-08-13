using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DxCoder;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    int TotalRing,RingLeft;
    public ParticleSystem WinExplosion1, WinExplosion2;
    public GameObject GameCompletefx, GameWinUI;
    bool GameWin;
    GameObject Level;
    public GameObject[] GameLevels;
    int CurrentLevel;
    public TextMeshProUGUI LevelIndicator;
    public GameObject ShowDialog;
    
    private ClickRotate[] wrenches;

    public GameObject Shop;
    // Start is called before the first frame update

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

    }

    void Start()
    {
        CurrentLevel = PlayerPrefs.GetInt("Level",0);
        LevelIndicator.text = "Level " + CurrentLevel;
       
        Instantiate(GameLevels[CurrentLevel]);
        GameCompletefx.SetActive(false);
        GameWin = false;
        RingLeft = 0;
        TotalRing = GameObject.FindGameObjectsWithTag("Wrench").Length;
        SoundManager.Instance.PlayMusic(SoundManager.Instance.Game);
        
        wrenches = Object.FindObjectsByType<ClickRotate>(FindObjectsSortMode.InstanceID);
    }

    // Update is called once per frame
    void Update()
    {
        if (RingLeft >= TotalRing) {
            if (!GameWin) {
                GameWin = true;
               StartCoroutine(LevelCompleted());
            }
        }
    }

    public void UpdateScore() {
        RingLeft++;
      //  RingCounter.text = RingLeft + "/" + TotalRing;
    }

    IEnumerator LevelCompleted() {
        SoundManager.Instance.StopMusic();
        SoundManager.Instance.PlaySound(SoundManager.Instance.GameWin);

        PlayerPrefs.SetInt("Level",(CurrentLevel+1));
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlaySound(SoundManager.Instance.go);
        GameDataManager.Instance.playerData.AddDiamond(5);
        
        GameCompletefx.SetActive(true);
        GameWinUI.SetActive(true);
    }


    public void ReloadLevel() {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

        SceneManager.LoadScene("Level");

    }

    public void LoadMenu() {
        SoundManager.Instance.PlaySound(SoundManager.Instance.button);

        SceneManager.LoadScene("Menu");
    }

    public void Vaccum()
    {
        if (GameDataManager.Instance.playerData.intDiamond >= 50)
        {
            GameDataManager.Instance.playerData.SubDiamond(50);
            for (int i = 0; i < wrenches.Length; i++)
            {
                if (wrenches[i].isClickable)
                {
                    wrenches[i].DoResolve();
                    break;  
                }
            }
        }
        else
        {
            Shop.gameObject.SetActive(true);
        }
    }
}
