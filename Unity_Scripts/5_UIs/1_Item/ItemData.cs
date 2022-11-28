using DataTableSt;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : TSingleton<ItemData>
{
    protected override void Init()
    {
        base.Init();
    }
    public void UpdateItemData()
    {
        Dictionary<int, Dictionary<int, int>> inventoryInfo = new Dictionary<int, Dictionary<int, int>>();
        Inventory _inventory = UIManager._instance.equipMentWindow.GetComponentInChildren<Inventory>();
        for (int n = 0; n < _inventory._slotList.Count; n++)
        {
            Dictionary<int, int> itemInfo = new Dictionary<int, int>();
            itemInfo.Add(_inventory._slotList[n]._key, _inventory._slotList[n]._amount);
            inventoryInfo.Add(n, itemInfo);
        }
        ItemSlotData(inventoryInfo);
    }
    void ItemSlotData(Dictionary<int, Dictionary<int, int>> inventoryInfo)
    {
        List<byte> _itemList = new List<byte>();
        foreach (KeyValuePair<int, Dictionary<int, int>> itemslot in inventoryInfo)
        {
            foreach (KeyValuePair<int, int> iteminfo in itemslot.Value)
            {
                _itemList.Add((byte)itemslot.Key);
                _itemList.Add((byte)iteminfo.Key);
                _itemList.Add((byte)iteminfo.Value);
            }
        }
        UserInfo._instance.GetItemData(_itemList.ToArray());
    }



}
