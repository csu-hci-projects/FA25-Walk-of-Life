using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public bool hasKey = false;

    private void Update()
    {
        if(Keyboard.current.rKey.wasPressedThisFrame) hasKey = !hasKey;
    }
}
