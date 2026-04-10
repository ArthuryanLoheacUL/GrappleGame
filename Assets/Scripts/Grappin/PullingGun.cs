using Unity.VisualScripting;
using UnityEngine;

public class PullingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    [SerializeField] private RopePullingGun ropeGrapplin;

    [Header("Refs")]
    public Transform firePoint;
    private Camera cam;
    [SerializeField] private Rigidbody2D rb;

    [Header("Pull Grappin Settings")]
    [SerializeField] private LayerMask layers;
    [SerializeField] private float distanceRelease = 5f;
    [SerializeField] private float pullForce;
    [SerializeField] private KeyCode key;
    const float MAX_DIST = 20f;

    [Header("Private Params & States:")]
    [HideInInspector] public Vector2 grappledPoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    private bool isGrappled;
    [HideInInspector] public bool isGrappedToNothing;
    private bool isPulling;

    

    void Start()
    {
        cam = Camera.main;
        UnGrapple();
    }

    bool IsPointOnScreen(Vector2 _point)
    {
        Vector3 _viewportPos = Camera.main.WorldToViewportPoint(_point);

        return _viewportPos.z > 0 &&
            _viewportPos.x >= 0 && _viewportPos.x <= 1 &&
            _viewportPos.y >= 0 && _viewportPos.y <= 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Vector3 _mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _direction = _mousePos - firePoint.position;
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, _direction.normalized, Mathf.Infinity, layers);

            if (_hit.collider && IsPointOnScreen(_hit.point))
            {
                isGrappedToNothing = false;
                Grapple(_hit.point);
            } else
            {
                isGrappedToNothing = true;
                Vector2 _newDir = new Vector2(_direction.normalized.x * MAX_DIST, _direction.normalized.y * MAX_DIST);
                Vector2 _newPos = new Vector2(firePoint.position.x + _newDir.x, firePoint.position.y + _newDir.y);
                Grapple(_newPos);
            }
        }
        if (Input.GetKeyUp(key))
        {
            UnGrapple();
        }
        if (Input.GetKey(key) && isGrappled && isPulling && !isGrappedToNothing)
        {
            float _distance = Vector2.Distance(grappledPoint, transform.position);
            if (_distance < distanceRelease)
            {
                UnGrapple();
            }
            else
            {
                Vector2 _dir = new Vector2(grappledPoint.x - transform.position.x, grappledPoint.y - transform.position.y);
                rb.linearVelocity += (_dir.normalized) * pullForce * Time.deltaTime;
            }
        }
    }

    void Grapple(Vector2 _pos)
    {
        isGrappled = true;
        grappledPoint = _pos;
        ropeGrapplin.enabled = true;
        grappleDistanceVector = new Vector2(_pos.x - firePoint.position.x, _pos.y - firePoint.position.y).normalized;
    }

    public void Pull()
    {
        isPulling = true;
        if (isGrappedToNothing)
            UnGrapple();
    }

    public void UnGrapple()
    {
        isGrappled = false;
        grappledPoint = Vector2.zero;
        ropeGrapplin.enabled = false;
        isPulling = false;
        grappleDistanceVector = Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        if (isGrappled)
            Gizmos.DrawSphere(grappledPoint, 0.1f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, distanceRelease);
    }
}
