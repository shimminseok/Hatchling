using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseEnemy : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            GameObject go = other.gameObject.transform.GetChild(0).gameObject;
            if (!go.activeSelf)
            {
                go.SetActive(true);
            }
        }
    }
}
