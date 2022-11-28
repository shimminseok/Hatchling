using DataTableSt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    [SerializeField] Transform _root;
    [SerializeField] GameObject _slot;

    [Range(0, 10)] int _horizontalSlotCount = 7;
    [Range(0, 10)] int _verticalSlotCount = 8;

    public List<ItemSlot> _slotList { get; private set; } = new List<ItemSlot>();
    List<int[]> _item = new List<int[]>();
    public int Capacity { get; private set; }

    void Awake()
    {
        InitSlot();
        GetItemData();
    }
    void InitSlot()
    {
        for (int n = 0; n < _verticalSlotCount; n++)
        {
            for (int m = 0; m < _horizontalSlotCount; m++)
            {
                int slotIndex = (_horizontalSlotCount * n) + m;
                GameObject go = Instantiate(_slot, _root);
                var itemslot = go.GetComponent<ItemSlot>();
                itemslot.SetSlotIndex(slotIndex);
                _slotList.Add(itemslot);
            }
        }
        Capacity = _verticalSlotCount * _horizontalSlotCount;
    }
    public void Add(int key,int price = 0)
    {
        if (DataTableManager._instance._ItemDataDic.TryGetValue(key, out stItemData itemData))
        {
            for (int n = 0; n < _slotList.Count; n++)
            {
                if (_slotList[n]._hasItem && itemData._type.Equals(_slotList[n]._itemData._type) && itemData._item_kind.Equals(_slotList[n]._itemData._item_kind) && itemData._type.Equals((int)DefineEnumHelper.ItemType.사용아이템))
                {
                    if (_slotList[n]._amount < _slotList[n]._maxAmount)
                    {
                        UpdateSlot(key, n, itemData);
                        break;
                    }
                }
                else if (!_slotList[n]._hasItem)
                {
                    UpdateSlot(key, n, itemData);
                    break;
                }
            }
            GameManager._instance.character._money -= price;
        }
    }
    public void UpdateSlot(int key, int index, stItemData itemData)
    {
        _slotList[index].SetItemAmount(++_slotList[index]._amount);
        _slotList[index].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ResourcePoolManager._instance.ItemImage(key);
        _slotList[index]._key = key;
        _slotList[index]._hasItem = true;
        _slotList[index]._itemData = itemData;

    }
    public void GetItemData()
    {
        for (int n = 0; n < (UserInfo._instance._itemdata.Length) / 3; n++)
        {
            for (int m = n; m < n + 1; m++)
            {
                int index = (m * 2) + (n);
                int key = (m * 2) + (n + 1);
                int amount = (m * 2) + (n + 2);
                int[] itemdata = { UserInfo._instance._itemdata[key], UserInfo._instance._itemdata[amount] };
                _item.Add(itemdata);
            }
        }
        for (int n = 0; n < _item.Count; n++)
        {
            if (DataTableManager._instance._ItemDataDic.TryGetValue(_item[n][0], out stItemData itemdata))
            {
                _slotList[n].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = ResourcePoolManager._instance.ItemImage(_item[n][0]);
                _slotList[n]._itemData = itemdata;
                _slotList[n]._key = _item[n][0];
                _slotList[n].SetItemAmount(_item[n][1]);
                _slotList[n]._hasItem = true;
            }
        }
    }
    public void DisMountItem()
    {
        EquipMentWindow mount = UIManager._instance.equipMentWindow;
    }
}
