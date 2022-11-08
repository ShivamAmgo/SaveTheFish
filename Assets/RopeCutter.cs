
using UnityEngine;

public class RopeCutter : MonoBehaviour
{
    bool Runningineditor = false;
    private void Start()
    {
        if (Application.isEditor)
            Runningineditor = true;
        Debug.Log(Runningineditor);
    }
    private void Update()
    {
        if (!Runningineditor)
        {

        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit2D Rayhit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),Vector2.zero);
                if (Rayhit.collider!=null && Rayhit.transform.tag == "Link")
                {
                    Destroy(Rayhit.collider.gameObject);

                }
            }
        }
    }
}
