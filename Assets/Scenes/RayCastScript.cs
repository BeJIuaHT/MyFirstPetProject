using UnityEngine;

public class RayCastScript : MonoBehaviour
{
    Camera cam;
    public LayerMask mask;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Draw Ray
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D test = Physics2D.OverlapPoint(mousePos, mask);

            if (test != null)
            {
                Debug.Log(test.gameObject.name +" в позиции "+ test.transform.position);
                                
            }
        }
    }
}