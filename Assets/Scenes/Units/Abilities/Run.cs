using Assets.Scenes.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scenes.Units.Abilities
{
    public class Run : Ability
    {
        private Vector3 _direction;
        private float _speed;
        private List<Vector3> _path;
        private NewPathFinder _pathFinder;
        private Vector3 _playerAnchor;
        private Unit _unit;
        int counter = 0;
        public Run(Transform owner, Vector3 direction, ref float speed,
                        int name = (int)AbilityNames.Run, int description = 0)
                      : base(name, description, owner)
        {
            _direction = direction;
            _speed = speed;
        }
        public override bool Use()
        {
            if (owner == null) return false;
            if (_unit == null)
            {
                _unit = owner.GetComponent<Unit>();
            }
            if (_unit.Actions < 0.3f) return false;
            if (_pathFinder == null)
            {
                _pathFinder = new(_unit.MyTilemap.TileMap,
                                  _unit.MyTilemap,
                                  _unit);
                _playerAnchor = _unit.PlayerAnchor;
            }
            _pathFinder.FindePath(owner.position, _direction);
            _path = _pathFinder.finalPath;
            _unit.RemovePosition();
            _unit.StopAllCoroutines();
            _unit.StartCoroutine(RunToNexTile());
            return true;
        }
        public bool SetDirection(Vector3 direction)
        {
            _direction = direction;
            return true;
        }
        private IEnumerator RunToNexTile()
        {
            counter = 0;
            if (_direction.x < owner.position.x) _unit.SpriteRenderer.flipX = true;
            else _unit.SpriteRenderer.flipX = false;
            while (Vector3.Distance(owner.position, _direction) > 0.03f)
            {
               
                _unit.Animator.SetBool("move", true);
                if (counter >= _path.Count || _unit.Actions < 0.3f)
                {
                    _unit.Animator.SetBool("move", false);
                    _unit.MyTilemap.RefreshUnitMap();
                    break;
                };

                Vector3 target = _path[counter] + _playerAnchor;
                float step = _speed * Time.deltaTime;
                if (Vector3.Distance(owner.position, target) > 0.03f)
                {
                    owner.position = Vector3.MoveTowards(owner.position, target, step);
                }
                else
                {
                    MapData data = _unit.MyTilemap.GetMapData(owner.position);
                    _unit.Actions -= 1 / data.speedModificator;
                    counter++;
                    _unit.RefreshBar();
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}