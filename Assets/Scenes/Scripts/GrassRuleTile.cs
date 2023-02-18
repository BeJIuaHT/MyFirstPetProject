using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "My Rule Tile", menuName = "Terrain Tile Hex Samples/RPG 32x32/[A]_type3")]
public class GrassRuleTile : RuleTile<GrassRuleTile.Neighbor> {
    public bool customField;
    public Color _color;

    public GameObject _haveGameObject {private set; get; }

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Null: return tile == null;
            case Neighbor.NotNull: return tile != null;
        }
        return base.RuleMatch(neighbor, tile);
    }
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.flags = TileFlags.None;
        tileData.color = _color;
        
        base.GetTileData(position, tilemap, ref tileData);

    }
    public void SaveMeInTile(GameObject go)
    {
        _haveGameObject = go;
    }
    public void RemomeMeFromTile()
    {
        _haveGameObject = null;
    }

 
}