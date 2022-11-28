using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour
{

    [SerializeField] Button _optionButton;

    void Start()
    {
        _optionButton.onClick.AddListener(() => UIManager._instance.OpenOpttionWindow());
    }
    public void ClickOpenButton(GameObject go)
    {
        go.SetActive(true);
    }

    public void ClickRaidButton()
    {
        GameManager._instance.SceneConttroller("RaidScene");
    }
}

