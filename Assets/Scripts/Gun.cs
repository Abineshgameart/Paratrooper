using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    // Private
    [SerializeField] private InputActionReference fireActionReference;

    // Public
    [Header("Reference")]
    public Transform firepoint;
    public GameObject bulletPrefab;


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
        Instantiate(bulletPrefab, firepoint.position, firepoint.rotation);
    }
}
