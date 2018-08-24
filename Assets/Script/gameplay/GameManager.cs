using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//用来管理游戏运行状态资源、整体性

public class GameManager : MonoBehaviour {
    public GameObject carPreb;
    public PanelResult resultPanel;

    public GameObject pausePanel;
    public GameObject aboutPanel;

    public Image minule1Img;
    public Image minule2Img;
    public Image second1Img;
    public Image second2Img;
    public Sprite[] num_sprites;   //  数字sprite的引用


    public readonly int MATCH_TIMA = 75;   //时间倒计时

    public enum STATE
    {
        Normal,
        Pause,
        About,
        Result
    }

    public STATE m_state;

    private LevelInfo levelInfo;    //本关卡数据信息

    [HideInInspector]
    public int levelId;
    int open_card_num;          //翻开卡牌的数量
    int match_ok_num;           //匹配成功的卡牌数量

    Card selectCard1;           //保存翻开的第一张牌
    Card selectCard2;           //保存翻开的第二张牌
    List<Card> list_cards = new List<Card>();      //保存所有卡牌引用
    float _timer;


    private void Awake()
    {
        levelInfo = DataMgr.Instance().levelInfo;
        levelId = DataMgr.Instance().levelId;

        GameObject.Find("PanseBtn").GetComponent<Button>().onClick.AddListener(() => { OnPauseBtn(); });
        GameObject.Find("AudioBtn").GetComponent<Button>().onClick.AddListener(() => { OnAudioBtn(); });
        GameObject.Find("AboutBtn").GetComponent<Button>().onClick.AddListener(() => { OnAboutBtn(); });
    }


    // Use this for initialization
    void Start() {

        InitBoard();
        GameObject.Find("levelTxt").GetComponent<Text>().text = levelId.ToString();
    }

    // Update is called once per frame
    void Update() {

        switch (m_state)
        {
            case STATE.Normal:
                {
                    _timer += Time.deltaTime;
                    int timeRemain = (int)(MATCH_TIMA - _timer);

                    if (timeRemain < 0)     //比赛失败
                    {
                        SwitchState(STATE.Result);
                        resultPanel.MatchResult(false);
                    }
                    else
                    {
                        UpdateTimeDisplay(timeRemain);      //更新比赛倒计时
                    }
                }
                break;
            case STATE.Pause:
                break;
            case STATE.About:
                break;
            case STATE.Result:
                break;
            default:
                break;
        }

    }

    //初始化棋盘
    void InitBoard()
    {
        open_card_num = 0;
        match_ok_num = 0;
        SwitchState(STATE.Normal);
        ClearAllCard();

        //求游戏区域的宽高
        GameObject gameAreaObj = GameObject.Find("GameArea");
        RectTransform rectTrans = gameAreaObj.GetComponent<RectTransform>();
        float gameWidth = rectTrans.rect.width;
        float gameHeigh = rectTrans.rect.height;
        //获取关卡信息
        int row = levelInfo.row;
        int col = levelInfo.col;
        //根据关卡信息的行列信息，初始化位置
        float spacingW = gameWidth / col / 10;
        float spacingH = gameHeigh / row / 10;
        float cellW = (gameWidth - spacingW * (col + 1)) / col;
        float cellH = (gameHeigh - spacingH * (row + 1)) / row;
        float cellSize = Mathf.Min(cellW, cellH);        //最终求出正方形卡牌尺寸

        //求出水平和垂直方向实际空格间隙
        float spacingX = (gameWidth - cellSize * col) / (col + 1);
        float spacingY = (gameHeigh - cellSize * row) / (row + 1);


        //根据关卡定义中图片素材个数，初始化一个索引下标的列表
        List<int> src_ids = new List<int>(levelInfo.sprites.Length);
        for (int i = 0; i < levelInfo.sprites.Length; i++)
        {
            src_ids.Add(i);
        }

        //从上面的下标集合中，随机关卡中定义的count数出来
        int[] rand_ids = new int[levelInfo.count];  //卡牌随机的素材下标集合
        for (int i = 0; i < levelInfo.count; i++)
        {
            int idx = Random.Range(0, src_ids.Count);
            rand_ids[i] = src_ids[idx];
            src_ids.RemoveAt(idx);        //删除随机过的素材下标，下次循环从剩余的集合中随机
        }

        //初始化卡牌id数组，我们卡牌id,可以用数组下标来表示，并且要保证卡牌成对出现
        List<int> cardIds = new List<int>(row * col);
        for (int i = 0; i < row * col / 2; i++)
        {
            if (i < levelInfo.count)
            {
                cardIds.Add(rand_ids[i]);
                cardIds.Add(rand_ids[i]);
            }
            else
            {
                int idx = Random.Range(0, rand_ids.Length);
                cardIds.Add(idx);
                cardIds.Add(idx);
            }
        }

        //对所有卡牌下标集合进行随机打乱
        for (int i = 0; i < row * col; i++)
        {
            int idx = Random.Range(0, cardIds.Count);
            int temp = cardIds[i];        //随机一张牌，跟i对调，进而打乱顺序
            cardIds[i] = cardIds[idx];
            cardIds[idx] = temp;
        }



        int count = row * col;

        //创建所有卡牌
        for (int i = 0; i < count; i++)
        {
            int row2 = i / col;
            int col2 = i % col;
            GameObject cardObj = Instantiate(carPreb, rectTrans);    //通过预制体创建卡牌对象
            cardObj.name = "Card" + i.ToString();
            RectTransform cardTrans = cardObj.GetComponent<RectTransform>();
            cardTrans.anchoredPosition = new Vector2(spacingX + (spacingX + cellSize) * col2, -spacingY - (spacingY + cellSize) * row2);
            cardTrans.sizeDelta = new Vector2(cellSize, cellSize);
            Card card = cardObj.GetComponent<Card>();
            card.Init(cardIds[i], levelInfo.sprites[cardIds[i]], this);
            list_cards.Add(card);
        }

    }

    //棋盘两个核心AI检测
    public void CheckCard(Card selectCard)
    {
        switch (open_card_num)
        {
            case 0:     //一张牌没翻开时
                {
                    open_card_num = 1;
                    selectCard1 = selectCard;
                }
                break;
            case 1:     //已经翻开一张牌，此时的selectCard,是第二张牌
                {
                    if (selectCard1 == selectCard)   //实际上不会调用
                    {
                        return;
                    }
                    selectCard2 = selectCard;
                    if (selectCard1.id == selectCard2.id)     //匹配成功
                    {
                        selectCard1.MatchOK();
                        selectCard2.MatchOK();
                        match_ok_num++;
                        if (match_ok_num >= levelInfo.row * levelInfo.col / 2)
                        {
                            //全部匹配成功，比赛胜利结束，进行下一关
                            SwitchState(STATE.Result);
                            resultPanel.MatchResult(true);
                        }
                    }
                    else                                  //匹配失败
                    {
                        selectCard1.MatchFail();
                        selectCard2.MatchFail();
                        // SwitchState(STATE.Result);        解开有惊喜
                        //resultPanel.MatchResult(false);    解开有惊喜
                    }

                    open_card_num = 0;
                    selectCard1 = null;
                    selectCard2 = null;
                }
                break;
            default:
                break;
        }
    }

    public void NextLevel()
    {
        int levelCount = DataMgr.Instance().levelData.levels.Length;
        if (levelId < levelCount)
        {
            levelId++;
            DataMgr.Instance().levelId = levelId;
            levelInfo = DataMgr.Instance().levelData.levels[levelId - 1];
        }
        InitBoard();
    }

    public void RetryLevel()
    {
        InitBoard();
    }


    //销毁所有卡牌，以便重新初始化
    void ClearAllCard()
    {
        if (list_cards.Count == 0)
        {
            return;
        }
        foreach (Card item in list_cards)
        {
            Destroy(item.gameObject);
        }
    }

    void SwitchState(STATE state)
    {
        m_state = state;
        _timer = 0;
        switch (state)
        {
            case STATE.Normal:
                resultPanel.gameObject.SetActive(false);
                pausePanel.SetActive(false);
                aboutPanel.SetActive(false);
                break;
            case STATE.Pause:
                pausePanel.SetActive(true);
                break;
            case STATE.About:
                aboutPanel.SetActive(true);
                break;
            case STATE.Result:
                resultPanel.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    //用来更新比赛倒计时显示
    void UpdateTimeDisplay(int timeRemain)
    {
        System.TimeSpan tspan = new System.TimeSpan(0, 0, timeRemain);
        string timeStr = string.Format("{0:00}:{1:00}", tspan.Minutes, tspan.Seconds);
        minule1Img.sprite = num_sprites[int.Parse(timeStr.Substring(0, 1))];    //把第一位字符转换成整数
        minule2Img.sprite = num_sprites[int.Parse(timeStr.Substring(1, 1))];    //把第二位字符转换成整数
        second1Img.sprite = num_sprites[int.Parse(timeStr.Substring(3, 1))];    //把第四位字符转换成整数   跳过的第三位是":"符号
        second2Img.sprite = num_sprites[int.Parse(timeStr.Substring(4, 1))];    //把第五位字符转换成整数
    }

    //暂停按钮，弹出暂停界面panel
    void OnPauseBtn()
    {
        SwitchState(STATE.Pause);
    }

    void OnAudioBtn()
    {
        //声音开关
    }

    void OnAboutBtn()
    {
        SwitchState(STATE.About);
    }
}
