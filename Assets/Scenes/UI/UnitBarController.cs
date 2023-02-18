using Assets.Scenes.Scripts;
using Assets.Scenes.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public struct Ui2Linck
{
    public Transform ui;
    public Transform interactObject;
    public int Ui_hashCode { private set; get; }
    public Ui2Linck(Transform ui, Transform interactObject )
    {
        this.ui = ui;
        this.interactObject = interactObject;
        this.Ui_hashCode = ui.GetHashCode();
    }
}
public class UnitBarController : MonoBehaviour
{
    [SerializeField] private GameObject _unitBar;
    [SerializeField] private GameObject _bounds;
    [SerializeField] private PlayerInput _input;
    private Dictionary<ISelectable, UiButton> _uiButtons = new();
    private List<UiButton> _buttons = new();
    private InputAction _tab;
    private InputAction _plus;
    private InputAction _minus;
    public Transform contetn;
    public GameObject prefab;
    private ISelectable _activeUnit;
    private UiButton _activeButton;

    public static UnityEvent<ISelectable> UnitDestroed { get; private set; } = new();

    private void Start()
    {
       // contents = unitBar.GetComponentsInChildren<Transform>();
        _tab = _input.actions.FindAction("Tab");
        _plus = _input.actions.FindAction("Plus");
        _minus = _input.actions.FindAction("Minus");
        _tab.performed += NextUnit;
        // plus.performed += AddUnit;
        //minus.performed += RemoveUnit;
        MouseController.Select.AddListener(TryAddToBar);
        //_bounds.SetActive(false);
       // _bounds.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        UnitDestroed.AddListener(OnUnitDesroy);
    }
    private void OnUnitDesroy(ISelectable go)
    {
        
        var button = _buttons.Find(x => x._link == go);
        if (_buttons.Contains(button))
        {
            button._link.DeSelected();
            _buttons.Remove(button);
            Destroy(button.gameObject);
        }
        
    }
    private void NextUnit(InputAction.CallbackContext callbackContext)
    {
        SelectNext(_activeUnit);
    }
    private void CreateIconBar(ISelectable link)
    {
        if (_buttons.Exists(x => x._link == link)) return;
        GameObject test = Instantiate(prefab);
        test.transform.SetParent(contetn);
        UiButton button = test.GetComponent<UiButton>();
        button.Init(this, link);
        _buttons.Add(button);
        
        if (link.GetType() == typeof(Allies))
        {
            _activeButton?.ChengeBoundsColor(Color.white);
            _activeButton = button;
            _activeUnit = link.GetTransform().GetComponent<Unit>();
            button.ChengeBoundsColor(Color.green);
        }
    }
    private void RemoveButton(UiButton button)
    {
        if (_buttons.Count > 0)
        {
            _buttons.Remove(button);
            Destroy(button.gameObject);
        }
    }
    private void OnDestroy()
    {
        _tab.performed -= NextUnit;
       // plus.performed -= AddUnit;
       // minus.performed -= RemoveUnit;

    }
    private void TryAddToBar(ISelectable transform)
    {
        if (transform != null)
        {
            CreateIconBar(transform);
        }
        else
        {
            if (_buttons.Count > 0)
            {
                foreach (UiButton button in _buttons)
                {
                    Destroy(button.gameObject);
                }
                _buttons.Clear();
                _activeButton = null; _activeUnit = null;
            }
        }
    }

    private void SelectNext(ISelectable unit)
    {
        if (unit != null)
        {
            var index = _buttons.FindIndex(x => x._link == unit);
            if (index == _buttons.Count - 1) index = -1;
            while (_buttons[index+1]._link.GetType() != typeof(Allies))
            {
                index++;
                    if (index == _buttons.Count - 1) index = -1;
            }
            
            _activeUnit.DeSelected();
            _activeButton.ChengeBoundsColor(Color.white);
            _activeButton = _buttons[index+1];
            _activeButton.ChengeBoundsColor(Color.green);
            _activeUnit = _activeButton._link;
            _activeUnit.Selected();
        }
    }

    public void OnButtonClik(UiButton button)
    {
        _activeButton.ChengeBoundsColor(Color.white);
        _activeButton = button;
        _activeUnit.DeSelected();
        _activeButton.ChengeBoundsColor(Color.green);
        _activeUnit = _activeButton._link;
        _activeUnit.Selected();
    }
  
}
