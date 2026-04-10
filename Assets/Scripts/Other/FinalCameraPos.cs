using Unity.Cinemachine;
using UnityEngine;

public class FinalCameraPos : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private float originalOrthographicSize = 12f;

    [Header("Final Pos")]
    [SerializeField] private Vector2 finalPosition = Vector2.zero;
    [SerializeField] private float finalOrthographicSize = 0f;

    [Header("Animation")]
    [SerializeField] private float animationTime = 1.5f;
    [SerializeField] private AnimationCurve animationMovement;
    private float timer;
    private bool onTransition = false;

    private Vector2 startPosAnimation = Vector2.zero;
    private float startOrthoSizeAnimation = 0f;


    private void OnDrawGizmosSelected()
    {
        DrawCameraRect(finalPosition, finalOrthographicSize, Camera.main.aspect, Color.blue);
    }

    private void DrawCameraRect(Vector3 _center, float _orthoSize, float _aspect, Color _color)
    {
        float _height = _orthoSize * 2f;
        float _width = _height * _aspect;

        Vector3 _topLeft = _center + new Vector3(-_width / 2, _height / 2, 0);
        Vector3 _topRight = _center + new Vector3(_width / 2, _height / 2, 0);
        Vector3 _bottomLeft = _center + new Vector3(-_width / 2, -_height / 2, 0);
        Vector3 _bottomRight = _center + new Vector3(_width / 2, -_height / 2, 0);

        Gizmos.color = _color;

        Gizmos.DrawLine(_topLeft, _topRight);
        Gizmos.DrawLine(_topRight, _bottomRight);
        Gizmos.DrawLine(_bottomRight, _bottomLeft);
        Gizmos.DrawLine(_bottomLeft, _topLeft);
    }

    public void ActiveFinalCam()
    {
        cinemachineCamera.Follow = null;
        startPosAnimation = cinemachineCamera.transform.position;
        startOrthoSizeAnimation = cinemachineCamera.Lens.OrthographicSize;
        onTransition = true;
        timer = 0;
    }

    public void ResetCam(GameObject _player)
    {
        cinemachineCamera.Lens.OrthographicSize = originalOrthographicSize;
        cinemachineCamera.Follow = _player.transform;
    }

    void Update()
    {
        if (onTransition)
        {
            timer += Time.deltaTime;
            float _t = animationMovement.Evaluate(timer / animationTime);
            if (timer >= animationTime)
            {
                _t = 1f;
                onTransition = false;
            }
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(startOrthoSizeAnimation, finalOrthographicSize, _t);
            Vector3 _finalPos = Vector3.Lerp(startPosAnimation, finalPosition, _t);
            _finalPos.z = -10;
            cinemachineCamera.transform.position = _finalPos;
        }
    }
}
