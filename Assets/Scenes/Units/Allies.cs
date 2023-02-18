using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Assets.Scenes.Map;
using Assets.Scenes.Units.Abilities;
using Assets.Scenes.Scripts;
using System;

namespace Assets.Scenes.Units
{


    public class Allies : Unit
    {
        [SerializeField] private PlayerInput _playerInput;
        private Dictionary<int, IAbility> _myAbilities = new(5);

        public override void Start()
        {
            base.Start();
            Attack attack = new(_transform, _maxActions, _damage, null);
            Run run = new(_transform, Vector3.zero, ref _speed);
            _myAbilities.Add(attack.name, attack);
            _myAbilities.Add(run.name, run);
            MousePosition.MouseRightClick.AddListener((x) => { Target = x.gameObject;});
            
            

        }
        public override void Selected()
        {
            base.Selected();
            MouseController.Action.AddListener(DoAction);
        }
        public override void DeSelected()
        {
            base.DeSelected();
            MouseController.Action.RemoveListener(DoAction);
        }

        private void DoAction(ISelectable selectable, Vector3 direction)
        {
            if (selectable != null)
            {
                if (selectable.GetType().Equals(typeof(Enemy)))
                {
                    Target = selectable.GetTransform().gameObject;
                    UseAbility(AbilityNames.Attack);
                }
            }
            else
            {
                
                UseAbility(AbilityNames.Run, direction);
            }
        }

        public void UseAbility(AbilityNames name)
        {
            
            switch (name)
            {
                    case AbilityNames.Attack:
                    {
                        Attack atack = (Attack)_myAbilities[(int)AbilityNames.Attack];
                        atack.SetDamage(_damage);
                        atack.SetTarget(Target.transform);
                        atack.SetRange(Actions);
                        atack.Use();
                        break;
                    }
                default:
                    {
                        Debug.Log("нет такой способности");
                        return;
                    }
            }
        }
        public void UseAbility(AbilityNames name, Vector3 direction)
        {

            switch (name)
            {
                case AbilityNames.Attack:
                    {
                        Attack atack = (Attack)_myAbilities[(int)AbilityNames.Attack];
                        atack.SetDamage(_damage);
                        atack.SetTarget(Target.transform);
                        atack.SetRange(Actions);
                        atack.Use();
                        break;
                    }
                case AbilityNames.Run:
                    {
                        Run run = (Run)_myAbilities[(int)AbilityNames.Run];
                        run.SetDirection(direction);
                        run.Use();
                        break;
                    }
                default:
                    {
                        Debug.Log("нет такой способности");
                        return;
                    }
            }
        }
        //private int MoveToNexTile(int counter)
        //{
        //    PathNode node = pathNodes[counter];
        //    Vector3 target = node.endPos + PlayerAnchor;
        //    float step = _speed * Time.deltaTime;
        //    if (Vector3.Distance(_transform.position, target) > 0.03f)
        //    {
        //        _transform.position = Vector3.MoveTowards(_transform.position, target, step);
        //        Vector3 v = node.endPos - node.startPos;
        //        if (v.x < 0) _spriteRenderer.flipX = true;
        //        else _spriteRenderer.flipX = false;

        //    }
        //    else
        //    {
        //        _actions -= 1;
        //        counter++;

        //    }
        //    return counter;
        //}
        // Legasy Path finder;
        //public List<PathNode> FindePath(Vector3 vec)
        //{
        //   // _pathFinding.FindPath(this, vec);
        //    pathNodes = _pathFinding.GetPathList(this, vec);
        //    return pathNodes;
        //}
    }
}