using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;


public class CameraDown : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;

    private Collider2D _coll;

    private void Start()
    {
        _coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 exitDirection = (collision.transform.position - _coll.bounds.center).normalized;

            if(customInspectorObjects.swapCameras && customInspectorObjects.cameraOnLeft != null && customInspectorObjects.cameraOnLeft != null)
            {
                CameraManager.instance.SwapCamera(customInspectorObjects.cameraOnLeft, customInspectorObjects.cameraOnRight, exitDirection);
            }
            if (customInspectorObjects.panCameraOnContact)
            {
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (customInspectorObjects.panCameraOnContact)
            {
                CameraManager.instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}


[System.Serializable]
public class CustomInspectorObjects
{
    public bool swapCameras = false;
    public bool panCameraOnContact = false;

    [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
    [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

    [HideInInspector] public float panDistance = 3f;
    [HideInInspector] public float panTime = 0.35f;
    [HideInInspector] public PanDirection panDirection;
}

public enum PanDirection
{
    Up,
    Down,
    Left,
    Right
}


[CustomEditor(typeof(CameraDown))]
public class MyScriptEditor : Editor
{
    CameraDown cameraDown;

    private void OnEnable()
    {
        cameraDown = (CameraDown)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (cameraDown.customInspectorObjects.swapCameras)
        {
            cameraDown.customInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera on Left", cameraDown.customInspectorObjects.cameraOnLeft, typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

            cameraDown.customInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera on Right", cameraDown.customInspectorObjects.cameraOnRight, typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
        }

        if (cameraDown.customInspectorObjects.panCameraOnContact)
        {
            cameraDown.customInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction", cameraDown.customInspectorObjects.panDirection);

            cameraDown.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraDown.customInspectorObjects.panDistance);
            cameraDown.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time", cameraDown.customInspectorObjects.panTime);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(cameraDown);
        }
    }
}

