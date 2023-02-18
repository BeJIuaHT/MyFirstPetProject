using Assets.Scenes.Scripts;
using MyProject.Utilites;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Unit : MonoBehaviour, ISelectable, IAtacker, ICanBeDamaged
{
    [SerializeField] protected float _maxHealth;
    [SerializeField] protected float _health;
    [SerializeField] protected float _maxMana;
    [SerializeField] protected float _mana;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _damage;
    [SerializeField] protected float _maxActions;
    [SerializeField] public float Actions { get; set; }
    [SerializeField] protected RectTransform _lifebar;
    [SerializeField] protected RectTransform _manabar;
    [SerializeField] protected RectTransform _actionsbar;
    [SerializeField] public Vector3 PlayerAnchor { get; private set; } = new(0.5f, 0.2f, 0f);
    private const float TOLBAR_Y = 42f;
    private const float TOLBAR_X = 408f;
    protected RectTransform[] _bar;
    public bool _imselected;
    public bool _imMove = false;
    public PathFinding PathFinding { private set; get; } = new();
    public NewPathFinder PathFinder { private set; get; }
    public Animator Animator { private set; get; }
    public SpriteRenderer SpriteRenderer { private set; get; }
    protected Collider2D _colider;
    protected Rigidbody2D _rigidbody;
    protected Transform _transform;
    public GameObject Target {set; get; }
    protected Unit unit;
    public MyTilemap MyTilemap { private set; get; }


    public virtual void Start()
    {
        Debug.Log("test");
        Animator = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        _colider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = GetComponent<Transform>();
        MyTilemap = GetComponentInParent<MyTilemap>();
        Actions = _maxActions;
        _health = _maxHealth;
        _mana = _maxMana;
        PathFinding.InitPathFinding();
        PathFinder = new(Lincks.GetTilemap(), MyTilemap, this);
        
    }
    private void OnDestroy()
    {
        UnitBarController.UnitDestroed.Invoke(this);
    }
    public virtual void Atack(GameObject target)
    {
        
        Debug.Log("UNIT" +this.gameObject.ToString() + "Атакует" + target.ToString() + "урон = " + _damage);
        target.TryGetComponent<Unit>(out unit);
            unit.TackeDamage(this.gameObject, _damage);
   
    }
    
    

    public virtual void DeSelected()
    {
        _imselected = false;
        LockPosition();
    }

    public virtual void Selected()
    {
        _imselected = true;
        RemovePosition();
    }

    public virtual void TackeDamage(GameObject atacekr, float damage)
    {
        if (damage >= _health) _health = 0;
        else _health -= damage;
        RefreshBar();
        Debug.Log("UNIT" + this.gameObject.ToString() + "Получает урон = "+ damage + " От " + atacekr) ;

    }
    public void RefreshBar()
    {
        float cofHealth = _health * (TOLBAR_X / _maxHealth);
        _lifebar.sizeDelta = new Vector2(cofHealth, TOLBAR_Y);
        float cofMana = _mana * (TOLBAR_X / _maxMana);
        _manabar.sizeDelta = new Vector2(cofMana, TOLBAR_Y);
        float cofActions = Actions * (TOLBAR_X / _maxActions);
        _actionsbar.sizeDelta = new Vector2(cofActions, TOLBAR_Y);
    }
    public void LockPosition()
    {
        MyTilemap.LockPosition(this);
    }
    public void RemovePosition()
    {
        MyTilemap.RemovePosition(_transform.position);
    }

    public Vector3 GetPosition()
    {
        return _transform.position;
    }

    public float GetRange()
    {
        return Actions;
    }

    public Transform GetTransform()
    {
        return _transform;
    }
}
