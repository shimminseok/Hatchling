using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using DataTableSt;

public class DataTableManager : MonoBehaviour
{
    static DataTableManager _uniqueInstance;
    public static DataTableManager _instance => _uniqueInstance;

    JsonData _jsonData;

    stCharacterInitData _initData;

    public Dictionary<int, stMonsterInitData> _monsterDic { get; } = new Dictionary<int, stMonsterInitData>();
    public Dictionary<int, stItemData> _ItemDataDic { get; } = new Dictionary<int, stItemData>();
    public Dictionary<int, stRaidMonsterData> _raidMonsterDic { get; } = new Dictionary<int, stRaidMonsterData>();
    public stCharacterInitData Init => _initData;

    void Awake()
    {
        if (_uniqueInstance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadItemData();
        LoadCharacterInitData();
        LoadMonsterInitData();
        LoadRaidMonsterData();
    }
    void ParsingJsonItem(JsonData data, Dictionary<int,stItemData> itemDic)
    {
        for (int i = 0; i < data.Count; i++)
        {
            int index = int.Parse(data[i][0].ToString()); //인덱스
            string name = data[i][1].ToString(); //이름
            int type = int.Parse(data[i][2].ToString()); //아이템 타입
            int item_kind = int.Parse(data[i][3].ToString()); //아이템 종류
            int stat = int.Parse(data[i][4].ToString()); //증가 능력치
            float value = float.Parse(data[i][5].ToString());
            string tooltip = data[i][6].ToString();
            int price = int.Parse(data[i][7].ToString());
            stItemData tempItem = new stItemData(name, type, item_kind, stat, value,tooltip, price);
            itemDic.Add(index, tempItem);
        }
    }
    void ParsingJsonCharacterInit(JsonData data)
    {
        for (int i = 0; i < data.Count;i++)
        {
            string index = data[i][0].ToString(); //인덱스
            string lv = data[i][1].ToString(); //이름
            string hp = data[i][2].ToString();
            string mp = data[i][3].ToString(); //데미지
            string dam = data[i][4].ToString();
            string def = data[i][5].ToString();
            string ex = data[i][6].ToString();

            int tempindex = int.Parse(index);
            int tempLv = int.Parse(lv);
            int temphp = int.Parse(hp);
            int tempmp = int.Parse(mp);
            int tempdam = int.Parse(dam);
            int tempdef = int.Parse(def);
            int tempex = int.Parse(ex);
            _initData = new stCharacterInitData(tempLv,temphp,tempmp,tempdam,tempdef,tempex);
        }
    }
    void ParsingJsonMonsterInitData(JsonData data, Dictionary<int,stMonsterInitData> dic)
    {
        for (int i = 0; i < data.Count; i++)
        {
            string index = data[i][0].ToString(); //인덱스
            string name = data[i][1].ToString(); //이름
            string lv = data[i][2].ToString();
            string hp = data[i][3].ToString(); //데미지
            string ex = data[i][4].ToString();
            string dam = data[i][5].ToString();
            string def = data[i][6].ToString();
            string attdis = data[i][7].ToString();
            string speed = data[i][8].ToString();
            string min = data[i][9].ToString();
            string max = data[i][10].ToString();

            int tempindex = int.Parse(index);
            string tempname = name;
            int tempLv = int.Parse(lv);
            int temphp = int.Parse(hp);
            int tempdam = int.Parse(dam);
            int tempdef = int.Parse(def);
            int tempex = int.Parse(ex);
            float tempattdis = float.Parse(attdis);
            float tempSpeed = float.Parse(speed);
            int tempMin = int.Parse(min);
            int tempMax = int.Parse(max);
            stMonsterInitData mondata = new stMonsterInitData(tempname,tempLv,temphp,tempex,tempdam,tempdef,tempattdis,tempSpeed,tempMin,tempMax);

            dic.Add(tempindex, mondata);
        }
    }
    void ParsingJsonRaidMonsterData(JsonData data)
    {
        for(int i=0; i<data.Count;i++)
        {
            int index = int.Parse(data[i][0].ToString());
            string name = data[i][1].ToString();
            int hp = int.Parse(data[i][2].ToString());
            float damege = float.Parse(data[i][3].ToString());
            float[] patDam = { float.Parse(data[i][4].ToString()), float.Parse(data[i][5].ToString()), float.Parse(data[i][6].ToString()) };
            int attDis = int.Parse(data[i][7].ToString());
            int speed = int.Parse(data[i][8].ToString());
            int def = int.Parse(data[i][9].ToString());

            stRaidMonsterData rm = new stRaidMonsterData(name, hp, damege, def, patDam, attDis, speed);
            _raidMonsterDic.Add(index, rm);
        }
    }
    public void LoadItemData()
    {
        string jsonstring;
        string path = Application.streamingAssetsPath +"\\Item.json";
        jsonstring = File.ReadAllText(path);
        _jsonData = JsonMapper.ToObject(jsonstring);
        ParsingJsonItem(_jsonData, _ItemDataDic);
    }
    public void LoadCharacterInitData()
    {
        string jsonstring;
        string path = Application.streamingAssetsPath + "\\CharacterInitData.json";
        jsonstring = File.ReadAllText(path);
        _jsonData = JsonMapper.ToObject(jsonstring);
        ParsingJsonCharacterInit(_jsonData);
    }
    public void LoadMonsterInitData()
    {
        string jsonstring;
        string path = Application.streamingAssetsPath + "\\MonsterInitData.json";
        jsonstring = File.ReadAllText(path);
        _jsonData = JsonMapper.ToObject(jsonstring);
        ParsingJsonMonsterInitData(_jsonData,_monsterDic);
    }
    public void LoadRaidMonsterData()
    {
        string jsonstring;
        string path = Application.streamingAssetsPath + "\\RaidMonsterData.json";
        jsonstring = File.ReadAllText(path);
        _jsonData = JsonMapper.ToObject(jsonstring);
        ParsingJsonRaidMonsterData(_jsonData);
    }
}
