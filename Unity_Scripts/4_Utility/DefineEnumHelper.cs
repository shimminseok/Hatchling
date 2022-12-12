using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefineEnumHelper
{
    //������ ����
    public enum ItemType
    {
        �������� = 1001,
        ����������
    }
    //������ Ÿ��
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
        ����,
        ���ݷ�,
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
