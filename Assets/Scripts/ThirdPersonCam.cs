using Cinemachine;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform lookDirection;
    public Transform combatLookAt;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    [Header("Cam Settings")]
    public CameraStyle camStyle;
    public GameObject basicCam;
    public GameObject combatCam;

    public enum CameraStyle
    {
        Basic, Combat
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.F3))
        {
            if (basicCam.activeInHierarchy)
            {
                basicCam.SetActive(false);
                combatCam.SetActive(true);
            }
            else if (combatCam.activeInHierarchy)
            {
                combatCam.SetActive(false);
                basicCam.SetActive(true);
            }
            else
            {
                basicCam.SetActive(true);
            }
        }

        // Rotate look direction of player
        Vector3 viewDirection = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        lookDirection.forward = viewDirection.normalized;

        if (camStyle == CameraStyle.Basic)
        {
            // Rotate player object
            float hInput = Input.GetAxisRaw("Horizontal");
            float vInput = Input.GetAxisRaw("Vertical");
            Vector3 inputDirection = lookDirection.forward * vInput + lookDirection.right * hInput;

            if (inputDirection != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDirection.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if (camStyle == CameraStyle.Combat)
        {
            Vector3 combatViewDirection = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            lookDirection.forward = combatViewDirection.normalized;
            playerObj.forward = combatViewDirection.normalized;
        }
    }
}
