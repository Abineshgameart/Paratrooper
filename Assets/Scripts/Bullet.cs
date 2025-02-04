using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Public
    public float speed = 20f;
    public Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        Destroy(gameObject);

        if (collision.gameObject.CompareTag("Aircraft") || collision.gameObject.CompareTag("Paratrooper"))
        {
            Destroy(collision.gameObject);
        }
    }
}
