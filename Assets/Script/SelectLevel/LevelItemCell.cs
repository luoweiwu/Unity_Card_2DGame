using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


//选关界面，每一个关卡按钮单元
public class LevelItemCell : MonoBehaviour ,IPointerEnterHandler,IPointerExitHandler,
        IPointerDownHandler,IPointerUpHandler,IPointerClickHandler{
    public RectTransform numTrans;
    public GameObject LockObj;
    public GameObject pressObj;

    //[HideInInspector]

    public int id;      //每个单元对应的关卡id（从1开始）
    public LevelManager lvlMgr;

    LevelInfo levelInfo;
    bool isLock;        

    //点击按钮，跳转到游戏界面
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLock)
        {
            return;
        }
        DataMgr.Instance().levelId = id;
        DataMgr.Instance().levelInfo = levelInfo;
        SceneManager.LoadScene("Gameplay");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressObj.SetActive(true);
    }

    //当鼠标进入本单元矩形区域，显示当前关卡描述
    public void OnPointerEnter(PointerEventData eventData)
    {
        lvlMgr.SetLevelDesc(levelInfo.desc);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        lvlMgr.SetLevelDesc("关卡信息");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressObj.SetActive(false);
    }

    private void Awake()
    {
        LockObj.SetActive(false);
        pressObj.SetActive(false);
    }

    // Use this for initialization
    void Start () {

        //根据解锁关卡记录，设置关卡是否锁定
        if (lvlMgr.unlock_levelID < id)
        {
            LockObj.SetActive(true);
            isLock = true;
        }

        float scale = 2;
        //初始化关卡数字显示
        if (id < 10)
        {
            //完全用代码动态创建一个Image对象，并作为num的子节点
            GameObject obj = new GameObject("num1",typeof(Image));
            RectTransform rtf = obj.GetComponent<RectTransform>();
            rtf.SetParent(numTrans);
            //设置数字
            Image img = obj.GetComponent<Image>();
            img.sprite = lvlMgr.spr_nums[id];
            img.SetNativeSize();    //图片原始尺寸
            rtf.localScale = new Vector3(2,2,1);
            rtf.localPosition = Vector3.zero;

        }
        else if (id<100)
        {
            //十位数
            GameObject obj = new GameObject("num1", typeof(Image));
            RectTransform rtf = obj.GetComponent<RectTransform>();
            rtf.SetParent(numTrans);
            //设置数字
            Image img = obj.GetComponent<Image>();
            img.sprite = lvlMgr.spr_nums[id/10];
            img.SetNativeSize();    //图片原始尺寸
            rtf.localScale = new Vector3(2, 2, 1);
            rtf.localPosition = new Vector3(-scale * rtf.rect.width/2-1,0,0);

            //个位数
            GameObject obj2 = new GameObject("num2", typeof(Image));
            RectTransform rtf2 = obj2.GetComponent<RectTransform>();
            rtf2.SetParent(numTrans);
            //设置数字
            Image img2 = obj2.GetComponent<Image>();
            img2.sprite = lvlMgr.spr_nums[id % 10];
            img2.SetNativeSize();    //图片原始尺寸
            rtf2.localScale = new Vector3(2, 2, 1);
            rtf2.localPosition = new Vector3(scale * rtf2.rect.width / 2 + 1, 0, 0);
        }
        levelInfo = DataMgr.Instance().levelData.levels[id - 1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
