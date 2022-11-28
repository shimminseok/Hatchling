using DefineServerUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public struct stSocket
{
    public long _uuid;
    public stUserInfo _userInfo;

    public Socket _client;

    public stSocket(long uu, Socket socket)
    {
        _uuid = uu;
        _userInfo = new stUserInfo();
        _client = socket;

    }
    public stSocket(long uu, stUserInfo userinfo, Socket socket)
    {
        _uuid = uu;
        _userInfo = userinfo;
        _client = socket;
    }
}

class ServerManager
{
    short _port;
    public static long _nowUUID = 10000000000;
    bool _isEnd = false;


    Queue<Packet> _sendQ = new Queue<Packet>();
    Queue<Packet> _receiveQ = new Queue<Packet>();

    Dictionary<long, stSocket> _clients = new Dictionary<long, stSocket>();

    Socket _waitServer;
    Thread _sendThread;
    Thread _receiveThread;

    DBController _db;

    public bool _isRun
    { get; set; }

    public ServerManager(short port, DBController db)
    {
        _port = port;
        _db = db;
        try
        {
            _waitServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _waitServer.Bind(new IPEndPoint(IPAddress.Any, _port));
            _waitServer.Listen(ConvertPacketFunc._maxPerson);
            Console.WriteLine("소켓 생성 성공");
        }
        catch (Exception ex)
        {
            Console.WriteLine("소켓 생성 실패");
            Console.WriteLine(ex.Message);

            return;
        }
        _isEnd = false;
        _sendThread = new Thread(() => SendProcess());
        _receiveThread = new Thread(() => ReceiveProcess());

        _sendThread.Start();
        _receiveThread.Start();
    }
    ~ServerManager()
    {
        ReleaseServer();
    }
    public void ReleaseServer()
    {

    }
    public void CloseSocketProcess()
    {
        foreach (long key in _clients.Keys)
        {
            if (_clients[key]._client == null)
            {
                //remove
            }
        }
    }
    public bool MainProcess()
    {
        if (_waitServer.Poll(0, SelectMode.SelectRead))
        {
            Console.WriteLine("서버에 클라가 접속했습니다.");
            stSocket add = new stSocket(++_nowUUID, _waitServer.Accept());
            _clients.Add(add._uuid, add);
            Send_UUID(add._uuid);
        }
        // 리시브 받은걸 처리
        while (_receiveQ.Count > 0)
        {
            Packet recv = _receiveQ.Dequeue();
            switch ((eReceiveMessage)recv._protocolID)
            {
                case eReceiveMessage.DuplicateID:
                    Receive_DuplicateID(recv);
                    break;
                case eReceiveMessage.UserSignUpInfo:
                    Receive_SignUpUserInfo(recv);
                    break;
                case eReceiveMessage.UserLoginInfo:
                    Receive_LoginUserInfo(recv);
                    break;
                case eReceiveMessage.DuplicateNick:
                    Receive_DuplicateNickName(recv);
                    break;
                case eReceiveMessage.InsertNickName:
                    Receive_UpdateNickName(recv);
                    break;
                case eReceiveMessage.UpdateUserInfo:
                    Receive_UpdateUserInfo(recv);
                    break;
            }
        }
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keys = Console.ReadKey(true);
            if (keys.Key == ConsoleKey.Escape)
            {
                return false;
            }
        }
        return true;
    }
    void SendProcess()
    {
        while (!_isEnd)
        {
            if (_sendQ.Count > 0)
            {
                Packet pack = _sendQ.Dequeue();
                byte[] buffer = ConvertPacketFunc.StructureToByteArray(pack);
                if (_clients.ContainsKey(pack._targetID))
                {
                    _clients[pack._targetID]._client.Send(buffer);
                }
            }
            Thread.Sleep(20);
        }
    }
    // 보내는걸 받아서 receiveQ에 넣음
    void ReceiveProcess()
    {
        while (!_isEnd)
        {
            foreach (long uuid in _clients.Keys)
            {
                if (_clients[uuid]._client != null && _clients[uuid]._client.Poll(0, SelectMode.SelectRead))
                {
                    byte[] buffer = new byte[ConvertPacketFunc._maxByte];
                    try
                    {
                        int recvLength = _clients[uuid]._client.Receive(buffer);
                        if (recvLength > 0)
                        {
                            Packet pack = (Packet)ConvertPacketFunc.ByteArrayToStructure(buffer, typeof(Packet), buffer.Length);
                            _receiveQ.Enqueue(pack);
                        }
                        else
                            continue;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("리시브 실패!");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            Thread.Sleep(20);
        }
    }

    void Send_UUID(long uuid)
    {
        SendUUID sendUUID;
        sendUUID._uuid = uuid;
        byte[] data = ConvertPacketFunc.StructureToByteArray(sendUUID);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.SendUUID, uuid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    void Send_DuplicateID(long uuid, string id, bool result)
    {
        Send_DuplicateIDResult _result;
        _result._isSuccess = result;
        _result._id = id;
        byte[] data = ConvertPacketFunc.StructureToByteArray(_result);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.DuplicateID, uuid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    void Send_SignUPResult(long uuid, bool result)
    {
        Send_SignUpResult _result;
        _result._isSuccess = result;
        byte[] data = ConvertPacketFunc.StructureToByteArray(_result);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.SignUpResult, uuid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    void Send_LoginUserInfo(long uuid, bool login, bool character, string id, string pw)
    {
        Send_LoginInfo info;
        info._loginSucess = login;
        info._hasCharacter = character;
        info._id = id;
        info._pw = pw;
        byte[] data = ConvertPacketFunc.StructureToByteArray(info);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.UserLoginInfo, uuid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    void Send_DuplicateNickResult(long uuid, bool result, string nick)
    {
        Send_DuplicateResult _result;
        _result._result = result;
        _result._nick = nick;
        byte[] data = ConvertPacketFunc.StructureToByteArray(_result);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.DuplicateNick, uuid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    void Send_UserInfo(long uuid, stUserInfo info)
    {
        Send_UserInfo userInfo;
        userInfo._nick = info._nickName;
        userInfo._lv = (short)info._level;
        userInfo._hp = info._hp;
        userInfo._mp = info._mp;
        userInfo._money = info._curMoney;
        userInfo._ex = info._curEx;
        userInfo._x = info._pos[0];
        userInfo._y = info._pos[1];
        userInfo._z = info._pos[2];
        userInfo._itemInfo = info._inventory;
        userInfo._mountItem = info._mountItem;
        byte[] data = ConvertPacketFunc.StructureToByteArray(userInfo);
        Packet pack = ConvertPacketFunc.CreatePack((int)eSendMessage.UserInfo, uuid, data.Length, data);
        _sendQ.Enqueue(pack);
    }
    // 리시브
    void Receive_DuplicateID(Packet recv)
    {
        Receive_DuplicateID id = (Receive_DuplicateID)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(Receive_DuplicateID), recv._totalSize);
        if (_clients.TryGetValue(recv._targetID, out stSocket socket))
        {
            socket._userInfo._id = id._id;
            bool result = _db.GetUserInfo(eReceiveMessage.DuplicateID, socket._userInfo, "hachling_userinfo.signupinfo", out stUserInfo _userinfo);
            Send_DuplicateID(recv._targetID, id._id, result);
        }
    }
    void Receive_SignUpUserInfo(Packet recv)
    {
        ReceiveUserSginUpInfo signUpinfo = (ReceiveUserSginUpInfo)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(ReceiveUserSginUpInfo), recv._totalSize);
        Send_SignUPResult(recv._targetID, _db.Insert(signUpinfo._id, signUpinfo._pw));
    }
    void Receive_LoginUserInfo(Packet recv)
    {
        Receive_LoginInfo userinfo = (Receive_LoginInfo)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(Receive_LoginInfo), recv._totalSize);
        if (_clients.TryGetValue(recv._targetID, out stSocket socket))
        {
            socket._userInfo._id = userinfo._id;
            socket._userInfo._pw = userinfo._pw;
            socket._userInfo._nickName = string.Empty;
            bool result = !_db.GetUserInfo(eReceiveMessage.UserLoginInfo, socket._userInfo, "hachling_userinfo.signupinfo", out stUserInfo _userinfo);
            bool haschar = false;
            if (result)
            {
                // 로그인 성공
                if (_userinfo._nickName.Equals(string.Empty))
                {
                    //캐릭터가 없는것
                    haschar = false;

                }
                else
                {
                    //캐릭터가 존재
                    haschar = true;
                    _db.GetUserInfo(eReceiveMessage.UserInfo, _userinfo, "hachling_userinfo.userinfo", out stUserInfo info);
                    info._inventory = null;
                    _userinfo = info;
                    _db.GetUserInfo(eReceiveMessage.UpdateItemInfo, _userinfo, "hachling_userinfo.inventory", out info);
                    _userinfo._inventory = info._inventory;
                    _db.GetUserInfo(eReceiveMessage.UpdateMountItem, _userinfo, "hachling_userinfo.mountitem", out info);
                    _userinfo._mountItem = info._mountItem;
                    socket._userInfo = _userinfo;
                    Send_UserInfo(recv._targetID, _userinfo);

                }
            }
            Send_LoginUserInfo(recv._targetID, result, haschar, socket._userInfo._id, socket._userInfo._pw);
        }
    }
    void Receive_DuplicateNickName(Packet recv)
    {
        Receive_DuplicateNick nick = (Receive_DuplicateNick)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(Receive_DuplicateNick), recv._totalSize);
        if (_clients.TryGetValue(recv._targetID, out stSocket socket))
        {
            socket._userInfo._nickName = nick._nick;
            bool result = _db.GetUserInfo(eReceiveMessage.DuplicateNick, socket._userInfo, "hachling_userinfo.signupinfo", out stUserInfo _userinfo);
            Send_DuplicateNickResult(recv._targetID, result, socket._userInfo._nickName);
        }
    }
    void Receive_UpdateNickName(Packet recv)
    {
        Receive_UpdateNickName insertNick = (Receive_UpdateNickName)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(Receive_UpdateNickName), recv._totalSize);
        stUserInfo userinfo;
        if (_clients.TryGetValue(recv._targetID, out stSocket socket))
        {
            userinfo = socket._userInfo;
            userinfo._id = insertNick._id;
            userinfo._nickName = insertNick._nick;

            _db.UpdateData(eReceiveMessage.InsertNickName, "hachling_userinfo.signupinfo", userinfo);
        }
        _db.Insert(insertNick._nick);
    }
    void Receive_UpdateUserInfo(Packet recv)
    {
        Receive_Update updateUserInfo = (Receive_Update)ConvertPacketFunc.ByteArrayToStructure(recv._datas, typeof(Receive_Update), recv._totalSize);
        stUserInfo userinfo;
        if (_clients.TryGetValue(recv._targetID, out stSocket socket))
        {
            userinfo = socket._userInfo;
            userinfo._nickName = updateUserInfo._nick;
            userinfo._level = updateUserInfo._lv;
            userinfo._hp = updateUserInfo._hp;
            userinfo._mp = updateUserInfo._mp;
            userinfo._curMoney = updateUserInfo._money;
            userinfo._curEx = updateUserInfo._ex;
            float[] pos = { updateUserInfo._x, updateUserInfo._y, updateUserInfo._z };
            userinfo._pos = pos;
            userinfo._inventory = updateUserInfo._itemInfo;
            userinfo._mountItem = updateUserInfo._mountItem;
            _db.UpdateData(eReceiveMessage.UpdateUserInfo, "hachling_userinfo.userinfo", userinfo);
            _db.UpdateData(eReceiveMessage.UpdateUserInfo, "hachling_userinfo.inventory", userinfo);
            _db.UpdateData(eReceiveMessage.UpdateUserInfo, "hachling_userinfo.mountitem", userinfo);
        }
    }
}
