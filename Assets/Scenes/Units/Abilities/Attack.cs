using System.Collections;
using UnityEngine;

namespace Assets.Scenes.Units.Abilities
{
    public class Attack : Ability
    {
        private float _range;
        private float _damage;
        private Transform _target;
        public Attack( Transform owner, float range, float damage, Transform target,
                        int name = (int)AbilityNames.Attack, int description = 0) 
                      : base(name, description, owner)
        {
            _range = range;
            _damage = damage;
            _target = target;
        }

        public override bool Use()
        {
            if (_target == null) return false;
            else if (_damage < 0f) return false;
            else if (_range < 0f) return false;
            else
            {
                if (!_target.TryGetComponent<Unit>(out var curentTarget)) return false;
                curentTarget.TackeDamage(owner.gameObject, _damage);
                return true;
            }
        }
        public bool SetTarget(Transform target)
        {
            if (target == null) return false;
            _target = target;
            return _target != null;
        }
        public bool SetRange(float range)
        {
            _range = range;
            return _range >= 0.2f;
        }
        public bool SetDamage(float damage)
        {
            _damage = damage;
            return _damage >= 0.2f;
        }
        
    }
}