using UnityEditor.SceneManagement;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GrapplingGun : MonoBehaviour
{
    [Header("Refs:")]
    private RopeGrabingGun ropeGrappin;
    public Transform firePoint;
    public Transform player;

    [Header("Datas:")]
    const float MAX_DIST = 20f;
    public LayerMask whatIsGrappleable;
    [SerializeField] private KeyCode key;

    [Header("Private Params")]
    [HideInInspector] public Vector2 grappleDistanceVector;
    [HideInInspector] public Vector3 grapplePoint;
    [HideInInspector] public bool isGrappedToNothing = false;
    [HideInInspector] public bool isActive = false;
    private bool isGrapped = false;
    private float distanceWithGrappedPoint = 0f;
    private SpringJoint2D joint;

    void Awake()
    {
        ropeGrappin = GetComponent<RopeGrabingGun>();
    }

    private void Start()
    {
        isGrapped = false;
        ropeGrappin.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            StartGrapple();
            if (GetComponent<GunSound>())
                GetComponent<GunSound>().PlayOnShoot();
        }
        else if (Input.GetKeyUp(key))
        {
            StopGrapple();
        }
        if (isGrapped && joint == null && isActive)
        {
            float _distance = Vector2.Distance(player.transform.position, grapplePoint);

            if (_distance <= distanceWithGrappedPoint)
            {
                distanceWithGrappedPoint = _distance;
            } else if (_distance - distanceWithGrappedPoint >= 0.1f)
            {
                InitJointGrapple();
            }
        }
    }

    bool IsPointOnScreen(Vector2 _point)
    {
        Vector3 _viewportPos = Camera.main.WorldToViewportPoint(_point);

        return _viewportPos.z > 0 &&
            _viewportPos.x >= 0 && _viewportPos.x <= 1 &&
            _viewportPos.y >= 0 && _viewportPos.y <= 1;
    }

    void StartGrapple()
    {
        Vector2 _direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position).normalized;

        RaycastHit2D _hit = Physics2D.Raycast(player.position, _direction, Mathf.Infinity, whatIsGrappleable);
        
        if (_hit.collider != null && IsPointOnScreen(_hit.point))
        {
            isGrappedToNothing = false;
            Grapple(_hit.point);

            distanceWithGrappedPoint = Vector2.Distance(player.transform.position, grapplePoint);
        } else
        {
            isGrappedToNothing = true;
            Vector2 _newDir = new Vector2(_direction.normalized.x * MAX_DIST, _direction.normalized.y * MAX_DIST);
            Grapple(new Vector2(firePoint.position.x + _newDir.x, firePoint.position.y + _newDir.y));
        }
    }

    void Grapple(Vector2 _point)
    {
        ropeGrappin.enabled = true;
        isGrapped = true;
        grapplePoint = _point;
        grappleDistanceVector = new Vector2(grapplePoint.x - firePoint.position.x, grapplePoint.y - firePoint.position.y).normalized;
    }

    public void Activate()
    {
        isActive = true;
    }

    void InitJointGrapple()
    {
        joint = player.gameObject.AddComponent<SpringJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.enableCollision = true;
        joint.connectedAnchor = grapplePoint;

        float _distanceFromPoint = Vector2.Distance(player.position, grapplePoint);
        joint.distance = _distanceFromPoint;
        joint.frequency = 4.5f;
        joint.dampingRatio = 0.7f;
    }

    public void StopGrapple()
    {
        ropeGrappin.enabled = false;
        isActive = false;
        isGrapped = false;
        grappleDistanceVector = Vector2.zero;
        Destroy(joint);
    }

    public void DestroyJoint(Collision2D _collision)
    {
        Destroy(joint);
        distanceWithGrappedPoint = Vector2.Distance(player.transform.position, grapplePoint);
    }
}
