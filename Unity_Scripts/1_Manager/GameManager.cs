using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static GameManager _uiniqueInstance;
    [SerializeField] GameObject _character;


    CameraCtrl _cameraCtrl;

    public static GameManager _instance => _uiniqueInstance;

    public CharacterCtrl character { get; private set; }

    public DefineEnumHelper.CurScene _prevScene { get; set; }
    public DefineEnumHelper.CurScene _curScene { get; set; }
    public bool _isGameStart { get; set; }

    void Awake()
    {
        if (_uiniqueInstance == null)
        {
            _uiniqueInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        StartScene();
        StartCoroutine(UIManager._instance.InputShortcut());

    }
    public void StartScene()
    {
        _isGameStart = true;
    }
    public void IngameScene()
    {
        GameObject go = Instantiate(_character);
        character = go.GetComponent<CharacterCtrl>();
        _cameraCtrl = Camera.main.GetComponent<CameraCtrl>();
        _cameraCtrl._targetTr = character.transform.GetChild(0).gameObject;
    }
    public void SceneConttroller(string SceneName)
    {
        StartCoroutine(LoaddingScene(SceneName));
    }
    IEnumerator LoaddingScene(string sceneName)
    {
        GameObject go = Instantiate(ResourcePoolManager._instance.GetWindowObject(DefineEnumHelper.WindowKind.LoadingWindow));
        LoadingWindow wnd = go.GetComponent<LoadingWindow>();
        wnd.OpenWindow();
        SoundManager._instance.PlaYBGM(DefineEnumHelper.CurScene.LoadingScene);
        yield return new WaitForSeconds(1);
        AsyncOperation aOper = SceneManager.LoadSceneAsync(sceneName);
        while (!aOper.isDone)
        {
            wnd.SetLoaddingProgress(aOper.progress);
            yield return null;
        }
        wnd.SetLoaddingProgress(aOper.progress);
        Scene _curScene = SceneManager.GetActiveScene();
        if(_curScene.name.Equals("LoginScene"))
        {
            LoginScene();
        }
        else if (_curScene.name.Equals("InGameScene"))
        {
            InGameScene();
        }
        yield return new WaitForSeconds(1);
        while (wnd != null)
        {
            yield return null;
        }
    }
    void LoginScene()
    {
        UIManager._instance.startWindow.gameObject.SetActive(true);
        UIManager._instance.characterInfoWindow.gameObject.SetActive(false);
        UIManager._instance.equipMentWindow.transform.parent.gameObject.SetActive(false);
    }
    void InGameScene()
    {
        UIManager._instance.startWindow.gameObject.SetActive(false);
        UIManager._instance.characterInfoWindow.gameObject.SetActive(true);
        UIManager._instance.equipMentWindow.transform.parent.gameObject.SetActive(true);
        GameObject monterRoot = GameObject.FindGameObjectWithTag("MonsterRoot");
        for (int n = 0; n < monterRoot.transform.childCount; n++)
        {
            ObjectPoolingManager._instance.rootPoint.Add(monterRoot.transform.GetChild(n));
        }
        ObjectPoolingManager._instance.Initialize(50);
    }
}
