using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr {
    private static DataMgr ins = null;

    public THEME_ID themeId;

    //某一主题所有关卡数据
    public LevelData levelData;
    public int levelId;             //进入游戏前，保存当前选择的关卡id
    public LevelInfo levelInfo;     //当前关卡信息

    private DataMgr()
    {
        //将构造函数设置为私有，这样避免外部创建类的实例，只能通过Instance()来获取数据管理类的实例
    }

    public static DataMgr Instance()
    {
        if(ins == null)
        { 
            ins = new DataMgr();
        }
        return ins;
    }

    public LevelData InitLevelData(THEME_ID id)
    {
        levelData = null;
        switch (id)
        {
            case THEME_ID.Logo:
                levelData = Resources.Load<LevelData>("Prefabs/data/LevelDataLogo");
                break;
            case THEME_ID.Student:
                levelData = Resources.Load<LevelData>("Prefabs/data/LevelDataStudent");
                break;
            default:
                levelData = Resources.Load<LevelData>("Prefabs/data/LevelDataLogo");
                break;
        }
        return levelData; 
    }
}
