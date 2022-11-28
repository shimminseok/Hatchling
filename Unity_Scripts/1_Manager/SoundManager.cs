using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager _uniqueInstance;
    public static SoundManager _instance => _uniqueInstance;

    [SerializeField] AudioSource _bgmPlayer;
    [SerializeField] AudioSource _sfxPlayer;

    [SerializeField] AudioClip[] _bgmSound;
    [SerializeField] AudioClip[] _sfxSounds;
    [SerializeField] AudioClip[] _earthStepSound;
    [SerializeField] AudioClip[] _characterSound;
    [SerializeField] AudioClip[] _wolfSound;
    [SerializeField] AudioClip[] _goblinSound;
    [SerializeField] AudioClip[] _hopeGoblinSound;
    [SerializeField] AudioClip[] _trollSound;



    public AudioSource bgmPlayer => _bgmPlayer;
    public AudioSource sfxPlayer => _sfxPlayer;
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
    public void PlaYBGM(DefineEnumHelper.CurScene scene)
    {
        _bgmPlayer.clip = _bgmSound[(int)scene];
        _bgmPlayer.Play();
    }
    public AudioClip PlayEaethSound(int index)
    {
        return _earthStepSound[index];
    }
    public AudioClip PlayAttackSound(int index)
    {
        return _characterSound[index];
    }
    public AudioClip WolfSound(int index)
    {
        return _wolfSound[index];
    }
    public AudioClip GoblinSound(int index)
    {
        return _goblinSound[index];
    }
    public AudioClip HopeGoblinSound(int index)
    {
        return _hopeGoblinSound[index];
    }
    public AudioClip TrollSound(int index)
    {
        return _trollSound[index];
    }

    public void SFXSoundPlay(DefineEnumHelper.SFXSounds kind)
    {
        _sfxPlayer.PlayOneShot(_sfxSounds[(int)kind]);
    }
    public void SFXSoundPlay(AudioClip audio)
    {
        _sfxPlayer.PlayOneShot(audio);
    }
    public void BGMVolumeControl(bool mute,float volume)
    {
        _bgmPlayer.mute = mute;
        _bgmPlayer.volume = volume;
    }
    public void SFXVolumeControl(bool mute,float volume)
    {
        _sfxPlayer.mute = mute;
        _sfxPlayer.volume = volume;
    }


}
