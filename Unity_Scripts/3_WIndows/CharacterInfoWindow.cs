using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoWindow : MonoBehaviour
{
    [SerializeField] Image _hpbar;
    [SerializeField] Text _txtHp;
    [SerializeField] Image _mpbar;
    [SerializeField] Text _txtMp;

    [SerializeField] Slider _exBar;

    [SerializeField] GameObject _mapName;

    float _lerpTime;
    float _currentTime;

    void Awake()
    {
        InitData();
    }
    void InitData()
    {
        UIManager._instance.GetCharacterInfoWindow(this);
    }
    /// <summary>
    /// 캐릭터의 현재 HP를 나타내는 UI 함수
    /// </summary>
    /// <param name="h">캐릭터의 현재 HP</param>
    /// <param name="mh">캐릭터의 MaxHP</param>
    public void CharacterHP(int h, int mh ,int mp, int mmp)
    {
        UIManager._instance.equipMentWindow.GetComponentInChildren<CharacterInfo>().UpdateInfo(h, mmp);
        _hpbar.fillAmount = (float)h / mh;
        _txtHp.text = string.Format("{0}/{1}", h, mh);
        _mpbar.fillAmount = (float)mp / mmp;
        _txtMp.text = string.Format("{0}/{1}", mp, mmp);
    }
    /// <summary>
    /// 캐릭터의 현재 경험치를 나타내는 UI
    /// </summary>
    /// <param name="ex"> 현재 경험치</param>
    /// <param name="mex"> 맥스 경험치</param>
    public void CharacterEx(int ex, int mex)
    {
        _exBar.maxValue = mex;
        _exBar.value = ex;
    }
    public void OpenMapNameBox(string mapName)
    {
        Animator ani = GetComponent<Animator>();
        _mapName.gameObject.SetActive(true);
        Text txt = _mapName.GetComponentInChildren<Text>();
        txt.text = mapName;
    }
}
