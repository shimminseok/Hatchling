using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectManager : MonoBehaviour
{
    static ObjectManager _uniqueInstance;

    public static ObjectManager _instance
    {
        get { return _uniqueInstance; }
    }
    public List<GameObject> _monsterList;

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
        _monsterList = new List<GameObject>();
    }
    public void Add(GameObject obj)
    {
        _monsterList.Add(obj);
    }
    public void Remove(GameObject obj)
    {
        _monsterList.Remove(obj);
    }
    public int ObjDistance(CharacterCtrl character)
    {
        float dis = 0;
        if (_monsterList != null)
        {
            dis = Vector3.Distance(character.transform.position, character._target.transform.position);
        }
        return (int)dis;
    }
}