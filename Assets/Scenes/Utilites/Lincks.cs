using UnityEngine;
using UnityEngine.Tilemaps;

namespace MyProject.Utilites 
{


    public static class Lincks
    {

        public static GameObject Tilemap;
        public static GameObject Camera;
        public static Tilemap _tilemap;
        public static Camera _camera;
        public static Tilemap GetTilemap()
        {
            Tilemap = GameObject.Find("Playble");       
            _tilemap = Tilemap.GetComponent<Tilemap>();
            return _tilemap;
       

        }
        public static Camera GetCamera()
        {
            Camera = GameObject.Find("Camera");
            _camera = Camera.GetComponent<Camera>();
            return _camera;
        }
    }
}
