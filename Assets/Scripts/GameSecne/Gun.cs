using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    // Private
    [SerializeField] private InputActionReference fireActionReference;
    [SerializeField] private AudioManager audioManager;

    // Public
    [Header("Reference")]
    public Transform firepoint;
    public GameObject bulletPrefab;

    public ScoreSystem scoreSystem;

    public int startingBullets = 25;
    private int bullet;

    private void Start()
    {
        bullet = startingBullets;
    }

    private void Update()
    {
        LoadingAmmos();
    }

    void LoadingAmmos()
    {
        if (startingBullets <= 0)
        {
            bullet = scoreSystem.score;
        }
        
    }

    private void OnEnable()
    {
        fireActionReference.action.Enable();
        fireActionReference.action.performed += ctx => Shooting();
    }

    private void OnDisable()
    {
        fireActionReference.action.performed -= ctx => Shooting();
        fireActionReference.action.Disable();
    }


    void Shooting()
    {
        if (bullet > 0)
        {
            audioManager.PlaySFX(audioManager.Shooting);
            Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
            bullet--;
            if (startingBullets > 0)
            {
                startingBullets--;
            }
        }
        
    }
}
