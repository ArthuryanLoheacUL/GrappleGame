using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private Vector2 parallaxFactor;

    private Vector3 lastCameraPosition;

    void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("CineCam").transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 _delta = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(_delta.x * parallaxFactor.x, _delta.y * parallaxFactor.y, 0);
        lastCameraPosition = cameraTransform.position;
    }
}
