using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineEnumHelper
{
    //아이템 종류
    public enum ItemType
    {
        사용아이템 = 1001,
        장착아이템
    }
    //아이템 타입
    public enum EquipmentItemKind
    {
        Helmet,
        Armor,
        Weapon,
        Ring,
        Shouse
    }
    public enum UsedItemKind
    {
        HP_Portion,
        MP_Portion
    }
    public enum StatKind
    {
        방어력,
        공격력,
        HP,
        MP
    }
    public enum MonsterState
    {
        Idle,
        Die,
        Attack,
        TargetOff,
        TargetOn
    }
    public enum WindowKind
    {
        StartWindow,
        LoadingWindow,
        CharacterInfoWindow,
        TouchWindow,
        MonsterInfoWindow
    }

    public enum MonsterKind
    {
        Wolf,
        Goblin,
        HopeGoblin,
        Troll
    }
    public enum CurScene
    {
        LoadingScene,
        LoginScene,
        InGameScene,
        RaidScene
    }
    public enum ButtonType
    {
        SceneController,
        CheackBox,
        MessageBox
    }
    public enum MessageBoxKind
    {
        Yes,
        Yes_or_No,
    }
    public enum StartWindowGetChild
    {
        Main,
        SignUp,
        CreateChar,
        Option,
    }

    public enum ClickButtonType
    {
        None,
        Quit,
    }

    public enum SFXSounds
    {
        MountEquipment,
        UsePortion,
        LevelUp,
        Click
    }
}
