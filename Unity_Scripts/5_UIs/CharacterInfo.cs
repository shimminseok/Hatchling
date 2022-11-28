using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] Text _name;
    [SerializeField] Text _lv;
    [SerializeField] Text _hp;
    [SerializeField] Text _mp;
    [SerializeField] Text _dam;
    [SerializeField] Text _def;

    string _maxHp;
    string _maxMp;

    void Start()
    {
        _name.text = UserInfo._instance._nickName;
        _lv.text = string.Format("Lv {0}", UserInfo._instance._level.ToString());
        _hp.text = string.Format("{0} / {1}", UserInfo._instance._curHP, GameManager._instance.character.maxHp.ToString());
        _mp.text = string.Format("{0} / {1}", UserInfo._instance._curMP, GameManager._instance.character.maxHp.ToString());
        _dam.text = GameManager._instance.character.finalDam.ToString();
        _def.text = GameManager._instance.character.finalDef.ToString();
    }

    public void UpdateInfo(string lv, string curhp, string hp, string curmp,string mp, string dam, string def)
    {
        _maxHp = hp;
        _maxMp = mp;
        _lv.text = string.Format("Lv {0}", lv); ;
        _hp.text = string.Format("{0} / {1}", curhp, hp);
        _mp.text = string.Format("{0} / {1}", curmp, mp);
        _dam.text = dam;
        _def.text = def;
    }
    public void UpdateInfo(string dam, string def)
    {
        _dam.text = dam;
        _def.text = def;
    }
    public void UpdateInfo(int curHp, int curMp)
    {
        _hp.text = string.Format("{0} / {1}", curHp, GameManager._instance.character.maxHp);
        _mp.text = string.Format("{0} / {1}", curMp, GameManager._instance.character.maxHp);
    }

}
