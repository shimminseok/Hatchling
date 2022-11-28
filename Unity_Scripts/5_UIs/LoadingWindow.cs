using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWindow : MonoBehaviour
{
    [SerializeField] Image _bg;
    [SerializeField] Slider _loadingBar;
    [SerializeField] Text _txtStaticLoadding;

    int _count = 0;
    float _cheakTime;
    int _limitCount = 5;
    void Start()
    {
        _bg.sprite = ResourcePoolManager._instance.LoadingWindowImage(Random.Range(0,4));
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            _cheakTime += Time.deltaTime;
            if (_cheakTime >= 0.5f)
            {
                _cheakTime = 0;
                _txtStaticLoadding.text = "Loading";
                for (int n = 0; n < _count; n++)
                {
                    _txtStaticLoadding.text += ".";
                }
                if (++_count >= _limitCount)
                {
                    _count = 0;
                }
            }
        }
    }

    public void OpenWindow()
    {
        SetLoaddingProgress(0);
        _txtStaticLoadding.text = "Loading";
        _count++;
    }
    public void SetLoaddingProgress(float pro)
    {
        _loadingBar.value = pro;
        if (_loadingBar.value == 1)
        {
            //gameObject.SetActive(false);
        }
    }
}
