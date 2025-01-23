using System.Collections.Generic;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
using Runtime.Data.UnityObjects;
using Runtime.Signals;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        private InputData _inputData;
        private bool _isAvailableForTouch, _isFirstTimeTouchTaken, _isTouching;
        
        private float _currentVelocity;
        private float3 _moveVector;
        private Vector2? _mousePosition;


        private void Awake()
        {
            _inputData = GetInputData();
        }

        private InputData GetInputData()
        {
            return Resources.Load<CD_Input>("Data/CD_Input").inputData;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            InputSignals.Instance.onEnableInput += OnEnableInput; 
            InputSignals.Instance.onDisableInput += OnDisableInput; 
        }


        private void OnDisableInput()
        {
            _isAvailableForTouch = false;
        }

        private void OnEnableInput()
        {
            _isAvailableForTouch = true;
        }

        private void OnReset()
        {
            _isAvailableForTouch = false;
            //_isFirstTimeTouchTaken = false;
            _isTouching = false;
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            InputSignals.Instance.onEnableInput -= OnEnableInput; 
            InputSignals.Instance.onDisableInput -= OnDisableInput; 
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Update()
        {
            if(!_isAvailableForTouch) return;
            if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement())
            {
                _isTouching = false; 
                InputSignals.Instance.onInputReleased?.Invoke();
                Debug.LogWarning("Executed ---> OnInputReleased");
            }

            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
            {
                _isTouching = true;
                InputSignals.Instance.onInputTaken?.Invoke();
                Debug.LogWarning("Executed ---> OnInputTaken");
                if (!_isFirstTimeTouchTaken)
                {
                    _isTouching = true;
                    InputSignals.Instance.onInputTaken?.Invoke();
                    Debug.LogWarning("Executed ---> OnFirstTimeTouchTaken");
                }
                
                _mousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
            {
                if (_isTouching)
                {
                    if (_mousePosition != null)
                    {
                        Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value;
                        if (mouseDeltaPos.x > _inputData.horizontalInputSpeed)
                        {
                            _moveVector.x = _inputData.horizontalInputSpeed/ 10f * mouseDeltaPos.x;
                        }
                        else if (mouseDeltaPos.x < -_inputData.horizontalInputSpeed)
                        {
                            _moveVector.x = -_inputData.horizontalInputSpeed/ 10f * mouseDeltaPos.x;
                        }
                        else
                        {
                            _moveVector.x = Mathf.SmoothDamp(-_moveVector.x, 0f, ref _currentVelocity, _inputData.clampSpeed);
                        }

                        _mousePosition = Input.mousePosition;
                        
                        InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                        {
                            HorizontalValues = _moveVector.x,
                            ClampValues = _inputData.clampValues,
                        });
                    }
                }
            }
        }

        private bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }

    }
}