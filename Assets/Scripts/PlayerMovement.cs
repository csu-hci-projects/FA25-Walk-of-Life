using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float speed = 6.0f;
    [SerializeField] int debuf = 6000;
    [SerializeField] float mouse_sensitivity = 20f;
    [SerializeField] bool invertYaxis = false;
    [SerializeField] bool invertXaxis = false; 
    
    float xRotate = 0;
    float yRotate = 0;
    float x;
    float y;
    float z;
    void Start()
    {

        Debug.Log("Horizontal input " + Input.GetAxis("Horizontal"));
        Debug.Log("Vertical input " + Input.GetAxis("Vertical"));
        Cursor.lockState = CursorLockMode.Locked;
        x = 0;
        y = 0;
        z = 0;
    }

    // Update is called once per frame
    void Update()
    {

        float speedFactor = speed; // Time.deltaTime;
        Debug.Log("Horizontal input " + Input.GetAxis("Horizontal"));
        Debug.Log("Vertical input " + Input.GetAxis("Vertical"));
        Debug.Log("player position: X:" + x + " Y: " + y+ " Z: " + z);
        x = 0;
        z = 0;
        y = 0;
        // this input map will only work if there is 0 camera or player rotation Unless we can find a way for the player
        //  to remain stationary while the model and the camera rotate 
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Key code detected");
            x = x + (speedFactor + Input.GetAxis("Horizontal")) / debuf;
        }
        if ( Input.GetKey(KeyCode.A)) {
            x = x - (speedFactor + Input.GetAxis("Horizontal")) / debuf;
        }
        if (Input.GetKey(KeyCode.W))
        {
            z =  z + (speedFactor + Input.GetAxis("Vertical")) / debuf;

        }
        if (Input.GetKey(KeyCode.S))
        {
            z = z -(speedFactor - Input.GetAxis("Vertical")) / debuf;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            y = y + ( speedFactor + 1.0f )/200;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            y = y - (speedFactor + 1f) /200;

        }
        // player rotation

        float mouseX = Input.GetAxis("Mouse X") * mouse_sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouse_sensitivity * Time.deltaTime;
        if (invertXaxis)
        {
            xRotate -= mouseY;
        }
        else
        {
            xRotate += mouseY;
        }
        if (invertYaxis)
        {
            yRotate -= mouseX;
        }
        else
        {
            yRotate += mouseX;
        }
        xRotate = Mathf.Clamp(xRotate, -90f, 90f);
        //yRotate = Mathf.Clamp(yRotate,);
        transform.localRotation = quaternion.Euler(xRotate, yRotate, 0f);  

        transform.Translate(x, y, z);

    }
}
