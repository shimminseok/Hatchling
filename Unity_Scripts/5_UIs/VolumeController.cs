using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] Sprite[] _muteImage;
    
    [SerializeField] Slider _bgmController;
    [SerializeField] Slider _sfxController;

    [SerializeField] Toggle _bgm;
    [SerializeField] Toggle _sfx;

    [SerializeField] Text _bgmTxt;
    [SerializeField] Text _sfxTxt;

    [SerializeField] Button _exitButton;

    void Start()
    {
        _exitButton.onClick.AddListener(() => UIManager._instance.ClickCancle(gameObject));
        _bgmController.value = SoundManager._instance.bgmPlayer.volume;
        _sfxController.value = SoundManager._instance.sfxPlayer.volume;

    }
    void Update()
    {
        BGMVolumeControl();
        SFXVolumeControl();
    }
    void BGMVolumeControl()
    {
        SoundManager._instance.BGMVolumeControl(!_bgm.isOn, _bgmController.value);
        _bgmTxt.text = ((int)(_bgmController.value * 100)).ToString();
    }
    void SFXVolumeControl()
    {
        SoundManager._instance.SFXVolumeControl(!_sfx.isOn, _sfxController.value);
        _sfxTxt.text = ((int)(_sfxController.value * 100)).ToString();
    }

    public void ChangeImage()
    {
        if(!_bgm.isOn)
        {
            _bgm.image.sprite = _muteImage[0];
        }
        else
        {
            _bgm.image.sprite = _muteImage[1];

        }
        if (!_sfx.isOn)
        {
            _sfx.image.sprite = _muteImage[0];

        }
        else
        {
            _sfx.image.sprite = _muteImage[1];

        }
    }
}
