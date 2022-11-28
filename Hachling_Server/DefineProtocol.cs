using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefineServerUtility
{
    public enum eSendMessage
    {
        SendUUID,
        DuplicateID,
        SignUpResult,
        UserSignUpInfo,
        UserLoginInfo,
        DuplicateNick,
        InsertNickName,
        UserInfo,
        UpdateUserInfo,
        UpdateItemInfo,
        UpdateMountItem,
    }
    public enum eReceiveMessage
    {
        GetUUID,
        DuplicateID,
        SignUpResult,
        UserSignUpInfo,
        UserLoginInfo,
        DuplicateNick,
        InsertNickName,
        UserInfo,
        UpdateUserInfo,
        UpdateItemInfo,
        UpdateMountItem,

    }
}
