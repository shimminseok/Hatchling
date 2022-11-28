using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePoolManager : MonoBehaviour
{
    static ResourcePoolManager _uniqueInstance;
    [Header("Windows")]
    [SerializeField] GameObject[] _window;
    [SerializeField] GameObject[] _messageBoxs;
    [SerializeField] GameObject _quitBox;
    [SerializeField] GameObject _option;



    [Header("Objects")]
    [SerializeField] GameObject[] _monsters;
    [SerializeField] GameObject _bloodObj;
    [SerializeField] GameObject _levelUpEffect;


    [Header("UI")]
    [SerializeField] Sprite[] _itemImage;
    [SerializeField] Sprite[] _loadingImage;
    public static ResourcePoolManager _instance => _uniqueInstance;
    void Awake()
    {
        if (_instance == null)
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public Sprite ItemImage(int key)
    {
        return _itemImage[key];
    }
    public GameObject GetMonsterObj(DefineEnumHelper.MonsterKind kind)
    {
        return _monsters[(int)kind];
    }
    public Sprite LoadingWindowImage(int index)
    {
        return _loadingImage[index];
    }
    public GameObject GetWindowObject(DefineEnumHelper.WindowKind kind)
    {
        return _window[(int)kind];
    }
    public GameObject GetMessageBox(DefineEnumHelper.MessageBoxKind kind)
    {
        return _messageBoxs[(int)kind];
    }
    public GameObject GetGameQuitBox()
    {
        return _quitBox;
    }
    public GameObject GetBooldObj()
    {
        return _bloodObj;
    }
    public GameObject GetLevelUpEffect()
    {
        return _levelUpEffect;
    }
    public GameObject GetOptionWindow()
    {
        return _option;
    }
}
