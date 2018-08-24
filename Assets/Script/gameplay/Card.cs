using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour,IPointerClickHandler {

    public GameObject bgObj;
    public Image iconImg;
    public GameObject maskObj;

    public enum STATE
    {
        Init,       //进入游戏时，展示几秒钟
        Closed,     //卡牌扣下状态
        Opened,     //点击一张卡牌时，翻开状态
        OK,        //匹配成功状态
        Fail        //匹配失败状态,短暂展示1秒钟，自动切换到扣下状态
    }

    public int id;
    GameManager gameMgr;
    STATE m_state;
    float _timer;   

	// Use this for initialization
	void Start () {
        SwithState(STATE.Init);

	}
	
	// Update is called once per frame
	void Update () {

        if(gameMgr.m_state != GameManager.STATE.Normal)
        {
            return;
        }

        _timer += Time.deltaTime;

        switch (m_state)
        {
            case STATE.Init:
                {
                    if (_timer > 5)
                    {
                        SwithState(STATE.Closed);
                    }
                }
                break;
            case STATE.Closed:
                break;
            case STATE.Opened:
                break;
            case STATE.OK:
                break;
            case STATE.Fail:
                {
                    if (_timer > 1)
                    {
                        SwithState(STATE.Closed);
                    }
                }
                break;
            default:
                break;
        }
	}

    public void Init(int id,Sprite spr,GameManager manager)
    {
        this.id = id;
        gameMgr = manager;
        //根据卡牌id，设置对应的图片素材
        iconImg.sprite = spr;
    }


    //卡牌点击事件，系统自动调用
    public void OnPointerClick(PointerEventData eventData)
    {
        //卡牌切换到opened状态
        if (m_state == STATE.Closed)
        {
            SwithState(STATE.Opened);
            gameMgr.CheckCard(this);
        }
    }

    //卡牌状态切换
    void SwithState(STATE state)
    {
        _timer = 0;
        m_state = state;
        switch (state)
        {
            case STATE.Init:
                iconImg.gameObject.SetActive(true);
                maskObj.SetActive(false);
                break;
            case STATE.Closed:
                iconImg.gameObject.SetActive(false);
                break;
            case STATE.Opened:
                iconImg.gameObject.SetActive(true);
                break;
            case STATE.OK:
                maskObj.SetActive(true);
                break;
            case STATE.Fail:
                break;
            default:
                break;
        }
    }

    public void MatchOK()
    {
        SwithState(STATE.OK);
    }

    public void MatchFail()
    {
        SwithState(STATE.Fail);
    }
}
