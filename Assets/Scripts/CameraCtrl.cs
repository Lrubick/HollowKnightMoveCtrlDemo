using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public bool isFollow = true;
    float _OffsetZ;
    public Vector3 _CameraOffset;

    public float _followSpeed;
    public float _resetSpeed;
    public Transform _target;
    public bool lookForward = false;
    public float lookForwardTriggerDistance = 0.1f;
    public float HlookForwardDistance = 3;

    public Vector2 _lastPosition ,_currentPosition;


    // Use this for initialization
    void Start()
    {
        _lastPosition = _target.position;
        _OffsetZ = (transform.position.z - _target.position.z);
    }

    // Update is called once per frame
    void Update()
    {
    }
}