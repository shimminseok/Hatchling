using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracableObject : MonoBehaviour
{
    private void Update()
    {
        if (Vector3.Distance(GameManager._instance.character.transform.position, transform.position) > 30)
        {
            ObjectManager._instance.Remove(transform.parent.gameObject);
            gameObject.SetActive(false);
            MonsterCtrl mon = gameObject.GetComponentInParent<MonsterCtrl>();
            mon.infoUI.gameObject.SetActive(false);
        }
    }
    void OnEnable()
    {
        ObjectManager._instance.Add(transform.parent.gameObject);
    }
}
