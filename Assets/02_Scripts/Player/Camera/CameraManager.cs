using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] public CinemachineVirtualCamera[] _allvirtualcameras;

    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallYPanTime = 0.35f;
    public float _fallSpeedYDampingChangeThreshold = -15f;

    public bool IsLerpingYDamping { get; private set; }
    public bool LerpedFromPlayerFalling { get; set; }

    private Coroutine _lerpYPanCoroutin;
    private Coroutine _panCameraCoroutine;

    private CinemachineVirtualCamera _currentCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private float _normYPanAmount;

    private Vector2 _startingTrackedObjectOffset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i = 0; i < _allvirtualcameras.Length; i++)
        {
            if (_allvirtualcameras[i].enabled)
            {
                _currentCamera = _allvirtualcameras[i];

                _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

            }
        }


        _normYPanAmount = _framingTransposer.m_YDamping;

        _startingTrackedObjectOffset = _framingTransposer.m_TrackedObjectOffset;

    }

    public void LerpYDamping(bool isPlayerFalling)
    {
        _lerpYPanCoroutin = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlyaerFalling)
    {
        IsLerpingYDamping = true;

        float startDampAmount = _framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if (isPlyaerFalling)
        {
            endDampAmount = _fallPanAmount;
            LerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }


        float elapsedTime = 0f;
        while (elapsedTime < _fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / _fallYPanTime));
            _framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }

        IsLerpingYDamping = false;
    }

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        _panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
                default:
                    break;

            }

            endPos *= panDistance;

            startingPos = _startingTrackedObjectOffset;

            endPos += startingPos;
        }

        else
        {
            startingPos = _framingTransposer.m_TrackedObjectOffset;
            endPos = _startingTrackedObjectOffset;
        }

        float elapsedTime = 0f;
        while(elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            _framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }
    }

    #region Swap Camera

    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        if(_currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            cameraFromRight.enabled = true;

            cameraFromLeft.enabled = false;

            _currentCamera = cameraFromRight;

            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        else if(_currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            cameraFromLeft.enabled = true;

            cameraFromRight.enabled = false;

            _currentCamera = cameraFromLeft;

            _framingTransposer = _currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }



    #endregion
}
