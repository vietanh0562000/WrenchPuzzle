using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
  
    public TextMeshProUGUI loading;
    public Slider slider;
    int startTime = 0;
    int newTime = 100;
    public Text CurrentLevelText;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    private void Awake()
    {
      
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        // Animation for increasing and decreasing of coins amount
        const float seconds = 2f;
        float elapsedTime = 0;

        while (elapsedTime < seconds)
        {
            loading.text = "" + Mathf.Floor(Mathf.Lerp(startTime, newTime, (elapsedTime / seconds))) +"%";
            slider.value = Mathf.Floor(Mathf.Lerp(startTime, newTime, (elapsedTime / seconds)));
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        startTime = newTime;
        loading.text = "" + newTime + "%";
        SceneManager.LoadScene("Level");
    }
}
