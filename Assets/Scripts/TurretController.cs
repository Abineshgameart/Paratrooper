using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurretController : MonoBehaviour
{
    // Private
    private PlayerInput playerInput;
    private InputAction rotateAction;
    private float rotateVal;

    [Header("Rotation Values")]
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float minRotate = -90f;
    [SerializeField] private float maxRotate = 90f;

    private float currentRotation = 0f;

    // Public


    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        rotateAction = playerInput.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        TurretRotation();
    }

    private void TurretRotation()
    {
        rotateVal = rotateAction.ReadValue<float>();

        float rotateAmount = rotateVal * rotateSpeed * Time.deltaTime;
        currentRotation = Mathf.Clamp(currentRotation + rotateAmount, minRotate, maxRotate);

        transform.localRotation = Quaternion.Euler(0, 0, -currentRotation);
    }

}
