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
        yield return new WaitForSeconds(1);
        while (wnd != null)
        {
            yield return null;
        }
    }
}
