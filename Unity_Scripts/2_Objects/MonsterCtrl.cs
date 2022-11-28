using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MonsterCtrl : ObjectBase
{
    [Header("몬스터 정보")]
    [SerializeField] DefineEnumHelper.MonsterKind _id;
    [SerializeField] GameObject _infoUI;
    [SerializeField] Transform _bloodPoint;
    float _attDis = 1;
    float _speed = 1.5f;
    int _ex = 100;

    int _minmoney;
    int _maxmoney;
    bool _isarrive;

    NavMeshAgent _navAgent;
    Animator _animator;
    CharacterCtrl _player;

    public DefineEnumHelper.MonsterState _state = DefineEnumHelper.MonsterState.Idle;
    Vector3 _originPos;
    Vector3 _goalPos;

    float _curhp;
    float _removeTime;
    public float _curHp => _curhp;
    public int _experience => _ex;
    public int id => (int)_id;
    public Transform infoUI => _infoUI.transform;
    void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }
    void Start()
    {
        InitData(_id);
    }
    private void OnEnable()
    {
        _isDie = false;
        _originPos = transform.position;
        _state = DefineEnumHelper.MonsterState.Idle;
        GetComponent<CapsuleCollider>().enabled = true;
    }
    void Update()
    {
        if (_curhp <= 0)
        {
            _state = DefineEnumHelper.MonsterState.Die;
        }
        switch (_state)
        {
            case DefineEnumHelper.MonsterState.Idle:
                StartCoroutine(NextGoalPos());
                break;
            case DefineEnumHelper.MonsterState.Die:
                {
                    _infoUI.gameObject.SetActive(false);
                    _player._target = null;
                    GetComponent<CapsuleCollider>().enabled = false;
                    if (!_isDie)
                    {
                        DropMoney(_minmoney, _maxmoney);
                        _animator.SetTrigger("dead");
                        _player._curex += _ex;
                        switch (_id)
                        {
                            case DefineEnumHelper.MonsterKind.Wolf:
                                SoundManager._instance.SFXSoundPlay(SoundManager._instance.WolfSound(1));
                                break;
                            case DefineEnumHelper.MonsterKind.Goblin:
                                SoundManager._instance.SFXSoundPlay(SoundManager._instance.GoblinSound(3));
                                break;
                            case DefineEnumHelper.MonsterKind.HopeGoblin:
                                SoundManager._instance.SFXSoundPlay(SoundManager._instance.HopeGoblinSound(3));
                                break;
                            case DefineEnumHelper.MonsterKind.Troll:
                                SoundManager._instance.SFXSoundPlay(SoundManager._instance.TrollSound(1));
                                break;
                        }

                    }
                    _isDie = true;
                    _navAgent.isStopped = true;
                    _removeTime += Time.deltaTime;
                    if (_removeTime > 2)
                    {
                        _removeTime = 0;
                        ObjectPoolingManager._instance.ReturnObj(this);
                        ObjectManager._instance.Remove(gameObject);
                        _state = DefineEnumHelper.MonsterState.Idle;
                    }
                }
                break;
            case DefineEnumHelper.MonsterState.TargetOff:
                if(_navAgent.remainingDistance < _attDis)
                {
                    _isarrive = true;
                    _goalPos = Vector3.zero;
                    _state = DefineEnumHelper.MonsterState.Idle;
                    _animator.SetFloat("SetBlend", 0);
                }
                else
                {
                    _navAgent.speed = _speed;
                }
                break;
            case DefineEnumHelper.MonsterState.TargetOn:
                if (Vector3.Distance(transform.position, _player.transform.position) > 50)
                {
                    _curhp = _maxHp;
                    _state = DefineEnumHelper.MonsterState.Idle;
                }
                else if (Vector3.Distance(transform.position, _player.transform.position) > _navAgent.stoppingDistance && !_isAtt)
                {

                    _animator.SetFloat("SetBlend", 2);
                    _navAgent.SetDestination(_player.transform.position);
                    _navAgent.isStopped = false;
                    _navAgent.speed = _speed * 2;
                }
                else
                {
                    Attack();
                }
                break;
        }
        Debug.DrawRay(transform.position, _goalPos, Color.red);
    }
    IEnumerator NextGoalPos()
    {
        yield return new WaitForSeconds(1);
        _state = DefineEnumHelper.MonsterState.TargetOff;
        yield return new WaitForSeconds(UnityEngine.Random.Range(1, 6));
        _animator.SetFloat("SetBlend", 1);
        if (_goalPos.Equals(Vector3.zero))
        {
            _navAgent.SetDestination(ObjectPoolingManager._instance.RandomNavSphere(_originPos, 20, out _goalPos));
        }
    }
    public void DropMoney(int min, int max)
    {
        int money = UnityEngine.Random.Range(min, max + 1);
        _player._money += money;
    }
    public override void Attack()
    {
        _navAgent.isStopped = true;
        if (!_isAtt)
        {
            transform.LookAt(_target.transform);
        }
        _isAtt = true;
        _animator.SetFloat("SetBlend", 0);
        _animator.SetBool("attack", true);
    }
    public override void EndAttack()
    {
        _isAtt = false;
        _animator.SetBool("attack", false);
    }
    public void InitData(DefineEnumHelper.MonsterKind id)
    {
        DataTableSt.stMonsterInitData mon = new DataTableSt.stMonsterInitData();
        _isarrive = false;
        if (DataTableManager._instance._monsterDic.TryGetValue((int)id, out mon))
        {
            _objName = mon._name;
            _lv = UnityEngine.Random.Range(mon._lv, mon._lv + 3);
            _maxHp = (int)((0.15 * mon._hp * _lv) + mon._hp);
            _ex = mon._ex * _lv + _lv;
            finalDam = _damage = (int)(Mathf.Log(_lv) + _lv + mon._dam);
            finalDef = _defense = (int)(Mathf.Log(_lv) + _lv + mon._def);
            _navAgent.stoppingDistance = _attDis = mon._attDis;
            _speed = mon._speed;
            _minmoney = mon._minGold;
            _maxmoney = mon._maxGold;
        }
        _curhp = _maxHp;
        _curhp = Mathf.Clamp(_curhp, 0, _maxHp);
        finalDam = dam;
    }

    public override void HittingME(int dam)
    {
        _player = _target.GetComponent<CharacterCtrl>();
        int _finalDam = dam - finalDef;
        if(_finalDam<=0 )
        {
            _finalDam = 1;
        }
        _curhp -= _finalDam;
        _state = DefineEnumHelper.MonsterState.TargetOn;
        UIManager._instance.monsterWindow.MonsterHP(_curhp, maxHp);
        GameObject go = Instantiate(ResourcePoolManager._instance.GetBooldObj(), transform);
        go.transform.position = _bloodPoint.transform.position;
        Destroy(go, 1.5f);
    }

}
