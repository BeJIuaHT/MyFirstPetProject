using Assets.Scenes.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBoxVision : MonoBehaviour
{
    public SpriteRenderer sprite;
    public bool Visible = false;
    public Allies _allies;

   // bool selected = false;
    void Start()
    {
        sprite= GetComponent<SpriteRenderer>();
        sprite.enabled = Visible;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //   // Debug.Log("Он тригер ENTER");
    //    sprite.enabled = true;
    //    sprite.color = Color.white;

    //}
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("Он тригер EXIT");
        sprite.color = Color.white;
        sprite.enabled = false;
        if(collision.gameObject.name != "Mouse")
        _allies = null;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name != "Mouse")
        collision.gameObject.TryGetComponent<Allies>(out _allies);
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        sprite.enabled = true;
        
        if (_allies != null)
        {
            
            if (_allies._imselected)
            {
                sprite.color = Color.green;
            }
            else sprite.color = Color.white;
        }
    }
}
