using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.Tilemaps;

public class PrintCord : MonoBehaviour
{
    [SerializeField] public Tilemap tm;
    [SerializeField] public Camera cam;
    Vector3Int start;
    Vector3Int end;

    public static void CrateNewCord()
    {
        GameObject Canvas = GameObject.Find("Canvas");
        GameObject TextCord = GameObject.Find("TextCord");
        var tilemap = GameObject.Find("Playble");
        var List = tilemap.GetComponent<MyTilemap>();
        // list = List.locPosTile;
        
        for (int i = 0; i < List.WorldPosTile.Count; i++)
        {
         
            GameObject newCord = Instantiate(TextCord, List.WorldPosTile[i], Quaternion.Euler(0, 0, 0)) as GameObject;
            newCord.transform.SetParent(Canvas.transform);
            newCord.name = List.UnitsMap[i].ToString();
            newCord.transform.position = List.WorldPosTile[i];
            newCord.GetComponent<TextMeshProUGUI>().text = List.UnitsMap[i]._position.x.ToString()+ " " + List.UnitsMap[i]._position.y.ToString();

        }
        
    }

    void Start()
    {
        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
    }


    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            ChengeColor(Color.white, MousePos(tm, cam));
            SetRange(16, MousePos(tm, cam));
            start = MousePos(tm, cam); 
       
        }
        if (Input.GetMouseButtonDown(1))
        {
            ChengeColor(Color.black, MousePos(tm, cam));
           print( AxialDistance((new Vector3Int(-3, 0 ,0)), (MousePos(tm, cam))));
            PathFinding path = new();
            end = MousePos(tm, cam);
           



        }
   


    }

    public Vector3Int MousePos(Tilemap tm, Camera cam)
    {
       // Camera cam = Camera.main;
        Vector3Int v = tm.WorldToCell(cam.ScreenToWorldPoint(Input.mousePosition));
        v.z = 0;
        return v;
    }
    public void ChengeColor(Color color, Vector3Int vec)
    {
        

        GameObject targetText = GameObject.Find(vec.ToString());
        if (targetText != null)
        { targetText.transform.GetComponent<TextMeshProUGUI>().color = color; }

    }
    public void SetRange(int range, Vector3Int cord)
    {
        print("Set renge" + range + cord.ToString());
        int r = range;
        List<Vector3Int> list = new List<Vector3Int>() {cord};
        for (int i = 0; i <= r; i++)
        {
            foreach (Vector3Int v in list.ToList())
            {
                foreach (Vector3Int v2 in FindNebethod(v).ToList())
                {
                    if (list.Contains(v2))
                    {

                    }
                    else
                    {

                        GameObject TextCord = GameObject.Find(v2.ToString());
                        if (TextCord == null) { }
                        else
                        {
                            list.Add(v2);
                            TextCord.GetComponent<TextMeshProUGUI>().text = v2.x.ToString() + " " + v2.y.ToString() + "\n" + (i+1);
                        }
                    }

                }
            }
        }
        
    }
    public void CengeColorCircl(List<Vector3Int> vec, Color color)
    {
        List<Vector3Int> v = vec;
        for (int i = 0 ; i < v.Count; i++) {
            ChengeColor(color, v[i]);
        }
       


    }
    public List<Vector3Int> FindNebethod(Vector3Int v)
    {
        int x;
        int y;
        List<Vector3Int> list = new List<Vector3Int>();
        list.Add(v);
        for (x = -1; x <= 1; x++)
        {
            Vector3Int forx = new Vector3Int(v.x + x, v.y, v.z);
            if (list.Contains(forx)) { }
            else { list.Add(forx); }
        }
        for (y = -1; y <= 1; y++)
        {
            Vector3Int fory = new Vector3Int(v.x, v.y + y, v.z);
            if (list.Contains(fory)) { }
            else { list.Add(fory); }
        }
        if ((v.y % 2) == 0)
        {
            list.Add(new Vector3Int(v.x - 1, v.y + 1, v.z));
            list.Add(new Vector3Int(v.x - 1, v.y - 1, v.z));
        }
        else
        {
            list.Add(new Vector3Int(v.x + 1, v.y + 1, v.z));
            list.Add(new Vector3Int(v.x + 1, v.y - 1, v.z));
        }
            return list;
    }
    public void FindDistancesTo()
    {
        StopAllCoroutines();
        StartCoroutine(Search());
    }

    IEnumerator Search()
    {
        WaitForSeconds delay = new WaitForSeconds(1f);
        for (int i = 0; i < 5; i++)
        {

            yield return delay;
           

        }
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
