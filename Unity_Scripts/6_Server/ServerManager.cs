using DefineServerUtility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    static ServerManager _uniqueInstance;

    public static ServerManager _instance => _uniqueInstance;

    const string _ip = "127.0.0.1";
    const short _port = 80;

    Socket _sock;
    Queue<Packet> _sendQ = new Queue<Packet>();
    Queue<Packet> _recvQ = new Queue<Packet>();

    bool _isConnectFaild = false;
    int _retryCount = 3;

    public bool _canUseID { get; set; }
    public bool _canUseNick { get; set; }
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
        NetConnect();
    }
    void Update()
    {
        if (_sock != null && _sock.Connected)
        {
            if (_sock.Poll(0, SelectMode.SelectRead))
            {
                try
                {
                byte[] buffer = new byte[ConvertPacketFunc._maxByte];
                int recvLength = _sock.Receive(buffer);
                    if (recvLength > 0)
                    {
                        Packet recv = (Packet)ConvertPacketFunc.ByteArrayToStructure(buffer, typeof(Packet), buffer.Length);
                        _recvQ.Enqueue(recv);
                    }
                }
                catch
                {
                    UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, UIManager._instance.monsterWindow.transform.root, "서버와 연결이 끊겼습니다.");
                }
            }
        }
        if (_isConnectFaild)
        {
            Debug.Log("서버가.............");
        }
        else
        {
            //UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, UIManager._instance.startWindow.transform, "서버와 연결 중 입니다.");
        }
    }
    public void NetConnect()
    {
        StartCoroutine(Connectings(_ip, _port));
        StartCoroutine(SendProcess());
        StartCoroutine(ReceiveProcess());
    }
    IEnumerator Connectings(string ipAddr, short port)
    {
        int cnt = 0;
        while (true)
        {
            yield return new WaitForSeconds(1);
            try
            {
                _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _sock.Connect(ipAddr, port);
                break;
            }
            catch
            {
                cnt++;
                if (cnt > _retryCount)
                {
                    _isConnectFaild = true;
                    UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, UIManager._instance.startWindow.transform, "서버를 연결할 수 없습니다.");
                    break;
                }
            }
            yield return new WaitForSeconds(2);
        }
    }
    IEnumerator SendProcess()
    {
        while (true)
        {
            if (_sendQ.Count > 0)
            {
                Packet send = _sendQ.Dequeue();
                byte[] data = ConvertPacketFunc.StructureToByteArray(send);
                _sock.Send(data);
            }
            else
            {
                yield return null;
            }
        }
    }
    IEnumerator ReceiveProcess()
    {
        while (true)
        {
            if (_recvQ.Count > 0)
            {
                Packet packet = _recvQ.Dequeue();
                switch ((eReceiveMessage)packet._protocolID)
                {
                    case eReceiveMessage.GetUUID:
                        GetUUID(packet);
                        break;
                    case eReceiveMessage.DuPlicateID:
                        Receive_DuplicateResult(packet);
                        break;
                    case eReceiveMessage.SignUpResult:
                        Receive_SignResult(packet);
                        break;
                    case eReceiveMessage.UserLoginInfo:
                        Receive_LoginUserInfo(packet);
                        break;
                    case eReceiveMessage.DuplicateNickName:
                        Receive_DuplicateNickName(packet);
                        break;
                    case eReceiveMessage.UserInfo:
                        Receive_UserInfo(packet);
                        break;
                }
            }
            else
            {
                yield return null;
            }
        }
    }
    #region[Sendprocessing Func]
    public void Send_DuplicateID(string id)
    {
        Send_DuplicateID userid;
        userid._id = id;
        byte[] data = ConvertPacketFunc.StructureToByteArray(userid);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.DuPlicateID, UserInfo._instance._myUUid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    public void Send_SignUp(string id, string pw)
    {
        Send_SignUpInfo signupInfo;
        signupInfo._id = id;
        signupInfo._pw = pw;
        byte[] data = ConvertPacketFunc.StructureToByteArray(signupInfo);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.UserSignUpInfo, UserInfo._instance._myUUid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    public void Send_LoginInfo(string id, string pw)
    {
        Send_LoginInfo info;
        info._id = id;
        info._pw = pw;
        byte[] data = ConvertPacketFunc.StructureToByteArray(info);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.UserLoginInfo, UserInfo._instance._myUUid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    public void Send_DuplicateNickName(string nick)
    {
        Send_DuplicateNickName _nick;
        _nick._nick = nick;
        byte[] data = ConvertPacketFunc.StructureToByteArray(_nick);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.DuplicateNickName, UserInfo._instance._myUUid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    public void Send_SetNicName(string nick)
    {
        Send_InsertNickName _setNick;
        _setNick._id = UserInfo._instance._id;
        _setNick._nick = nick;
        byte[] data = ConvertPacketFunc.StructureToByteArray(_setNick);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.InsertNickName, UserInfo._instance._myUUid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    public void Send_UpdateUserInfo()
    {
        Send_UserInfo userInfo;
        userInfo._nick = UserInfo._instance._nickName;
        userInfo._lv = (short)UserInfo._instance._level;
        userInfo._hp = UserInfo._instance._curHP;
        userInfo._mp = UserInfo._instance._curMP;
        userInfo._money = UserInfo._instance._money;
        userInfo._ex = UserInfo._instance._curEx;
        userInfo._x = UserInfo._instance._curPos.x;
        userInfo._y = UserInfo._instance._curPos.y;
        userInfo._z = UserInfo._instance._curPos.z;
        userInfo._itemInfo = UserInfo._instance._itemdata;
        userInfo._mountItem = UserInfo._instance._mountItemData;
        byte[] data = ConvertPacketFunc.StructureToByteArray(userInfo);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.UpdateUserInfo, UserInfo._instance._myUUid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    #endregion[Sendprocessing Func]

    #region[Recvprocessing Func]
    void GetUUID(Packet recv)
    {
        GetUUID _uuid = (GetUUID)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(GetUUID), recv._totalSize);
        UserInfo._instance.SetUUID(_uuid._uuid);
        Debug.Log(UserInfo._instance._myUUid);
    }
    public void Receive_DuplicateResult(Packet recv)
    {
        Receive_DuplicateID result = (Receive_DuplicateID)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(Receive_DuplicateID), recv._totalSize);
        _canUseID = result._isSuccess;
        if(_canUseID)
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes_or_No, UIManager._instance.startWindow.transform, string.Format("{0} 은/는 사용 가능한 아이디입니다. 사용하시겠습니까?",result._id));
        }
        else
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, UIManager._instance.startWindow.transform, string.Format("중복된 아이디가 존재합니다."));
        }
    }
    void Receive_SignResult(Packet recv)
    {
        SignUpResult result = (SignUpResult)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(SignUpResult), recv._totalSize);
        //회원가입 성공
        if (result._isSuccess)
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, UIManager._instance.startWindow.transform, "회원가입 성공");
            UIManager._instance.startWindow.transform.GetChild((int)DefineEnumHelper.StartWindowGetChild.SignUp).gameObject.SetActive(false);
        }
        //회원가입 실패
        else
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, UIManager._instance.startWindow.transform, "회원가입 실패");
        }
    }
    void Receive_LoginUserInfo(Packet recv)
    {
        Receive_LoginInfo result = (Receive_LoginInfo)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(Receive_LoginInfo), recv._totalSize);
        if(result._loginSucess)
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, UIManager._instance.startWindow.transform, "로그인 성공");
            UserInfo._instance.SetID(result._id);
            if (result._hasCharacter)
            {
                GameManager._instance.SceneConttroller("IngameScene");
            }
            else
            {
                UIManager._instance.startWindow.transform.GetChild((int)DefineEnumHelper.StartWindowGetChild.CreateChar).gameObject.SetActive(true);
            }
        }
        else
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, UIManager._instance.startWindow.transform, "로그인 실패");
        }
    }
    void Receive_DuplicateNickName(Packet recv)
    {
        Receive_DuplicateNick nick = (Receive_DuplicateNick)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(Receive_DuplicateNick), recv._totalSize);
        _canUseNick = nick._result;
        if (_canUseNick)
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes_or_No, UIManager._instance.startWindow.transform, string.Format("{0} 은/는 사용 가능한 아이디입니다. 사용하시겠습니까?", nick._nick));
        }
        else
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes_or_No,UIManager._instance.startWindow.transform, string.Format("사용할 수 없는 닉네임 입니다.", nick._nick));
        }
    }
    void Receive_UserInfo(Packet recv)
    {
        Receive_UserInfo userinfo = (Receive_UserInfo)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(Receive_UserInfo), recv._totalSize);
        UserInfo._instance.SetUserInfo(userinfo._nick, userinfo._lv, userinfo._hp, userinfo._mp, userinfo._money, userinfo._ex);
        Vector3 pos = new Vector3(userinfo._x, userinfo._y, userinfo._z);
        //인벤토리에 넣어줘야함..
        UserInfo._instance.SaveUserPosition(pos);
        UserInfo._instance.GetItemData(userinfo._itemInfo);
        UserInfo._instance.GetAmountData(userinfo._mountItem);
    }
    #endregion[Recvprocessing Func]
}
