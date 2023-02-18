
using UnityEngine;

public struct NewPathNode 
{
    
    public Vector2Int start { get; set; }
    public Vector2Int end { get; set; }
    public float gCost { get; set; } 
    public float hCost { private get; set; }
    public float fCost { get; set; }
    
    public NewPathNode(Vector3Int start, Vector3Int end, float heuristicCost, float stepCost)
    {
        this.start = new Vector2Int(start.x, start.y);
        this.end = new Vector2Int(end.x, end.y);
        hCost = heuristicCost;
        gCost = stepCost;
        fCost = gCost + hCost;
    }
    public override string ToString()
    {
        return $"{start}-{fCost}-{end}";
    }

}
