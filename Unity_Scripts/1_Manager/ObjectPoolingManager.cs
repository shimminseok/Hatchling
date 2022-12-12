using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPoolingManager : MonoBehaviour
{
    static ObjectPoolingManager _uniqueInstance;

    public static ObjectPoolingManager _instance => _uniqueInstance;

    [SerializeField] GameObject[] _objects;
    [SerializeField] float _respawnTime = 15;
    Queue<MonsterCtrl> _poolingObjectQueue = new Queue<MonsterCtrl>();

    public List<Transform> rootPoint = new List<Transform>();
    private void Awake()
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
    void Start()
    {
        //Initialize(50);
        StartCoroutine(MonsterSpawn());
    }
    public void Initialize(int initCount)
    {
        for (int n = 0; n < _objects.Length; n++)
        {
            for (int i = 0; i < initCount; i++)
            {
                CreateNewObj(n);
            }
        }
    }
    public void CreateNewObj(int id)
    {
        var newObj = Instantiate(ResourcePoolManager._instance.GetMonsterObj((DefineEnumHelper.MonsterKind)id)).GetComponent<MonsterCtrl>();
        newObj.transform.SetParent(rootPoint[newObj.id]);
        Vector3 originPos = new Vector3();
        RandomNavSphere(newObj.transform.parent.position, 250, out originPos);
        if(float.IsInfinity(originPos.x) || float.IsInfinity(originPos.y)||float.IsInfinity(originPos.z))
        {
            RandomNavSphere(newObj.transform.parent.position, 250, out originPos);
        }
        newObj.transform.position = originPos;
        newObj.gameObject.SetActive(false);

    }
    public Vector3 RandomNavSphere(Vector3 origin, float dist, out Vector3 goalPos)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist + origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, NavMesh.AllAreas);
        return goalPos = navHit.position;
    }
    IEnumerator MonsterSpawn()
    {
        while (GameManager._instance._isGameStart)
        {
            if (_poolingObjectQueue.Count > 0)
            {
                yield return new WaitForSeconds(_respawnTime);
                var mon = _poolingObjectQueue.Dequeue();
                mon.gameObject.SetActive(true);
                RandomNavSphere(mon.transform.parent.position, 250, out Vector3 originPos);
                mon.transform.position = originPos;
                mon.transform.SetParent(rootPoint[mon.id]);
                mon.InitData((DefineEnumHelper.MonsterKind)mon.id);
            }
            yield return null;
        }
    }
    public void ReturnObj(MonsterCtrl obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(transform);
        _poolingObjectQueue.Enqueue(obj);
    }
}
