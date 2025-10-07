using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float speed = 2.0f;
    float x;
    float y;
    float z;
    void Start()
    {

        Debug.Log("Horizontal input " + Input.GetAxis("Horizontal"));
        Debug.Log("Vertical input " + Input.GetAxis("Vertical"));
        x = 0;
        y = 0;
        z = 0;
    }

    // Update is called once per frame
    void Update()
    {

        float speedFactor = speed * Time.deltaTime;
        Debug.Log("Horizontal input " + Input.GetAxis("Horizontal"));
        Debug.Log("Vertical input " + Input.GetAxis("Vertical"));
        Debug.Log("player position" + x + y + z);
        if (!Input.GetKey(KeyCode.None))
        {
            x = speedFactor + Input.GetAxis("Horizontal") / 10;
            z = speedFactor + Input.GetAxis("Vertical") / 10;
            y = 0;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            y = speedFactor + 0.1f ;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            y = speedFactor - 0.1f ;

        }

        transform.Translate(x, y, z);

    }
}
