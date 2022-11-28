using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] Inventory _inventory;

    [SerializeField] Image[] _icons;
    [SerializeField] Text[] _pricetxt;
    [SerializeField] Button _exitButton;
    [SerializeField] Text _gold;

    List<int> _itemKey = new List<int>();
    List<DataTableSt.stItemData> _itemdata = new List<DataTableSt.stItemData>();
    void Start()
    {
        foreach(KeyValuePair<int,DataTableSt.stItemData> data in DataTableManager._instance._ItemDataDic)
        {
            _itemKey.Add(data.Key);
            _itemdata.Add(data.Value);
        }
        for(int n =0; n< _icons.Length;n++)
        {
            _icons[n].sprite = ResourcePoolManager._instance.ItemImage(_itemKey[n]);
            _pricetxt[n].text = _itemdata[n]._price.ToString();
        }
        _exitButton.onClick.AddListener(() => UIManager._instance.ExitWindow(gameObject));

        gameObject.SetActive(false);
    }
    public void BuyItem(int key)
    {
        if(GameManager._instance.character._money < _itemdata[key]._price)
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, transform.root, "골드가 부족합니다.");
            return;
        }
        _inventory.Add(_itemKey[key], _itemdata[key]._price);
    }
    void Update()
    {
        _gold.text = GameManager._instance.character._money.ToString();
    }
}
