using System.Dynamic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Assets.Scenes.Scripts
{
    [CreateAssetMenu]
    public class TileData : ScriptableObject
    {

        public TileBase[] Tiles;

        [SerializeField] public int test1;
        [SerializeField] public bool walckable;

    }
}
