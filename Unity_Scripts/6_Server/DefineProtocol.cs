using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineServerUtility
{
    public enum eSendMessage
    {
        SendUUID,
        DuPlicateID,
        SignUpResult,
        UserSignUpInfo,
        UserLoginInfo,
        DuplicateNickName,
        InsertNickName,
        UserInfo,
        UpdateUserInfo,
        UpdateItemInfo,
        UpdateMountItem,
    }
    public enum eReceiveMessage
    {
        GetUUID,
        DuPlicateID,
        SignUpResult,
        UserSignUpInfo,
        UserLoginInfo,
        DuplicateNickName,
        InsertNickName,
        UserInfo,
        UpdateUserInfo,
        UpdateItemInfo,
        UpdateMountItem,
    }
}
