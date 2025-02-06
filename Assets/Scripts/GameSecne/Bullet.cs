using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Public
    public float speed = 20f;
    public Rigidbody2D rb;

    CameraShakeEffect cameraShakeEffect;
    GameObject audioManagerObj;
    AudioManager audioManager;
    ScoreSystem scoreSystem;
    
    // Start is called before the first frame update
    void Start()
    {
        audioManager = Camera.main.GetComponent<AudioManager>();
        scoreSystem = GameObject.FindGameObjectWithTag("Canvas").GetComponent<ScoreSystem>();
        rb.velocity = transform.up * speed;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        Destroy(gameObject);

        cameraShakeEffect = Camera.main.GetComponent<CameraShakeEffect>();

        switch(collision.gameObject.tag)
        {
            case "Aircraft":
                //StartCoroutine(cameraShakeEffect.Shake(0.2f, 0.3f));
                audioManager.PlaySFX(audioManager.ShipDestroy);
                Destroy(collision.gameObject);
                scoreSystem.score += 5;
                break;

            case "Paratrooper":
                audioManager.PlaySFX(audioManager.ParatrooperDestroy);
                Destroy(collision.gameObject);
                scoreSystem.score++;
                break;

            case "EnemyBullet":
                audioManager.PlaySFX(audioManager.BombDestroy);
                Destroy(collision.gameObject);
                scoreSystem.score += 5;
                break;

            default:
                break;
        }
        
    }
}
