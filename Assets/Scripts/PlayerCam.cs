using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform lookDirection;

    float rotationX;
    float rotationY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from mouse and apply sensitivity to it
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // Calculate camera rotation
        rotationY += mouseX;
        rotationX = Mathf.Clamp(rotationX - mouseY, -90f, 90f);

        // Set and calculate rotation for cam and orientation
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        lookDirection.rotation = Quaternion.Euler(0f, rotationY, 0f);
    }
}
