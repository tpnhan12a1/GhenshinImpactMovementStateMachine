using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

namespace GhenshinImpactMovement
{
    public class CameraZoom : MonoBehaviour
    {
        [SerializeField] [Range(0f, 10f)] private float _defaultDistance = 6f;
        [SerializeField] [Range(0f, 10f)] private float _minimumDistance = 1f;
        [SerializeField] [Range(0f, 10f)] private float _maximumDistance = 6f;

        [SerializeField][Range(0f, 10f)] private float _smoothing = 4f;
        [SerializeField][Range(0f, 10f)] private float _zoomSensitivity = 1f;

        private CinemachineFramingTransposer _framingTransposer;
        private CinemachineInputProvider _inputProvider;

        private float _currentTargeDistance;
        private void Awake()
        {
            _framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
            _inputProvider = GetComponent<CinemachineInputProvider>();
            _currentTargeDistance = _defaultDistance;
        }
        private void Update()
        {
            Zoom();
        }

        private void Zoom()
        {
            float zoomValue = _inputProvider.GetAxisValue(2) * _zoomSensitivity;
            _currentTargeDistance = Mathf.Clamp(_currentTargeDistance + zoomValue,_minimumDistance,_maximumDistance);

            float curentDistance = _framingTransposer.m_CameraDistance;
            if (curentDistance == _currentTargeDistance) return;

            float lerpedZoomValue = Mathf.Lerp(curentDistance, _currentTargeDistance,_smoothing*Time.deltaTime);
            _framingTransposer.m_CameraDistance = lerpedZoomValue;
                

        }
    }
}
