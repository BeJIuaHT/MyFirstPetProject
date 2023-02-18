using Assets.Scenes.Scripts;
using UnityEditor;
using UnityEngine;

namespace Assets.Scenes.Units.Abilities
{
    public interface IAbility : ISelectable
    {
        public abstract bool Use();
    }
}