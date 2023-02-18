using System.Collections;
using UnityEngine;

namespace Assets.Scenes.Map
{
    public struct MapData
    {
        public bool isWalkable;
        public float speedModificator;
        public Unit hasUnit;
        public Vector3 worldPosition;
        public Vector3Int tilePosition;
    }
}