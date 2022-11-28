using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager _uniqueInstance;


    GameObject _mainWindow;
    GameObject _window;

    public static UIManager _instance => _uniqueInstance;
    public bool _isOk { get; private set; }
    public CharacterInfoWindow characterInfoWindow { get; private set; }
    public MonsterWindow monsterWindow { get; private set; }
    public EquipMentWindow equipMentWindow { get; private set; }
    public StartWindow startWindow { get; private set; }
    public CharacterInfo characterinfo { get; private set; }
    GameObject _quitWindow;
    GameObject _optionWindow;

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
    }
    /// <summary>
    /// ¥‹√‡≈∞
    /// </summary>
    /// <returns></returns>
    public IEnumerator InputShortcut()
    {
        while (GameManager._instance._isGameStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    SoundManager._instance.SFXSoundPlay(DefineEnumHelper.SFXSounds.Click);
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Transform root = GameObject.FindGameObjectWithTag("Windows").transform;
                GameQuit(root);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                GameObject inven = equipMentWindow.gameObject;
                _window = inven;
                if (inven.activeSelf)
                {
                    inven.SetActive(false);
                    _window = null;
                }
                else
                    inven.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameObject shop = equipMentWindow.gameObject;
                _window = shop;
                if (shop.activeSelf)
                {
                    shop.SetActive(false);
                    _window = null;
                }
                else
                    shop.SetActive(true);
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return null;
    }
    public void ExitWindow(GameObject obj)
    {
        obj.SetActive(false);
        _window = null;
    }
    public void OpenWindow(GameObject obj)
    {
        _window = obj;
        if (obj.activeSelf)
        {
            obj.SetActive(false);
            _window = null;
            return;
        }
        obj.SetActive(true);
    }

    public void GetCharacterInfoWindow(CharacterInfoWindow window)
    {
        characterInfoWindow = window;
    }
    public void GetMonsterINfoWindow(MonsterWindow window)
    {
        monsterWindow = window;
    }
    public void GetEquipmentWindow(EquipMentWindow window)
    {
        equipMentWindow = window;
    }
    public void GetStartWindow(StartWindow window)
    {
        startWindow = window;
    }
    public IEnumerator CloseMonsterInfoWindow()
    {
        yield return new WaitForSeconds(3);
        monsterWindow.gameObject.SetActive(false);
    }

    public void MessageBox(DefineEnumHelper.MessageBoxKind kind, Transform root, string message)
    {
        GameObject messageBox = Instantiate(ResourcePoolManager._instance.GetMessageBox(kind), root);
        Text text = messageBox.transform.GetChild(0).GetComponent<Text>();
        text.text = message;
        Button okButton = messageBox.transform.GetChild(1).GetComponent<Button>();
        okButton.onClick.AddListener(() => ClickOk(messageBox));
        if (kind.Equals(DefineEnumHelper.MessageBoxKind.Yes_or_No))
        {
            Button NoButton = messageBox.transform.GetChild(2).GetComponent<Button>();
            NoButton.onClick.AddListener(() => ClickCancle(messageBox));
        }

    }
    public void ClickOk(GameObject box)
    {
        _isOk = true;
        Destroy(box);
    }
    public void ClickCancle(GameObject box)
    {
        _isOk = false;
        Destroy(box);
    }

    public void GameQuit(Transform root)
    {
        if (_quitWindow == null)
        {
            _quitWindow = Instantiate(ResourcePoolManager._instance.GetGameQuitBox(), root);
        }
        else
        {
            return;
        }
        Button yes = _quitWindow.transform.GetChild(1).gameObject.GetComponent<Button>();
        Button no = _quitWindow.transform.GetChild(0).gameObject.GetComponent<Button>();
        yes.onClick.AddListener(() => QuitOk());
        no.onClick.AddListener(() => ClickCancle(_quitWindow));
    }
    public void QuitOk()
    {
        if (GameManager._instance.character != null)
        {
            UserInfo._instance.SaveUserPosition(GameManager._instance.character.transform.position);
            UserInfo._instance.UserItemInfo();
            equipMentWindow.CheakMountItem();
            ServerManager._instance.Send_UpdateUserInfo();
        }
        GameManager._instance._isGameStart = false;
#if UNITY_EDITOR
        //GameManager._instance.SceneConttroller("LoginScene");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void OpenOpttionWindow()
    {

        Transform root = GameObject.FindGameObjectWithTag("Windows").transform;
        if (_optionWindow == null)
        {
            _optionWindow = Instantiate(ResourcePoolManager._instance.GetOptionWindow(), root);
        }
        else
        {
            return;
        }
    }
    //
}
