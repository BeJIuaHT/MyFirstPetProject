using System.Collections;
using UnityEngine;

namespace Assets.Scenes.Units.Abilities
{
    public abstract class Ability : IAbility
    {
        public int name { get; set; }
        public int description { get; set; }
        public Transform owner { get; set; }

        public Ability(int name, int description, Transform owner)
        {
            this.name= name;
            this.description = description;
            this.owner = owner;
        }

        public abstract bool Use();

        public void Selected()
        {
            throw new System.NotImplementedException();
        }

        public void DeSelected()
        {
            throw new System.NotImplementedException();
        }

        public Transform GetTransform()
        {
            return owner;
        }

        public Vector3 GetPosition()
        {
            return owner.position;
        }

        public float GetRange()
        {
            throw new System.NotImplementedException();
        }

        public void LockPosition()
        {
            throw new System.NotImplementedException();
        }

        public void RemovePosition()
        {
            throw new System.NotImplementedException();
        }
    }
}

