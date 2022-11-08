
using UnityEngine;

public class LinkDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            Debug.Log("HitGround");
            Destroy(gameObject);
        }

    }
}
