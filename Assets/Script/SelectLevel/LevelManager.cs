using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

//用来管理所有关卡按钮
//根据我们关卡数据定义，判断一共多少个关卡按钮。LevelManager来创建LevelPanelCell表格单元，LevelPanelCell表格单元是LevelItemCell
public class LevelManager : MonoBehaviour
{

    int totalCount = 0; //临时测试，一共18关
    readonly int ITEM_COUNT_PAGE = 15;  //每一页显示15个关卡
    readonly float PANEL_SPACE = 10f;
    readonly float MOVE_TIME = 0.5f;

    public RectTransform contenTrans;
    public Sprite[] spr_nums;

  

    float panelCellOffset;
    int pageCount;

    GameObject levelPanelCellPreb;
    GameObject levelItemCellPreb;
    Text levelDescTxt;

    float pointerDownTimeBak;
    float pointerDownX;

    int pageIdx;
    [HideInInspector]
    public int unlock_levelID=1;

    public void Awake()
    {
        levelPanelCellPreb = Resources.Load<GameObject>("Prefabs/SelectLevel/LevelPanelCell");
        levelItemCellPreb = Resources.Load("Prefabs/SelectLevel/LevelItemCell") as GameObject;
        levelDescTxt = GameObject.Find("LevelDesc/text").GetComponent<Text>();

        GameObject.Find("BackMainMenu").GetComponent<Button>().onClick.AddListener(()=> { OnBackMainMenuBtn(); });

        DataMgr.Instance().InitLevelData(DataMgr.Instance().themeId);
        totalCount = DataMgr.Instance().levelData.levels.Length;    //根据关卡数据表的个数，动态设置单元数量


        string level_key = System.Enum.GetName(typeof(THEME_ID), DataMgr.Instance().themeId) + "_level";
        if(PlayerPrefs.HasKey(level_key))
        {
            unlock_levelID = PlayerPrefs.GetInt(level_key);
        }
        else
        {
            unlock_levelID = 1;         //如果是第一次玩，还没有记录过，给默认解锁第一关
        }
        
    }

    // Use this for initialization
    void Start()
    {
        //计算panel的偏移
        panelCellOffset = contenTrans.rect.width + PANEL_SPACE;
        //计算出一共需要多少页
        pageCount = Mathf.CeilToInt((float)totalCount / ITEM_COUNT_PAGE);
        //当前显示的第几页表格panel，从0开始
        pageIdx = 0;

        for (int i = 0; i < pageCount; i++)
        {
            CreateLevelPanclCell(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //按下时记录鼠标当前位置
        if (Input.GetMouseButtonDown(0))
        {
            pointerDownTimeBak = Time.time;
            pointerDownX = Input.mousePosition.x;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //模拟滑动事件,假定很短时间的滑动
            if (Time.time - pointerDownTimeBak < 0.5f)
            {
                float offsetX = Input.mousePosition.x - pointerDownX;
                //向左滑动鼠标，并超过一定距离
                if (offsetX < -200 && pageIdx < pageCount - 1)
                {
                    //向左滑动鼠标，并超过一段距离
                    pageIdx++;
                    //执行所有表格面板向左平滑滑动
                    //0.5秒对自身左边叠加量，向左移动一段距离
                    contenTrans.DOBlendableLocalMoveBy(new Vector3(-panelCellOffset, 0, 0), MOVE_TIME);

                }
                else if (offsetX > 200 && pageIdx > 0)
                {
                    //向右滑动鼠标，并超过一段距离
                    pageIdx--;
                    //执行所有表格面板向右平滑滑动
                    //0.5秒对自身左边叠加量，向右移动一段距离
                    contenTrans.DOBlendableLocalMoveBy(new Vector3(panelCellOffset, 0, 0), MOVE_TIME);
                }
            }
        }
    }

    //创建表格panel，从0开始编号
    void CreateLevelPanclCell(int pageInx)
    {
        GameObject panelObj = Instantiate(levelPanelCellPreb, contenTrans);
        RectTransform panelTrans = panelObj.GetComponent<RectTransform>();
        panelTrans.anchoredPosition = new Vector2(panelCellOffset * pageInx, 0);

        //确定本业 中具有的关卡元素个数
        int startId = ITEM_COUNT_PAGE * pageInx;
        //本业需要显示按钮的个数
         int count = totalCount - startId;

        if (count > ITEM_COUNT_PAGE)
        {
            count = ITEM_COUNT_PAGE;
        }

        //创建本页中所有关卡按钮
        for (int i = 0; i < count; i++)
        {
            //创建每一个关卡的按钮单元
            GameObject itemObj = Instantiate(levelItemCellPreb, panelTrans);

            //这一段cell相关调用，实在Start()之前执行的
            LevelItemCell cell = itemObj.GetComponent<LevelItemCell>();
            cell.id = startId + i + 1;      //设置每个单元的关卡编号
            cell.lvlMgr = this;
            itemObj.name = cell.id.ToString();

        }

    }

    //设置关卡描述
    public void SetLevelDesc(string content)
    {
        levelDescTxt.text = content;
    }

    void OnBackMainMenuBtn()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
