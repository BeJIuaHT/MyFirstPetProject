using Assets.Scenes.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    [SerializeField] private Transform _mousePoint;
    [SerializeField] private Camera _camera;
    [SerializeField] private MyTilemap _map;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private string _leftMouse = "Select";
    [SerializeField] private string _rightMouse = "Action";

    private InputAction LeftMouse;
    private InputAction RightMouse;
    private Vector3 _mousePosition;
    private ISelectable selected;

    public static UnityEvent<ISelectable> Select = new();
    public static UnityEvent<ISelectable, Vector3> Action = new();
    private void Start()
    {
        LeftMouse = _input.actions.FindAction(_leftMouse);
        RightMouse = _input.actions.FindAction(_rightMouse);
        LeftMouse.started += LeftMouseDown;
        LeftMouse.canceled += LeftMouseUp;
        RightMouse.started += RightMouseDown;
        RightMouse.canceled += RightMouseUp;
    }

    private void Update()
    {
        _mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        switch (LeftMouse.phase)
        {
            case InputActionPhase.Performed :
                {
                    Vector3 scale = _mousePoint.position - _mousePosition;
                    scale.z = 0;
                    scale *= -32f;
                    _mousePoint.localScale = scale;
                    break;
                }
            default:
                {
                    _mousePoint.position = _camera.ScreenToWorldPoint(Input.mousePosition);
                    break;
                }
        }

        
    }
    private void OnDestroy()
    {
        LeftMouse.started -= LeftMouseDown;
        LeftMouse.canceled -= LeftMouseUp;
        RightMouse.started -= RightMouseDown;
        RightMouse.canceled -= RightMouseUp;
    }
    private void OnDrawGizmos()
    {
        Vector3 direction = _mousePosition;
        direction.z = 100;
        Gizmos.DrawLine(_mousePoint.position, direction);

    }
    private void LeftMouseDown(InputAction.CallbackContext callbeck)
    {

    }
    private void LeftMouseUp(InputAction.CallbackContext callbeck)
    {
        Vector3 direction = _mousePosition;
        direction.z = 100;
        _mousePoint.localScale = Vector3.one;
        if (EventSystem.current.IsPointerOverGameObject() !=false )
        {
            Debug.Log("Навжл на Ui");
            return;
        }

        RaycastHit2D[] rays = Physics2D.RaycastAll(_mousePosition, direction);
        foreach (RaycastHit2D ray in rays)
        {
            
            Debug.Log("Луч попал в " + ray.transform.gameObject);
        }
        Collider2D[] all = Physics2D.OverlapAreaAll(_mousePoint.position, _mousePosition);
        foreach (Collider2D c in all)
        {
            if (c.transform.TryGetComponent<ISelectable>(out ISelectable selectable))
            {
                selected?.DeSelected();
                selected = selectable;
                selected?.Selected();
                Select.Invoke(selected);
            }
        }
        if (all.Count() == 0)
        {
            selected?.DeSelected();
            Select.Invoke(null);
        }

    }
    private void RightMouseDown(InputAction.CallbackContext callbeck)
    {

    }
    private void RightMouseUp(InputAction.CallbackContext callbeck)
    {
        Collider2D[] all = Physics2D.OverlapAreaAll(_mousePoint.position, _mousePosition);
        foreach (Collider2D c in all)
        {
            c.transform.TryGetComponent<ISelectable>(out ISelectable selectable);
            
                Action.Invoke(selectable, _mousePosition);
            
        }
        if (all.Count() == 0)
        {
            Action.Invoke(null, _mousePosition);
        }
    }
}
