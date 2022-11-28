using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    [SerializeField] Image _hpBar;
    [SerializeField] Text _name;
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!UIManager._instance.monsterWindow.monsterName.text.Equals(string.Empty))
        {
            transform.LookAt(Camera.main.transform);
            TargetMonsterInfo();
        }
    }

    public void TargetMonsterInfo()
    {
        _hpBar.fillAmount = UIManager._instance.monsterWindow.monsterHp;
        string name = UIManager._instance.monsterWindow.monsterName.text.Split()[0] + UIManager._instance.monsterWindow.monsterName.text.Split()[2];
        name = name.Replace("Lv", "");
        _name.text = name;
    }
}
