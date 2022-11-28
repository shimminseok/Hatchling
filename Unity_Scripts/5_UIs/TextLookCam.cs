using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLookCam : MonoBehaviour
{
    GameObject _cam;

    Vector3 _originScale;
    CameraCtrl _camctrl;
    TextMesh _text;
    void Start()
    {
        _text = GetComponent<TextMesh>();
        _text.text = gameObject.transform.root.GetComponent<ObjectBase>()._name;
        _originScale = transform.localScale;
        while (_cam == null)
        {
            _cam = Camera.main.gameObject;
            _camctrl = _cam.GetComponent<CameraCtrl>();
        }
    }
    void Update()
    {
        if (_cam != null)
        {
            float dist = Vector3.Distance(_cam.transform.position, transform.position);
            Vector3 newScale = _originScale * dist / _camctrl._dis;
            //transform.localScale = newScale;
            transform.rotation = _cam.transform.rotation;
        }
    }
}
