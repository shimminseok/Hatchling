using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectBase : MonoBehaviour
{
    [Header("Object Info")]
    [SerializeField] protected Collider _hitBox;
    [SerializeField] protected string _objName;

    //레벨
    [SerializeField] protected int _lv;
    [SerializeField] protected int _maxHp;
    [SerializeField] protected int _damage; // 원래 데미지;
    [SerializeField] protected int _defense;

    protected bool _isAtt = false;
    protected bool _isDie = false;

    public int finalDam; //최종 합산 데미지...
    public int finalDef; //최종 합산 방어력...

    public int dam => _damage;
    public int def => _defense;
    public int level => _lv;

    public Transform _hitboxTf => _hitBox.transform;
    public GameObject _target { get; set; }
    public string _name => _objName;
    public int maxHp
    {
        get { return _maxHp; }
    }

    public int FinalDam(int addDam) => (finalDam = _damage + addDam);
    public int FinalDef(int addDef) => (finalDef = _defense + addDef);



    public abstract void EndAttack();
    /// <summary>
    /// 공격할때
    /// </summary>
    public abstract void Attack();
    /// <summary>
    /// 맞앗을때
    /// </summary>
    /// <param name="dam"></param>
    public abstract void HittingME(int dam);
    /// <summary>
    /// 공격 콜라이더
    /// </summary>
    /// <param name="n"></param>
    public void ColliderCtrl(int n)
    {
        _hitBox.enabled = true;
        AudioPlay();
    }
    public void AudioPlay()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            MonsterCtrl mon = gameObject.GetComponent<MonsterCtrl>();
            switch((DefineEnumHelper.MonsterKind)mon.id)
            {
                case DefineEnumHelper.MonsterKind.Wolf:
                    SoundManager._instance.SFXSoundPlay(SoundManager._instance.WolfSound(0));
                    break;
                case DefineEnumHelper.MonsterKind.Goblin:
                    SoundManager._instance.SFXSoundPlay(SoundManager._instance.GoblinSound(Random.Range(0, 3)));
                    break;
                case DefineEnumHelper.MonsterKind.HopeGoblin:
                    SoundManager._instance.SFXSoundPlay(SoundManager._instance.HopeGoblinSound(Random.Range(0,3)));
                    break;
                case DefineEnumHelper.MonsterKind.Troll:
                    SoundManager._instance.SFXSoundPlay(SoundManager._instance.TrollSound(0));
                    break;
            }
        }
        else if(gameObject.CompareTag("Player"))
        {
            SoundManager._instance.SFXSoundPlay(SoundManager._instance.PlayAttackSound(0));
        }
    }
}
