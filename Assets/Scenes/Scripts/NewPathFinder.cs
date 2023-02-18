using Assets.Scenes.Map;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NewPathFinder
{
    private int limit = 1000;
    private Vector2Int Start;
    private Vector2Int End;
    private Unit MyUnit;
    private Tilemap MyTilemap;
    private MyTilemap MyMap;
    private List<NewPathNode> OpenList = new();
    private List<NewPathNode> CloseList = new();
    public List<Vector3> finalPath = new List<Vector3>();
    private Func<Vector2Int, MapData> getMapData;
    private Stopwatch stopWatch = new Stopwatch();
    
    public NewPathFinder(Tilemap myTilemap, MyTilemap myMap, Unit unit)
    {
        MyTilemap = myTilemap;
        MyMap = myMap;
        MyUnit = unit;
    }

    public List<Vector3> FindePath(Vector3 start, Vector3 end)
    {
        Start = Convert(MyTilemap.WorldToCell(start));
        End = Convert(MyTilemap.WorldToCell(end));
        OpenList.Clear();
        CloseList.Clear();
        finalPath.Clear();
        NewPathNode tempNode = new((Vector3Int)Start, (Vector3Int)Start, 0f, 0f);
        limit = 1000;
        OpenList.Add(tempNode);
        stopWatch.Start();
        while (OpenList.Count > 0)
        {
            if (OpenList.Count > limit)
            {
                UnityEngine.Debug.LogWarning("Поиск пути упёрся в лимит");
                break;
            }
            if (CloseList.Count > limit)
            {
                UnityEngine.Debug.LogWarning("Поиск пути упёрся в лимит");
                break;
            }
            NewPathNode curentNode = FindeMinFcost(OpenList);
            if (curentNode.end == End)
            {
                stopWatch.Stop();
                // Get the elapsed time as a TimeSpan value.
                TimeSpan ts = stopWatch.Elapsed;

                // Format and display the TimeSpan value.
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                UnityEngine.Debug.Log("RunTime " + elapsedTime);
                return GetFinalPath(curentNode);
            }

            OpenList.Remove(curentNode);
            CloseList.Add(curentNode);
            foreach (NewPathNode neighbour in FindNeighbour(curentNode))
            {
                if (CloseList.Exists(x => x.start == neighbour.end)) continue;
                if (!OpenList.Contains(neighbour))
                {
                    OpenList.Add(neighbour);
                    MyMap.ChangeTileColor((Vector3Int)neighbour.end, Color.red);
                }

            }

        }
        return null;

    }
    private List<Vector3> GetFinalPath(NewPathNode endNode)
    {
        //stopWatch.Stop();
        //// Get the elapsed time as a TimeSpan value.
        //TimeSpan ts = stopWatch.Elapsed;

        //// Format and display the TimeSpan value.
        //string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
        //    ts.Hours, ts.Minutes, ts.Seconds,
        //    ts.Milliseconds / 10);
        //UnityEngine.Debug.Log("RunTime " + elapsedTime);
        //stopWatch.Start();

        NewPathNode curentNode = endNode;
        finalPath.Add(new Vector3(curentNode.end.x, curentNode.end.y, 0f));
        while (curentNode.start != Start)
        {
            List<NewPathNode> temp = CloseList.FindAll(x => x.end == curentNode.start);
            curentNode = FindeMinFcost(temp);
            finalPath.Add(new Vector3(curentNode.end.x, curentNode.end.y, 0f));
        }

        finalPath.Reverse();
        return finalPath;

    }
    private NewPathNode FindeMinFcost(List<NewPathNode> list)
    {
        NewPathNode bestNode = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].fCost < bestNode.fCost)
            {
                bestNode = list[i];
            }
        }
        return bestNode;
    }

    private List<NewPathNode> FindNeighbour(NewPathNode curentNode)
    {
        List<NewPathNode> neighbour = new();
        foreach (Vector3Int pos in MyMap.FindNebethod((Vector3Int)curentNode.end, out getMapData))
        {
            Vector2Int vec = new(pos.x, pos.y);
            if (CloseList.Exists(x => x.start == vec)) continue;
            MapData data = getMapData(vec);
            if (data.isWalkable == false) continue;
            if (data.hasUnit != null) continue;
            float gCost = 1f;
            gCost /= data.speedModificator;
            gCost += curentNode.gCost;
            
            neighbour.Add(new NewPathNode((Vector3Int)curentNode.end, pos, ManhattaDistance(pos, End, data.speedModificator), gCost));
        }
        return neighbour;
    }
    public void Init(Tilemap tilemap, MyTilemap myMap, Unit unit)
    {
        MyTilemap = tilemap;
        MyUnit = unit;
        MyMap = myMap;

    }
    private float ManhattaDistance(Vector3Int position, Vector2Int target, float speed)
    {

        float x = math.abs(position.x - target.x);
        float y = math.abs(position.y - target.y);
        float hCost = (x + y) / speed;
        return hCost;
    }
    private Vector2Int Convert(Vector3 vector)
    {
        Vector3Int temp = MyTilemap.WorldToCell(vector);
        Vector2Int vec = new Vector2Int(temp.x, temp.y);
        return vec;
    }
    private Vector2Int Convert(Vector3Int vector)
    {
        Vector2Int vec = new Vector2Int(vector.x, vector.y);
        return vec;
    }

    private void test()
    {
       
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
       
    }
}
