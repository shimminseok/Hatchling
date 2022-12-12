using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StartWindow : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] InputField _id;
    [SerializeField] InputField _pw;
    [SerializeField] Button _signupButton;
    [SerializeField] GameObject _signUpwindow;

    [Header("SignUp")]
    [SerializeField] InputField _signUpid;
    [SerializeField] InputField _signUpPw;
    [SerializeField] InputField _signUpcheakPw;
    [SerializeField] Toggle _showPW;

    [Header("CreateCharacter")]
    [SerializeField] InputField _nickName;

    void Awake()
    {
        InitData();
    }
    void InitData()
    {
        _id.text = string.Empty;
        _pw.text = string.Empty;
        _signUpid.text = string.Empty;
        _signUpPw.text = string.Empty;
        _signUpcheakPw.text = string.Empty;
        SoundManager._instance.PlaYBGM(DefineEnumHelper.CurScene.LoginScene);
        _signupButton.onClick.AddListener(() => ClickSignUpButton());
    }
    void ClickSignUpButton()
    {
        _signUpwindow.SetActive(true);
    }
    public void CheakDuplicateID()
    {
        byte[] id = Encoding.Default.GetBytes(_signUpid.text);
        for (int n = 0; n < id.Length; n++)
        {
            int value = Convert.ToInt32(id[n].ToString());
            if (value > 127)
            {
                UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, transform, "아이디는 영문, 숫자 조합으로만 가능합니다.");
                return;
            }
        }
        ServerManager._instance.Send_DuplicateID(_signUpid.text);
    }
    public void SignUpButton()
    {

        if (!ServerManager._instance._canUseID)
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes,  transform, "아이디 중복확인을 해주세요.");
        }
        else
        {
            if (_signUpPw.text.Equals(_signUpcheakPw.text))
            {
                ServerManager._instance.Send_SignUp(_signUpid.text, _signUpPw.text);
            }
            else
            {
                UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, transform, "비밀번호가 서로 다릅니다.");
            }
        }

    }
    public void LoginButton()
    {
        ServerManager._instance.Send_LoginInfo(_id.text, _pw.text);
    }
    public void CheakDuplicateNickName()
    {
        ServerManager._instance.Send_DuplicateNickName(_nickName.text);
    }
    public void CreateCharacterButton()
    {
        if (_nickName.text.Length <= 5)
        {
            if (ServerManager._instance._canUseNick)
            {
                ServerManager._instance.Send_SetNicName(_nickName.text);
                UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes,transform, "캐릭터 생성이 완료되었습니다.");
                UIManager._instance.ExitWindow(transform.GetChild((int)DefineEnumHelper.StartWindowGetChild.CreateChar).gameObject);
            }
            else
            {
                UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes, transform, "닉네임 중복 확인을 해주세요.");
            }
        }
        else
        {
            UIManager._instance.MessageBox(DefineEnumHelper.MessageBoxKind.Yes,transform, "닉네임은 5글자를 넘을 수 없습니다.");
        }
    }
    public void ReviseString(bool b)
    {
        ServerManager._instance._canUseNick = b;
        ServerManager._instance._canUseID = b;
    }

    public void ShowPassword()
    {
        if(_showPW.isOn)
        {
            _signUpPw.contentType = InputField.ContentType.Standard;
            _signUpcheakPw.contentType = InputField.ContentType.Standard;
            _signUpPw.ActivateInputField();
            _signUpcheakPw.ActivateInputField();
        }
        else
        {
            _signUpPw.contentType = InputField.ContentType.Password;
            _signUpcheakPw.contentType = InputField.ContentType.Password;
            _signUpPw.ActivateInputField();
            _signUpcheakPw.ActivateInputField();
        }
    }
}

