using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveMonster : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int n = 0; n < transform.childCount; n++)
            {
                transform.GetChild(n).gameObject.SetActive(true);
            }
            UIManager._instance.characterInfoWindow.OpenMapNameBox(gameObject.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int n = 0; n < transform.childCount; n++)
            {
                transform.GetChild(n).gameObject.SetActive(false);
            }
        }
    }
}
