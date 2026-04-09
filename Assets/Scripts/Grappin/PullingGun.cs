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
    [SerializeField] private float maxDistance = 50.0f;
    [SerializeField] private float distanceRelease = 5f;
    [SerializeField] private float pullForce;
    [SerializeField] private KeyCode key;

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

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Vector3 _mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 _direction = _mousePos - firePoint.position;
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, _direction.normalized, Mathf.Infinity, layers);

            if (_hit)
            {
                float _distance = _hit.distance;
                if (_distance < maxDistance)
                {
                    isGrappedToNothing = false;
                    Grapple(_hit.point);
                }
                else
                {
                    isGrappedToNothing = true;
                    Vector2 _newDir = new Vector2(_direction.normalized.x * maxDistance, _direction.normalized.y * maxDistance);
                    Vector2 _newPos = new Vector2(firePoint.position.x + _newDir.x, firePoint.position.y + _newDir.y);
                    Grapple(_newPos);
                }
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
        Gizmos.DrawWireSphere(transform.position, maxDistance);
        Gizmos.DrawWireSphere(transform.position, distanceRelease);
    }
}
