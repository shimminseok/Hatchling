using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] GameObject _cinemachineCameraTarget;
    [SerializeField] CinemachineVirtualCamera _cam;
    [SerializeField] float _mouseSpeed = 5;
    [SerializeField] float _topClamp = 70.0f;
    [SerializeField] float _bottomClamp = -30.0f;

    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;

    public Vector3 _cameraOffset;
    public float _dis { get; private set; }
    public GameObject _targetTr
    {
        get => _cinemachineCameraTarget;
        set => _cinemachineCameraTarget = value;
    }
    void OnEnable()
    {
        GameManager._instance.IngameScene();
    }
    void Start()
    {
        _cinemachineTargetYaw = _cinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _cam.Follow = _cinemachineCameraTarget.transform;
        _cam.LookAt = _cinemachineCameraTarget.transform;
        _dis = _cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;
    }
    void FixedUpdate()
    {
        CameraZoomInOut();
        CameraRotation();
    }
    void CameraRotation()
    {
        if (Input.GetMouseButton(1))
        {
            _cinemachineTargetYaw += Input.GetAxis("Mouse X");
            _cinemachineTargetPitch += Input.GetAxis("Mouse Y");
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
        }
        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, -_cinemachineTargetYaw * _mouseSpeed, 0.0f);
    }
    float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);

    }
    void CameraZoomInOut()
    {
        float zoom = Input.GetAxisRaw("Mouse ScrollWheel");
        if (zoom != 0)
        {
            _cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance -= zoom * 3f;
            _dis = _cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance = Mathf.Clamp(_cam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance, 0, 50);
        }
    }
}
