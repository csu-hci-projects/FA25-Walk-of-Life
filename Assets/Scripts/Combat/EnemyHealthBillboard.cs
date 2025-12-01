using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera cam;

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (cam == null) return;

        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
}

