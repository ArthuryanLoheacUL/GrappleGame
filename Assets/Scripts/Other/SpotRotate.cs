using UnityEngine;

public class SpotRotate : MonoBehaviour
{
    float originalRotate = 0f;
    [SerializeField] private float speed = 2;
    [SerializeField] private float angle = 5;


    void Start()
    {
        originalRotate = transform.rotation.eulerAngles.z;
    }

    private void Update()
    {
        Vector3 _euler = transform.eulerAngles;
        _euler.z = originalRotate + Mathf.Sin(Time.time * speed) * angle;
        transform.rotation = Quaternion.Euler(_euler);
    }
}
