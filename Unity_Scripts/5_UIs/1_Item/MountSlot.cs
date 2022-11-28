using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MountSlot : MonoBehaviour, IPointerClickHandler
{
    public int _itemKey;

    public bool _isMount;


    public DataTableSt.stItemData _data;

    public void MountItem(int key)
    {
        _itemKey = key;
        if (DataTableManager._instance._ItemDataDic.TryGetValue(key, out DataTableSt.stItemData itemData))
        {
            _isMount = true;
            _data = itemData;
            UserInfo._instance.MountEquipment(_data);
        }
    }
    public void DisMountItem()
    {
        EquipMentWindow owner = UIManager._instance.equipMentWindow;
        if (DataTableManager._instance._ItemDataDic.TryGetValue(_itemKey, out DataTableSt.stItemData itemData))
        {
            _data = itemData;
            owner.DisMountItem(_itemKey, _data, this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            return;
        }
        DisMountItem();
        _isMount = false;
        _itemKey = 0;
    }
}
