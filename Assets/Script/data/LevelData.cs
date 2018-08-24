using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//某一关数据结构
[System.Serializable]
public class LevelInfo
{
    public int id;      //关卡id
    public int row;
    public int col;
    public int count;   //本关从指定集合中随机几个素材
    public string desc;     //关卡简要描述
    public Sprite[] sprites;
}

public class LevelData : MonoBehaviour {

    public LevelInfo[] levels;
}
