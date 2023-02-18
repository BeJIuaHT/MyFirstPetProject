using Assets.Scenes.Scripts;
using Assets.Scenes.Units;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiButton : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _bounds;
    private UnitBarController _unitbar;
    public ISelectable _link { get; private set; }

    public void OnClick()
    {
        if (_unitbar != null)
        {
            _unitbar.OnButtonClik(this);
            NewCameraContol.FocusOn.Invoke(_link.GetPosition());
        }
        else Debug.Log("Укажите BarController");
    }
    public void Init(UnitBarController perent,ISelectable link)
    {
        _unitbar = perent;
        _link = link;
        _icon = GetComponent<Image>();
        _icon.sprite = _link.GetTransform().GetComponent<SpriteRenderer>().sprite;
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        if (link.GetType() == typeof(Allies))
        {
            _bounds.color = Color.white;
        }
        else if (link.GetType() == typeof(Enemy)) 
        {
            _bounds.color = Color.red;
        }
        else
        {
            _bounds.color = Color.yellow;
        }
    }
    public void ChengeBoundsColor(Color color)
    {
        _bounds.color = color;
    }
}
