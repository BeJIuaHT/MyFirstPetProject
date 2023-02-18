using UnityEngine;
using Assets.Scenes.Units;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEditor.PackageManager;
using System;
using UnityEngine.Events;

public class NewCameraContol : MonoBehaviour
{
    private Vector3 Position;
    [SerializeField] private float SPEED = 5f;
    [SerializeField] private float MinScale = 5f;
    [SerializeField] private float MaxScale = 5f;
    [SerializeField] private bool DebugMode = false;
    private PlayerInput m_PlayerInput;
    private InputAction m_MoveAction;
    private Camera m_camera;
    private Transform m_transgorm;
    private Vector2 movederection;

    public static UnityEvent<Vector3> FocusOn = new();
    

    private void Start()
    {
       m_camera= GetComponent<Camera>();
       m_transgorm = transform;
        FocusOn.AddListener(Move);
    }
    void Update()
    {
        if (m_PlayerInput == null)
        {
            m_PlayerInput = GetComponent<PlayerInput>();
            m_PlayerInput.actions["MouseWheel"].performed += Scroll;
            m_MoveAction = m_PlayerInput.actions["Move"];
        }
        m_transgorm.position += SPEED * Time.deltaTime * (Vector3)m_MoveAction.ReadValue<Vector2>();

        if (DebugMode == true)
            InputSystem.onActionChange +=
            ActionChangeDebug;
        else InputSystem.onActionChange -=
                ActionChangeDebug;
    }

    private void Scroll(InputAction.CallbackContext callback)
    {
        Vector2 wheel = callback.ReadValue<Vector2>();
        if (m_camera.orthographicSize <= MinScale && wheel.y > 0f) return;
        if (m_camera.orthographicSize >= MaxScale && wheel.y < 0f) return;
        else m_camera.orthographicSize += wheel.y / -120f;
    }
    private void Move(Vector3 vec)
    {
        Vector3 input = vec;
        input.z = m_transgorm.position.z;
        m_transgorm.position = input;

    }
    private void ActionChangeDebug(object obj, InputActionChange change)
    {
        // obj can be either an InputAction or an InputActionMap
        // depending on the specific change.
        switch (change)
        {
            case InputActionChange.ActionStarted:
            case InputActionChange.ActionPerformed:
            case InputActionChange.ActionCanceled:
                Debug.Log($"{((InputAction)obj).name} {change}");
                break;
        }
    }


}
