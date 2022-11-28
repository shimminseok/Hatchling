using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DefineServerUtility
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Packet
    {
        // 프로토콜 넘버.
        [MarshalAs(UnmanagedType.U4)] public int _protocolID;
        // _data에 들어가는 구조체의 실질 메모리 크기.
        [MarshalAs(UnmanagedType.U2)] public short _totalSize;
        // 신호를 받을 주체
        [MarshalAs(UnmanagedType.U8)] public long _targetID;
        // 실제 정보 구조체
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1002)] public byte[] _datas;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct SendUUID
    {
        [MarshalAs(UnmanagedType.U8)] public long _uuid;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Send_DuplicateIDResult
    {
        [MarshalAs(UnmanagedType.Bool)] public bool _isSuccess;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] public string _id;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Send_SignUpResult
    {
        [MarshalAs(UnmanagedType.Bool)] public bool _isSuccess;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Send_LoginInfo
    {
        [MarshalAs(UnmanagedType.Bool)] public bool _loginSucess;
        [MarshalAs(UnmanagedType.Bool)] public bool _hasCharacter;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] public string _id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] public string _pw;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Send_DuplicateResult
    {
        [MarshalAs(UnmanagedType.Bool)] public bool _result;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] public string _nick;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Send_UserInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] public string _nick;
        [MarshalAs(UnmanagedType.U2)] public short _lv;
        [MarshalAs(UnmanagedType.U4)] public int _hp;
        [MarshalAs(UnmanagedType.U4)] public int _mp;
        [MarshalAs(UnmanagedType.U4)] public int _money;
        [MarshalAs(UnmanagedType.U4)] public int _ex;
        [MarshalAs(UnmanagedType.R4)] public float _x;
        [MarshalAs(UnmanagedType.R4)] public float _y;
        [MarshalAs(UnmanagedType.R4)] public float _z;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 168)] public byte[] _itemInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] public byte[] _mountItem;

    }
    public struct Send_ItemData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] public string _nick;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 168)] public byte[] _itemInfo;
    }
    // 리시브
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Receive_DuplicateID
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] public string _id;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct ReceiveUserSginUpInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] public string _id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] public string _pw;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Receive_LoginInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] public string _id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] public string _pw;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Receive_DuplicateNick
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] public string _nick;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Receive_UpdateNickName
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)] public string _id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] public string _nick;
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Receive_Update
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] public string _nick;
        [MarshalAs(UnmanagedType.U2)] public short _lv;
        [MarshalAs(UnmanagedType.U4)] public int _hp;
        [MarshalAs(UnmanagedType.U4)] public int _mp;
        [MarshalAs(UnmanagedType.U4)] public int _money;
        [MarshalAs(UnmanagedType.U4)] public int _ex;
        [MarshalAs(UnmanagedType.R4)] public float _x;
        [MarshalAs(UnmanagedType.R4)] public float _y;
        [MarshalAs(UnmanagedType.R4)] public float _z;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 168)] public byte[] _itemInfo;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] public byte[] _mountItem;

    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 1)]
    public struct Receive_ItemData
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] public string _nick;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 168)] public byte[] _itemInfo;
    }
    public struct Receive_MountItem
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 10)] public string _nick;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)] public byte[] _mountItem;
    }
}
