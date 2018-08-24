using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelResult : MonoBehaviour {

    public GameObject winTxtObj;
    public GameObject loseTxtObj;
    public Button nextBtn;
    public Button retryBtn;
    public Button mainmenuBtn;

    GameManager gameMgr;

    private void Awake()
    {

        nextBtn.onClick.AddListener(()=> { onNextLevelBtn(); });
        retryBtn.onClick.AddListener(() => { OnRetryBtn(); });
        mainmenuBtn.onClick.AddListener(() => { OnMainMenuBtn(); });

        gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //进行下一关
    void onNextLevelBtn()
    {
        gameMgr.NextLevel();
    }

    //再来一次
    void OnRetryBtn()
    {
        gameMgr.RetryLevel();
    }

    void OnMainMenuBtn()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //设置比赛结果相关信息显示
    public void MatchResult(bool win)
    {
        winTxtObj.SetActive(win);
        loseTxtObj.SetActive(!win);
        nextBtn.gameObject.SetActive(win);
        retryBtn.gameObject.SetActive(!win);

        if (win)
        {
            string level_key = System.Enum.GetName(typeof(THEME_ID), DataMgr.Instance().themeId)+"_level";    //主题名+下划线作为值键
            PlayerPrefs.SetInt(level_key,gameMgr.levelId+1);  //保存当前解锁的关卡编号,实际是当前关卡的下一关

        }
    }
}
