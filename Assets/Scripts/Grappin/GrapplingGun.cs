using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    [SerializeField] private RopeGrabingGun ropeGrapplin;

    [Header("Refs")]
    public Transform firePoint;
    private Camera cam;
    [SerializeField] private Rigidbody2D rb;

    [Header("Pull Grappin Settings")]
    [SerializeField] private LayerMask layers;
    [SerializeField] private float maxDistance = 50.0f;
    [SerializeField] private float distanceRelease = 5f;
    [SerializeField] private KeyCode key;
    private SpringJoint2D joint;

    [Header("Private Params & States:")]
    [HideInInspector] public Vector2 grappledPoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    private bool isGrappled;
    [HideInInspector] public bool isGrappedToNothing;

    [SerializeField] private float ropeShrinkSpeed = 10f;

    void Start()
    {
        joint = gameObject.AddComponent<SpringJoint2D>();
        joint.enabled = false;  
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
        if (Input.GetKey(key) && isGrappled && !isGrappedToNothing)
        {
            float _currentDistance = Vector2.Distance(transform.position, grappledPoint);

            if (joint.distance > distanceRelease)
            {
                joint.distance -= ropeShrinkSpeed * Time.deltaTime;
            }

            if (joint.distance < _currentDistance)
            {
                joint.distance = _currentDistance;
            }
        }
    }

    void Grapple(Vector2 _pos)
    {
        isGrappled = true;
        grappledPoint = _pos;

        ropeGrapplin.enabled = true;

        joint.enabled = true;
        joint.autoConfigureDistance = false;
        joint.enableCollision = true;

        float _distance = Vector2.Distance(transform.position, grappledPoint);

        joint.distance = _distance; // distance fixe
        joint.connectedAnchor = grappledPoint;

        joint.dampingRatio = 0.5f; // stabilité
        joint.frequency = 1.5f;    // rigidité
    }

    public void Pull()
    {
        if (isGrappedToNothing)
            UnGrapple();
    }

    public void UnGrapple()
    {
        isGrappled = false;
        joint.enabled = false;

        grappledPoint = Vector2.zero;
        ropeGrapplin.enabled = false;
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
