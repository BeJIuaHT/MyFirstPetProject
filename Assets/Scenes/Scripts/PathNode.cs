using UnityEngine;

public struct PathNode
{

    public Vector3Int startPos;
    public Vector3Int endPos;
    public float ves;
    public PathNode(Vector3Int startPos, Vector3Int endPos, float ves)
    {
        this.startPos = startPos; 
        this.endPos = endPos;
        this.ves = ves;
    }

    public override string ToString()
    {
      return (startPos.x + " " + startPos.y + "->" + ves + "->" + endPos.x + " " + endPos.y);  
    }
}
