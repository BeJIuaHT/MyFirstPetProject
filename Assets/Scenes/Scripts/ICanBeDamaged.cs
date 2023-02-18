using System.Reflection;
using UnityEngine;

namespace Assets.Scenes.Scripts
{
    public interface ICanBeDamaged
    {
        public void TackeDamage(GameObject atacekr, float damage);
    }
}