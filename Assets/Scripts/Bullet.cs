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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        Destroy(gameObject);
    }
}
