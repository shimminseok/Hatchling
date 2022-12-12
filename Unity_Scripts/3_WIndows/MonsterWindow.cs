using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterWindow : MonoBehaviour
{
    [SerializeField] Image _monsterHP;
    [SerializeField] Text _monsterName;
    [SerializeField] Text _monsterDis;


    public float monsterHp => _monsterHP.fillAmount;
    public Text monsterName => _monsterName;
    void Awake()
    {
        InitData();
    }
    void OnDisable()
    {
        _monsterName.text = string.Empty;
        _monsterDis.text = string.Empty;
    }
    void InitData()
    {
    }
    public void MonsterName(int lv,string name)
    {
        gameObject.SetActive(true);
        int lvGab = lv - GameManager._instance.character.level;
        string txt = string.Empty;
        if (lvGab >= 7)
        {
            txt = string.Format("<color=#ff0000ff>Lv {0} {1}</color>", lv.ToString(), name);
        }
        else if(lvGab >= 5 && lvGab >= 3)
        {
            txt = string.Format("<color=#FF9F00>Lv {0} {1}</color>", lv.ToString(), name);
        }
        else if(lvGab >=0 )
        {
            txt = string.Format("<color=#000000>Lv {0} {1}</color>", lv.ToString(), name);
        }
        else
        {
            txt = string.Format("<color=grey>Lv {0} {1}</color>", lv.ToString(), name);
        }
        _monsterName.text = txt;
    }
    public void MonsterHP(float h, float mh)
    {
        _monsterHP.fillAmount = (float)h / mh;
    }
    public void MonsterDis(float mondis)
    {
        _monsterDis.text = string.Format("{0}m", (int)mondis);
    }


}
