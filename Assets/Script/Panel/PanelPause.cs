using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelPause : MonoBehaviour {

    public void Awake()
    {
        GameObject.Find("ContinueBtn").GetComponent<Button>().onClick.AddListener(()=> { OnContinueBtn(); });
        GameObject.Find("MainMenuBtn").GetComponent<Button>().onClick.AddListener(() => { OnBackMainMenu(); });
    }

    void OnContinueBtn()
    {
        gameObject.SetActive(false);
    }
    
    void OnBackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
