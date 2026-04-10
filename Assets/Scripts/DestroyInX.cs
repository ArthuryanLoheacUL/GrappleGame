using UnityEngine;

public class DestroyInX : MonoBehaviour
{
    [SerializeField] private float destroyInX;

    void Start()
    {
        Destroy(gameObject, destroyInX);
    }
}
