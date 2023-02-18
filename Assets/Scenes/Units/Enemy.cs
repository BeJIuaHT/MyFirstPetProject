using System.Collections;
using UnityEngine;

namespace Assets.Scenes.Units
{
    public class Enemy : Unit
    {
       
        public override void Start()
        {
            base.Start();
            
            _damage = 5;

        }
        public override void TackeDamage(GameObject atacker, float damage)
        {
            if (damage >= _health) _health = 0;
            else _health -= damage;
            if (_health == 0 )
            {
                Destroy(gameObject);
            }
            RefreshBar();
            Target = atacker.gameObject;
            Atack(Target);
        }
    }
}