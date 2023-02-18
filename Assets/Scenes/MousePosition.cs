using Assets.Scenes.Units;
using Assets.Scenes.Units.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MousePosition : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Tilemap _tilemap;
    private Transform _transform;
    private InputAction _look;
    private InputAction _attack;
    private InputAction _spase;
    private bool _move = true;
    private Allies _alies;
    private Enemy _enemy;
    private Unit _unit;
    private List<Allies> _selectedAllies = new();
    Vector3 starPos = Vector3.zero;
    Vector3 posInWorld = Vector3.zero;
    private MyTilemap _myTilemap;
    public Vector3 test = Vector3.zero;
    void Start()
    {
        _camera = Camera.main.transform.GetComponent<Camera>();
        _transform = GetComponent<Transform>();
        _input = _camera.GetComponent<PlayerInput>();
        _look = _input.actions["Select"];
        _attack = _input.actions["Attack"];
        _spase = _input.actions["Spase"];
        _look.started += LeftMouseDown;
        _look.canceled += LeftMouseUp;
        _attack.started += RightMouseDown;
        _myTilemap = _transform.GetComponentInParent<MyTilemap>(); 
    }
    private void OnDestroy()
    {
        if (_input == null) return;
        _look.started -= LeftMouseDown;
        _look.canceled -= LeftMouseUp;
        _attack.started -= RightMouseDown;

    }

    public static UnityEvent<Transform> MouseLeftClick { get; private set; } = new();
    public static UnityEvent<Transform> MouseRightClick { get; private set; } = new();
    public static UnityEvent<Unit, Vector3> MouseChangeTile { get; private set; } = new();

    void Update()
    {

        if ( Vector3.Distance(posInWorld, test) > 0.5f) // || _spase.phase == InputActionPhase.Performed curentTile == null ||
        {
           // curentTile = _tilemap.GetTile(_tilemap.WorldToCell(posInWorld));
            if (_selectedAllies.Count > 0)
            {
                foreach (Allies allies in _selectedAllies)
                {
                    test = _camera.ScreenToWorldPoint(Input.mousePosition);
                    test.z = 0;
                    MouseChangeTile.Invoke(allies, posInWorld);
                }
            }

        }
        if (_input == null)
        {
            _input= _camera.GetComponent<PlayerInput>();
            Debug.Log("”правление = null");
        }
        Conrtol();
    }

    private void Conrtol()
    {
        // LegasyControl();
        // Debug.Log(_look.phase);

        if (_look.phase == InputActionPhase.Performed)
        {
            posInWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
            posInWorld.z = 0;
            Vector3 scale = starPos - posInWorld;
            scale.z = 0;
            scale *= -32f;
            transform.localScale = scale;
           // Debug.Log("держу");
        }
        else
        {
            posInWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
            posInWorld.z = 0;
            _transform.position = posInWorld;
        }
    }


    private void  LegasyControl()
    {
        
        switch (_look.phase) 
        {
            case InputActionPhase.Waiting:
                posInWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
                posInWorld.z = 0;
                _transform.position = posInWorld;
                if (_move == false)
                {
                    _spriteRenderer.enabled = false;
                    _transform.localScale = Vector3.one;
                    SelectAllies();
                }
                _move = true;
                break;
            case InputActionPhase.Started:
                    _spriteRenderer.enabled = true;
                _move = false;
                    starPos = posInWorld;
                break;
            case InputActionPhase.Performed:
                posInWorld = _camera.ScreenToWorldPoint(Input.mousePosition);
                posInWorld.z = 0;
                Vector3 scale = starPos - posInWorld;
                scale.z = 0;
                scale *= -32f;
                transform.localScale = scale;
               
                break;
                
        

        }
        

    }
    private void LeftMouseDown(InputAction.CallbackContext context)
    {
        _spriteRenderer.enabled = true;
        starPos = posInWorld;
    }
    private void LeftMouseUp(InputAction.CallbackContext context)
    {
        _spriteRenderer.enabled = false;
        _transform.localScale = Vector3.one;
        SelectAllies();
    }

    private void RightMouseDown(InputAction.CallbackContext context)
    {
        AttackTarget();
       // GoTo();
    }

    private void SetUnit(Unit unit)
    {
        _unit = unit;
    }
    private void SelectAllies()
    {
       
        Collider2D[] coliders = Physics2D.OverlapAreaAll(starPos, posInWorld);
        if (coliders.Count() > 0) {
           
            foreach (Collider2D colider in coliders)
            {
                //Debug.Log(colider.gameObject.ToString());
                if (colider.gameObject.TryGetComponent<Allies>(out _alies)) _selectedAllies.Add(_alies);

                if (_selectedAllies.Count > 0)
                {
                    foreach (Allies allies in _selectedAllies)
                    {
                        allies.Selected();
                    }
                }
                MouseLeftClick.Invoke(colider.transform);
            }

        }
        else
        {
            foreach (Allies allies in _selectedAllies)
            {
                allies.DeSelected();
            }
            _selectedAllies = new();
        }
    }
    private void AttackTarget()
    {
        var colider = Physics2D.OverlapPoint(posInWorld);
        if (colider != null)
        {
            MouseRightClick.Invoke(colider.transform);
            Allies a = (Allies)_unit;
            a.UseAbility(AbilityNames.Attack);
        }
        else GoTo();
    }
    private void GoTo() 
    {
        if (_unit == null)
        {
            Debug.Log("нет активных юнитов");
            return;
        }
        _unit._imMove = false;
        //_unit.pathFinder.FindePath(_unit.GetPosition(), posInWorld);
        Allies a = (Allies)_unit;
        a.UseAbility(AbilityNames.Run, posInWorld);
        
    }
}
