using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCtrl : ObjectBase
{

    [Header("Player")]
    [SerializeField] int _maxMp;
    [SerializeField] float _moveSpeed = 7;
    [SerializeField] TextMesh _textMesh;
    [Range(0.0f, 0.3f)]
    [SerializeField] float _rotationSmoothTime = 0.12f;
    [SerializeField] float SpeedChangeRate = 10.0f;

    float _targetDis;

    // player
    [SerializeField]
    int _curhp;
    [SerializeField]
    int _curmp;
    [SerializeField]
    int _maxEx;
    float _speed;
    float _animationBlend;
    float _targetRotation = 0.0f;
    float _rotationVelocity;


    Animator _animator;
    CharacterController _controller;
    GameObject _mainCamera;


    //이동관련 변수
    public bool _isArrive = false;
    public bool _isMove = true;
    //임시 변수
    int _monsterNum = 0;
    float _cheakTime;
    public int _money;
    Vector3 _moveVec;
    public int _curex { get; set; }
    public int UseItem(int key)
    {
        if (DataTableManager._instance._ItemDataDic.TryGetValue(key, out DataTableSt.stItemData itemdata))
        {
            switch ((DefineEnumHelper.StatKind)itemdata._stat)
            {
                case DefineEnumHelper.StatKind.HP:

                    if (_curhp < _maxHp)
                    {
                        _curhp += (int)(_maxHp * itemdata._value);
                        if (_curhp > _maxHp)
                        {
                            _curhp = _maxHp;
                        }
                    }
                    else
                    {
                        Debug.Log("체력이 가득 찼습니다");
                        return 0;
                    }
                    break;
                case DefineEnumHelper.StatKind.MP:
                    if (_curmp > _maxMp)
                    {
                        _curmp = _maxMp;
                    }
                    else if (_curmp < _maxMp)
                    {
                        _curmp += (int)(_maxMp * itemdata._value);
                    }
                    else
                    {
                        Debug.Log("마나가 가득 찼습니다");
                        return 0;
                    }
                    break;
            }
            SoundManager._instance.SFXSoundPlay(DefineEnumHelper.SFXSounds.UsePortion);
        }
        return -1;
    }
    //
    private void Awake()
    {
#if UNITY_EDITOR
        _moveSpeed = 15;
#endif
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        InitData(UserInfo._instance._level, UserInfo._instance._curHP, UserInfo._instance._curMP, UserInfo._instance._money, UserInfo._instance._curEx);
    }
    private void Start()
    {
        StartCoroutine(UpdateUserInfo());
    }
    void InitData(int l, int h, int m, int money, int ex)
    {
        _objName = UserInfo._instance._nickName;
        _textMesh.text = _objName;
        _lv = l;
        _curhp = h;
        _curmp = m;
        _money = money;
        _curex = ex;
        _maxEx = (_lv * 2) * _lv * 100;
        _damage = (int)((0.15 * _lv * 10) + 10);
        _defense = (int)(Mathf.Log(Mathf.Pow(_lv, 2)) + _lv);
        transform.position = UserInfo._instance._curPos;
        _maxHp = (int)(_lv * (1000 / Mathf.Log(_lv + 1)));
        _maxMp = (int)(_lv * (1000 / Mathf.Log(_lv + 1)));
        if (_lv == 1)
        {
            _maxHp = 1000;
            _maxMp = 1000;
        }
        FinalDam(0);
        FinalDef(0);
    }
    void LevelUP()
    {
        _lv++;
        _maxHp = (int)(_lv * (1000 / Mathf.Log(_lv + 1)));
        _maxMp = (int)(_lv * (1000 / Mathf.Log(_lv + 1)));
        finalDam = _damage = (int)((0.15 * _lv * 10) + 10);
        finalDef = _defense = (int)(Mathf.Log(Mathf.Pow(_lv, 2)) + _lv);
        _curex = _curex - _maxEx;
        _curhp = _maxHp;
        _curmp = _maxMp;
        _maxEx = (_lv * 2) * _lv * 100;
        UserInfo._instance.SetUserInfo(UserInfo._instance._nickName, _lv, _curhp, _curmp, _money, _curex);
        UIManager._instance.equipMentWindow.GetComponentInChildren<CharacterInfo>().UpdateInfo(_lv.ToString(), _curhp.ToString(), _maxHp.ToString(), _curmp.ToString(), _maxMp.ToString(), finalDam.ToString(), finalDef.ToString());
        GameObject go = Instantiate(ResourcePoolManager._instance.GetLevelUpEffect(), transform);
        Destroy(go, 3f);
        SoundManager._instance.SFXSoundPlay(DefineEnumHelper.SFXSounds.LevelUp);
    }
    IEnumerator UpdateUserInfo()
    {
        UserInfo._instance.SetUserInfo(UserInfo._instance._nickName, _lv, _curhp, _curmp, _money, _curex);
        while (GameManager._instance._isGameStart)
        {
            UserInfo._instance.SetUserInfo(UserInfo._instance._nickName, _lv, _curhp, _curmp, _money, _curex);
            UserInfo._instance.SaveUserPosition(transform.position);
            yield return new WaitForSeconds(10f);
        }
        UserInfo._instance.SaveUserPosition(transform.position);
    }
    private void Update()
    {
        if (!_isArrive)
        {
            TargetMove();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && _target != null)
        {
            _isArrive = false;
            _isMove = false;
        }
        if (!_isMove)
        {
            if (_isArrive)
            {
                Attack();
            }
        }
        if (_targetDis <= 2)
        {
            _isArrive = true;
        }
        else
        {
            _isAtt = false;
            _animator.SetBool("boolAttack", false);
        }
        if (_curex >= _maxEx)
        {
            LevelUP();
        }
        TarGetOn();
        UIManager._instance.characterInfoWindow.CharacterHP(_curhp, _maxHp, _curmp, _maxMp);
        UIManager._instance.characterInfoWindow.CharacterEx(_curex, _maxEx);
    }
    void FixedUpdate()
    {
        if (_isArrive)
        {
            Move();
        }

    }
    private void OnDisable()
    {
        UIManager._instance.equipMentWindow.CheakMountItem();
        UserInfo._instance.SaveUserPosition(GameManager._instance.character.transform.position);
    }
    void Move()
    {
        _animator.SetBool("boolAttack", false);
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector2 move = new Vector3(x, z);
        float targetSpeed = _moveSpeed;
        if (move == Vector2.zero)
        {
            targetSpeed = 0.0f;
        }
        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * SpeedChangeRate);
        }
        else
        {
            _speed = targetSpeed;
        }
        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        _animationBlend = _animationBlend < 0.01f ? 0 : _animationBlend;
        Vector3 inputDirection = new Vector3(x, 0.0f, z).normalized;
        if (x != 0 || z != 0)
        {
            _isMove = true;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
            _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                _rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
        else
        {
            _animationBlend = Mathf.Lerp(_animationBlend, 0, Time.deltaTime * SpeedChangeRate);
            _animationBlend = _animationBlend < 1.5f ? 0 : _animationBlend;
        }
        _moveVec = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        _moveVec.y += Physics.gravity.y;
        _controller.Move(_moveVec * _speed * Time.deltaTime);
        _animator.SetFloat("Speed", _animationBlend);

    }
    void TargetMove()
    {
        if (_target != null)
        {
            Vector3 targetPos = Vector3.MoveTowards(transform.position, _target.transform.position, _moveSpeed * Time.deltaTime);
            Vector3 frameDir = targetPos - transform.position;
            _controller.Move(frameDir + Physics.gravity);
            transform.LookAt(_target.transform);
            _speed = Mathf.Lerp(_speed, _moveSpeed, Time.deltaTime * _moveSpeed);
            _animator.SetFloat("Speed", _speed);
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                _isArrive = true;
            }
        }
    }
    public override void Attack()
    {
        if (_target == null)
        {
            _isAtt = false;
            _animator.SetBool("boolAttack", false);
        }
        else
        {
            Vector3 lvec = _target.transform.position - transform.position;
            lvec.y = 0;
            transform.rotation = Quaternion.LookRotation(lvec);
            _hitboxTf.position = _target.transform.position + new Vector3(0, 1, 0);
            _isAtt = true;
            _isArrive = true;
            _animator.SetBool("boolAttack", true);
            _speed = 0;
            _animator.SetFloat("Speed", _speed);
        }
    }
    public override void EndAttack()
    {
        if (_target != null)
        {
            Vector3 lvec = _target.transform.position - transform.position;
            lvec.y = 0;
            transform.rotation = Quaternion.LookRotation(lvec);
        }
        else
        {
            _isAtt = false;
        }
    }
    public void TarGetOn()
    {
        if (!ObjectManager._instance._monsterList.Contains(_target))
        {
            _cheakTime += Time.deltaTime;
            if (_cheakTime > 2)
            {
                _target = null;
                _targetDis = 0;
                _cheakTime = 0;
                UIManager._instance.monsterWindow.gameObject.SetActive(false);
            }
        }
        if (ObjectManager._instance._monsterList.Count != 0)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (_monsterNum > ObjectManager._instance._monsterList.Count - 1)
                {
                    _monsterNum = 0;
                    _target = ObjectManager._instance._monsterList[_monsterNum++];
                }
                else
                {
                    _target = ObjectManager._instance._monsterList[_monsterNum++];
                }
            }
            if (_target != null)
            {
                MonsterCtrl monsterCtrl = _target.GetComponent<MonsterCtrl>();
                UIManager._instance.monsterWindow.MonsterName(monsterCtrl.level, monsterCtrl._name);
                UIManager._instance.monsterWindow.MonsterHP(monsterCtrl._curHp, monsterCtrl.maxHp);
                _targetDis = Vector3.Distance(transform.position, _target.transform.position);
                UIManager._instance.monsterWindow.MonsterDis(ObjectManager._instance.ObjDistance(this));
                _target = monsterCtrl.gameObject;
                monsterCtrl.infoUI.gameObject.SetActive(true);
            }
        }
    }
    public override void HittingME(int dam)
    {
        int _finalDam = dam - finalDef;
        if (_finalDam <= 0)
        {
            _finalDam = 1;
        }
        _curhp -= _finalDam;

    }
    public void EarthSound()
    {
        SoundManager._instance.SFXSoundPlay(SoundManager._instance.PlayEaethSound(Random.Range(0, 19)));
    }
}
