using Unity.Cinemachine;
using UnityEngine;

public class DynamicSizeCamera : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private SpeedPlayerManager speedPlayerManager;
    private Rigidbody2D rbPlayer;

    [SerializeField] private CinemachineCamera cinemachineCamera;
    private float originalSizeOrtho;
    private float maxSizeOrtho;

    private float targetSizeOrtho;
    float smoothSpeed = 3f;

    void Start()
    {
        originalSizeOrtho = cinemachineCamera.Lens.OrthographicSize;
        maxSizeOrtho = originalSizeOrtho * 1.5f;

        speedPlayerManager = player.GetComponent<SpeedPlayerManager>();
        rbPlayer = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!GameManager.instance.inGame)
            return;

        float _speed = rbPlayer.linearVelocity.magnitude / speedPlayerManager.maxSpeed;
        targetSizeOrtho = Mathf.Lerp(originalSizeOrtho, maxSizeOrtho, _speed);
        
        cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(
            cinemachineCamera.Lens.OrthographicSize,
            targetSizeOrtho,
            Time.deltaTime * smoothSpeed
        );
    }
}
