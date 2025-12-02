using UnityEngine;

public enum KeyType
{
    Red, Green, Black
}

public class KeyInventory : MonoBehaviour
{
    public bool HasRedKey  { get; private set; }
    public bool HasGreenKey { get; private set; }
    public bool HasBlackKey { get; private set; }

    public void AddKey(KeyType keyType)
    {
        switch (keyType)
        {
            case KeyType.Red:
                HasRedKey = true;
                break;
            case KeyType.Green:
                HasGreenKey = true;
                break;
            case KeyType.Black:
                HasBlackKey = true;
                break;
        }

        Debug.Log($"Picked up {keyType} key.");
    }
}
