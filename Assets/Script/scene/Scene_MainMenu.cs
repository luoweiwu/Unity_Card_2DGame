using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum THEME_ID
{
    Logo,
    Student
}

public class Scene_MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find("LogoBtn").GetComponent<Button>().onClick.AddListener(()=>{ OnClickThemeBtn(THEME_ID.Logo); });
        GameObject.Find("StudentBtn").GetComponent<Button>().onClick.AddListener(() => { OnClickThemeBtn(THEME_ID.Student); });
        GameObject.Find("CloseBtn").GetComponent<Button>().onClick.AddListener(() => { OnCloseApp(); });
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    void OnClickThemeBtn(THEME_ID theme)
    {
        SceneManager.LoadScene("SelectLevel");
        DataMgr.Instance().themeId = theme;
    }

    //退出程序
    void OnCloseApp()
    {
        Application.Quit();
    }
 
}
