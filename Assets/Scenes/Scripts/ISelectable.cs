using UnityEngine;

namespace Assets.Scenes.Scripts
{


    public interface ISelectable
    {
        public void Selected();
        public void DeSelected();
        public Transform GetTransform();
        public Vector3 GetPosition();
        public float GetRange();
        public void LockPosition();
        public void RemovePosition();
    }
}