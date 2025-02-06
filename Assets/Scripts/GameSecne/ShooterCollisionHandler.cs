using UnityEngine;

public class ShooterCollisionHandler : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    public GameObject tryAgainMenuUI;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Paratrooper":
                {
                    TryAgainMenu();
                    audioManager.PlaySFX(audioManager.ShooterDestroy);
                    Destroy(gameObject);
                    break;
                }
            case "EnemyBullet":
                {
                    TryAgainMenu();
                    audioManager.PlaySFX(audioManager.ShooterDestroy);
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


    void TryAgainMenu()
    {

        tryAgainMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

}
