using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using MyProject.Utilites;


public class PathFinding
{
    private Tilemap _tilemap;
     public List<PathNode> closedList { get; private set; } = new();
    public List<PathNode> rangeList { get; private set; } = new();
    private PathNode pathnode;
    private MyTilemap _myTilemap;
    public Vector3Int start;
    public Vector3Int end;
    public float range;
    private List<Vector3Int> veclist = new();
    


    public void InitPathFinding()
    {
        if (_tilemap == null || _myTilemap == null)
        {
            _tilemap = GameObject.Find("Playble").GetComponent<Tilemap>();
            _myTilemap = _tilemap.GetComponent<MyTilemap>();
        }
    }
    public void FindPath(Unit unit, Vector3 direction)
    {
        this.start = _tilemap.WorldToCell(unit.GetPosition());
        this.end = _tilemap.WorldToCell(direction);
        this.range = unit.GetRange();
        int test = 0;

        if (!_tilemap.HasTile(start) || !_tilemap.HasTile(end)) 
        {
            Debug.Log("Сетка не имеет начала или конца");
           // closedList.Clear();
            return;
        }
        if (start == end) 
        { 
            Debug.Log("Вы уже на данной позиции");
            closedList.Clear();
            return;
        }
        if (closedList.Count > 0)
        {
            if (closedList.Last().endPos == end) return;
        }
        
            closedList.Clear();
            PathNode temp = GetRangeList(unit).Find(x => x.endPos == end);
            if (temp.endPos != temp.startPos)
            {
                closedList.Add(temp);
                while (temp.startPos != start || test > (5 + unit.GetRange()))
                {
                    foreach (PathNode pathnode in rangeList.ToList())
                    {

                        if (pathnode.endPos == temp.startPos)
                        {
                            if (closedList.Contains(pathnode) == false)
                            {
                                closedList.Add(pathnode);
                                temp = pathnode;
                                if (pathnode.startPos == start) return;
                            }
                        }

                    }
                }
            }
        
    }
    public List<PathNode> GetRangeList(Unit unit)
    {
            FindeRange(unit);
            return rangeList;
    }
    public List<PathNode> GetPathList(Unit unit, Vector3 dir)
    {
        FindPath(unit, dir);
        if (closedList.Count > 0) 
        { 
            if (closedList.Last().endPos != end)
                closedList.Reverse(); 
        }
        return closedList;
       
    }

    public void FindeRange( Unit Unit )
    {
        if (Unit.GetRange() > 0)
        {

            Vector3Int cord = _tilemap.WorldToCell(Unit.GetPosition());
            List<Vector3Int> test2 = new() { cord };
            if (rangeList.Count > 0)
            {
                if (rangeList[0].startPos == cord) return;
            }
            rangeList.Clear();
            veclist.Clear();
            for (int i = 0; i < Unit.GetRange(); i++)
            {

                foreach (Vector3Int v in test2.ToList())
                {
                    foreach (Vector3Int direction in _myTilemap.FindNebethod(v))
                    {
                        if (v != direction)
                        {
                            if (!veclist.Contains(direction))
                            {
                                veclist.Add(direction);
                                test2.Add(direction);
                                PathNode pathRange = new(v, direction, i);
                                rangeList.Add(pathRange);
                            }
                        }
                    }
                }
            }

        }
        else rangeList.Clear();
    }
    public Vector3Int AxialToOddr(Vector3Int hex)
    {
        int x = hex.x + (hex.y - (hex.y & 1)) / 2;
        int y = hex.y;
        hex.x = x;
        hex.y = y;
        return hex;
    }

    public Vector3Int OddrToAxial(Vector3Int hex)
    {
        int x = hex.x - (hex.y - (hex.y & 1)) / 2;
        int y = hex.y;
        hex.x = x;
        hex.y = y;
        return hex;
    }

    public int AxialDistance(Vector3Int hex1, Vector3Int hex2)
    {
        Vector3Int ahex1 = OddrToAxial(hex1);
        Vector3Int ahex2 = OddrToAxial(hex2);

        Vector3Int vec = (new Vector3Int(ahex1.x - ahex2.x, ahex1.y - ahex2.y, 0));
        return math.max(math.max(math.abs(vec.x), math.abs(vec.y)), math.abs(vec.x + vec.y));
    }
}
