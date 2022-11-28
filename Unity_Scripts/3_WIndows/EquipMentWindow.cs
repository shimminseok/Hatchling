using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipMentWindow : MonoBehaviour
{


    [SerializeField] GameObject[] _slot;
    [SerializeField] MountSlot[] _mountSlot;
    [SerializeField] Inventory _inventory;

    void Awake()
    {
        UIManager._instance.GetEquipmentWindow(this);
    }
    void Start()
    {
        gameObject.SetActive(false);
        GetMountItemData();
    }
    public void MountItem(int key, ItemSlot slot = null)
    {
        if (DataTableManager._instance._ItemDataDic.TryGetValue(key, out DataTableSt.stItemData itemData))
        {
            if (_slot[itemData._item_kind].transform.GetChild(0).GetComponent<Image>().sprite.Equals(ResourcePoolManager._instance.ItemImage(0)))
            {
                _slot[itemData._item_kind].transform.GetChild(0).GetComponent<Image>().sprite = ResourcePoolManager._instance.ItemImage(key);
                _mountSlot[itemData._item_kind].MountItem(key);
                slot.RemoveItem(0);
                SoundManager._instance.SFXSoundPlay(DefineEnumHelper.SFXSounds.MountEquipment);
            }
        }
    }
    public void DisMountItem(int key, DataTableSt.stItemData data, MountSlot slot)
    {
        _inventory.Add(key);
        data._value = 0;
        UserInfo._instance.MountEquipment(data);
        key = 0;
        slot.transform.GetChild(0).GetComponent<Image>().sprite = ResourcePoolManager._instance.ItemImage(key);

    }
    public void CheakMountItem()
    {
        List<byte> mountItem = new List<byte>();
        for (int n = 0; n < _mountSlot.Length; n++)
        {
            mountItem.Add((byte)_mountSlot[n]._itemKey);
        }
        UserInfo._instance.GetAmountData(mountItem.ToArray());
    }
    public void GetMountItemData()
    {
        for(int n =0; n< UserInfo._instance._mountItemData.Length;n++)
        {
            if(DataTableManager._instance._ItemDataDic.TryGetValue(UserInfo._instance._mountItemData[n],out DataTableSt.stItemData mountItem))
            {
                _slot[mountItem._item_kind].transform.GetChild(0).GetComponent<Image>().sprite = ResourcePoolManager._instance.ItemImage(UserInfo._instance._mountItemData[n]);
                _mountSlot[mountItem._item_kind]._itemKey = UserInfo._instance._mountItemData[n];
                UserInfo._instance.MountEquipment(mountItem);
            }
        }
    }

}
