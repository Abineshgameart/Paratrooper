using UnityEngine;

public class ShooterCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Paratrooper":
                {
                    Destroy(gameObject);
                    break;
                }
            case "EnemyBullet":
                {
                    Destroy(collision.gameObject);
                    Destroy(gameObject);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

}
