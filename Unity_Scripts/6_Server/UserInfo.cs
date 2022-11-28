using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : TSingleton<UserInfo>
{
    //static UserInfo _uniqueInstance;
    //public static UserInfo _instance => _uniqueInstance;

    public long _myUUid { get; private set; }
    public string _id { get; private set; }
    public string _nickName { get; private set; }
    public int _level { get; private set; }
    public int _curHP { get; private set; }
    public int _curMP { get; private set; }
    public int _money { get; private set; }
    public int _curEx { get; private set; }
    public byte[] _itemdata { get; private set; }
    public byte[] _mountItemData { get; private set; }
    public Vector3 _curPos { get; private set; }

    public int _helmetDef { get; private set; }
    public int _armorDef { get; private set; }
    public int _weaponDam { get; private set; }
    public int _ringDam { get; private set; }
    public int _shoseDef { get; private set; }

    protected override void Init()
    {
        base.Init();
    }

    public void SetUUID(long uuid)
    {
        _myUUid = uuid;
    }
    public void SetID(string id)
    {
        _id = id;
    }
    public void SetUserInfo(string nick, int lv, int hp, int mp, int money, int ex)
    {
        _nickName = nick;
        _level = lv;
        _curHP = hp;
        _curMP = mp;
        _money = money;
        _curEx = ex;
    }
    public void SaveUserPosition(Vector3 pos)
    {
        _curPos = new Vector3(pos.x, pos.y, pos.z);
    }
    public void GetItemData(byte[] itemdata)
    {
        _itemdata = itemdata;
    }
    public void GetAmountData(byte[] amount)
    {
        _mountItemData = amount;
    }
    public void UserItemInfo()
    {
        ItemData._instance.UpdateItemData();
    }
    public void MountEquipment(DataTableSt.stItemData data)
    {
        switch ((DefineEnumHelper.EquipmentItemKind)data._item_kind)
        {
            case DefineEnumHelper.EquipmentItemKind.Helmet:
                _helmetDef = (int)data._value;
                break;
            case DefineEnumHelper.EquipmentItemKind.Armor:
                _armorDef = (int)data._value;
                break;
            case DefineEnumHelper.EquipmentItemKind.Weapon:
                _weaponDam = (int)data._value;
                break;
            case DefineEnumHelper.EquipmentItemKind.Ring:
                _ringDam = (int)data._value; ;
                break;
            case DefineEnumHelper.EquipmentItemKind.Shouse:
                _shoseDef = (int)data._value;
                break;
        }
        CalculateStat();
    }
    public void CalculateStat()
    {
        CharacterCtrl character = GameManager._instance.character;
        character.FinalDam(_weaponDam + _ringDam);
        character.FinalDef(_helmetDef + _armorDef + _shoseDef);
        UIManager._instance.equipMentWindow.GetComponentInChildren<CharacterInfo>().UpdateInfo(character.finalDam.ToString(), character.finalDef.ToString());

    }
}
