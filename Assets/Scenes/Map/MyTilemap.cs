using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using MyProject.Utilites;
using System;
using Assets.Scenes.Scripts;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.UI.CanvasScaler;
using UnityEngine.Rendering.Universal;
using Assets.Scenes.Map;

public class MyTilemap : MonoBehaviour

{
    [SerializeField] Color _unitColor;
    [SerializeField] Color _alliesColor;
    [SerializeField] Color _enemyColor;
    [SerializeField] List<Assets.Scenes.Scripts.TileData> _tilesData;
    public Camera camera;
    public GameObject thisGameObject = null;
    public List<ISelectable> AllSelectable { get; set; } = new();
    public List<Vector3> WorldPosTile { get; private set; } = new();
    public List<Vector3Int> LocPosTile { get; private set; } = new();
    public List<UnitsMap> UnitsMap { get; set; } = new();
    public Dictionary<Vector2Int, MapData> MapData { get; private set; } = new();
    public Tilemap TileMap { get; private set; } = null;
    private Tilemap _groundTileMap;
    private Tilemap _onGroundTileMap;
    private Tilemap _structTileMap;
    private List<PathNode> _pathRange;
    private List<PathNode> _path;
    private Dictionary<TileBase, Assets.Scenes.Scripts.TileData> _tileDictionary;
    private TileBase _curentTile;
    
    

    void Start()
    {
      //  collider = GetComponent<TilemapCollider2D>(); 
        TileMap = GetComponent<Tilemap>();

        camera = Lincks.GetCamera();
        _groundTileMap = GameObject.Find("Ground").GetComponent<Tilemap>();
        _onGroundTileMap = GameObject.Find("OnGrond").GetComponent<Tilemap>();
        _structTileMap = GameObject.Find("Structure").GetComponent<Tilemap>();
        //PrintCord.CrateNewCord();

        //MousePosition.MouseChangeTile.AddListener(DrawPath);
        _tileDictionary = new();
        foreach(var tiles in _tilesData)
        {
            foreach(var tile in tiles.Tiles)
            {
                _tileDictionary.Add(tile, tiles);
            }
        }
       
        InitMap();
        RefreshUnitMap();
    }

    private void InitMap()
    {
        for (int n = TileMap.cellBounds.xMin; n < TileMap.cellBounds.xMax; n++)
        {
            for (int p = TileMap.cellBounds.yMin; p < TileMap.cellBounds.yMax; p++)
            {
                
                MapData data = new();
                data.tilePosition = new Vector3Int(n, p, TileMap.cellBounds.z);
                data.worldPosition = TileMap.CellToWorld(data.tilePosition);
                
                data.speedModificator = 1f;
                if (TileMap.HasTile(data.tilePosition))
                {
                    if (_groundTileMap.HasTile(data.tilePosition))
                    {
                        _curentTile = _groundTileMap.GetTile(data.tilePosition);
                        _tileDictionary.TryGetValue(_curentTile, out Assets.Scenes.Scripts.TileData curentTileData) ;
                        if (curentTileData != null)
                        {
                            data.isWalkable = curentTileData.walckable;
                            data.speedModificator *= curentTileData.test1;
                        }
                        else data.isWalkable = false;
                    }
                    if (_onGroundTileMap.HasTile(data.tilePosition))
                    {
                        _curentTile = _onGroundTileMap.GetTile(data.tilePosition);
                        _tileDictionary.TryGetValue(_curentTile, out Assets.Scenes.Scripts.TileData curentTileData);
                        if (curentTileData != null)
                        {
                            data.isWalkable = curentTileData.walckable;
                            data.speedModificator *= curentTileData.test1;
                        }
                        
                    }
                    if (_structTileMap.HasTile(data.tilePosition))
                    {
                        _curentTile = _structTileMap.GetTile(data.tilePosition);
                        _tileDictionary.TryGetValue(_curentTile, out Assets.Scenes.Scripts.TileData curentTileData);
                        if (curentTileData != null)
                        {
                            data.isWalkable = curentTileData.walckable;
                            data.speedModificator *= curentTileData.test1;
                        }
                        
                    }
                    LocPosTile.Add(new Vector3Int(n, p, TileMap.cellBounds.z));
                    MapData.Add(new Vector2Int(n, p), data);
                    WorldPosTile.Add(data.worldPosition);
                }
                else
                {

                    //No tile at "place"
                }
            }
        }
    }

    public bool GetWalck(Vector3 vec)
    {
        Vector2Int v = (Vector2Int)TileMap.WorldToCell(vec);
        if (TileMap.HasTile((Vector3Int)v))
        {
            Debug.Log($"Модификатор скорости ={ MapData[v].speedModificator}");
            return MapData[v].isWalkable;
        }
        else
        {
            Debug.Log("Клик за границу карты");
            return false;
        }
    }
    public void DrawRangeOfPath(Unit unit, Vector3 dir )
    {
        _pathRange = unit.PathFinding.GetRangeList(unit);
        if (_pathRange.Count > 0)
        {
            foreach (PathNode node in _pathRange)
            {
                ChangeTileColor(node.endPos, _unitColor);
            }
        }
        else Debug.Log("Радиус лист = 0");
    }
    public void DrawPath(Unit unit, Vector3 dir)
    {

        _path = unit.PathFinding.GetPathList(unit, dir);
        if (_path.Count > 0)
        {
            DrawRangeOfPath(unit, dir);
            foreach (PathNode node in _path)
            {
                ChangeTileColor(node.endPos, _alliesColor);
            }
        }
        else Debug.Log("путь лист = 0");
    }
    
    public List<Vector3Int> FindNebethod(Vector3Int v)
    {

        List<Vector3Int> list = new();
        Vector3Int right = new(v.x+1, v.y, v.z);
        Vector2Int r = new(v.x + 1, v.y);
        if (MapData.ContainsKey(r))
        {
            MapData.TryGetValue(r, out MapData data);
            if (data.isWalkable && data.hasUnit == null)
            {
                list.Add(right);
            }
        }
        Vector3Int left = new(v.x-1, v.y, v.z);
        Vector2Int l = new(v.x - 1, v.y);
        if (MapData.ContainsKey(l))
        {
            MapData.TryGetValue(l, out MapData data);
            if (data.isWalkable && data.hasUnit == null)
            {
                list.Add(left);
            }
        }
        Vector3Int top = new(v.x, v.y+1, v.z);
        Vector2Int t = new(v.x, v.y+1);
        if (MapData.ContainsKey(t))
        {
            MapData.TryGetValue(t, out MapData data);
            if (data.isWalkable && data.hasUnit == null)
            {
                list.Add(top);
            }
        }
        Vector3Int down = new(v.x, v.y-1, v.z);
        Vector2Int d = new(v.x, v.y-1);
        if (MapData.ContainsKey(d))
        {
            MapData.TryGetValue(d, out MapData data);
            if (data.isWalkable && data.hasUnit == null)
            {
                list.Add(down);
            }
        }
        return list;
    }
    public List<Vector3Int> FindNebethod(Vector3Int v, out Func<Vector2Int, MapData> getMapData)
    {

        List<Vector3Int> list = new();
        getMapData = GetMapData;
        Vector3Int right = new(v.x + 1, v.y, v.z);
                list.Add(right);
        Vector3Int left = new(v.x - 1, v.y, v.z);
                list.Add(left);
        Vector3Int top = new(v.x, v.y + 1, v.z);
                list.Add(top);
        Vector3Int down = new(v.x, v.y - 1, v.z);
                list.Add(down);
        return list;
    }
    public MapData GetMapData(Vector2Int pos)
    {
        if (MapData.TryGetValue(pos, out MapData data)) return data;
        else
        {
            data.isWalkable = false;
            data.speedModificator = 1;
            data.tilePosition = new Vector3Int(pos.x, pos.y, 0);
            data.hasUnit = null;
            return data;
        }
    }
    public MapData GetMapData(Vector3 pos)
    {
        Vector3Int vec = TileMap.WorldToCell(pos);
        Vector2Int v = new(vec.x, vec.y);
        if (MapData.TryGetValue(v, out MapData data)) return data;
        else
        {
            data.isWalkable = false;
            data.speedModificator = 1;
            data.tilePosition = new Vector3Int(v.x, v.y, 0);
            data.hasUnit = null;
            return data;
        }
    }

    public void LockPosition(ISelectable unit)
    {

        Vector3Int pos = TileMap.WorldToCell(unit.GetPosition());
        Vector2Int key = new(pos.x, pos.y);
        MapData.TryGetValue(key, out MapData data);
        data.hasUnit = (Unit)unit;
        MapData[key] = data;

    }
    public void RemovePosition(Vector3 position)
    {
        Vector3Int pos = TileMap.WorldToCell(position);
        Vector2Int key = new(pos.x, pos.y);
        MapData.TryGetValue(key, out MapData data);
        data.hasUnit = null;
        MapData[key] = data;
    }

    public void RefreshUnitMap()
    {
        AllSelectable = new(15);
        foreach (Vector3Int v in LocPosTile)
        {
            ChangeTileColor(v, Color.white);
        }
        
        for (int i = 0; i < transform.childCount; i++)
        {
            
            if (transform.GetChild(i).TryGetComponent<ISelectable>(out ISelectable unit)) AllSelectable?.Add(unit);
        }
        foreach( ISelectable unit in AllSelectable)
        {
            ChangeTileColor(unit, _unitColor);
            unit.LockPosition();
        }
    }
    public void DrowUnitActionRange(Unit unit)
    {
       
        foreach (PathNode node in unit.PathFinding.rangeList)
        {
            ChangeTileColor( node.endPos, _alliesColor);
        }
        
    }
    public void DrowUnitAction(Unit unit)
    {
        DrowUnitActionRange(unit);
        foreach (PathNode node in unit.PathFinding.closedList)
        {
           
            ChangeTileColor(node.endPos, _unitColor);
            
        }

    }

    private void ChangeTileColor(ISelectable unit, Color color)
    {
        _groundTileMap.SetColor(TileMap.WorldToCell(unit.GetPosition()), color);
    }
    private void ChangeTileColor(Vector3 position, Color color)
    {
        _groundTileMap.SetColor(TileMap.WorldToCell(position), color);
    }
    public void ChangeTileColor(Vector3Int position, Color color)
    {
        _groundTileMap.SetColor(position, color);
    }
    private void ChangeTileColor(List<Vector3Int> position, Color color)
    {
        foreach (Vector3Int pos in position)
        {
            _groundTileMap.SetColor(pos, color);
        }
        
    }
    

}

public struct UnitsMap
{
    public Vector3Int _position;
    public ISelectable _unit;
    public UnitsMap(Vector3Int position, ISelectable unit = null)
    {
        _position = position;
        _unit = unit;
    }
}
