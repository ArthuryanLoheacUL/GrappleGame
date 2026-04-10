using UnityEditor.SceneManagement;
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
    [SerializeField] private float maxDistance = 20f;
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
            } else
            {
                InitJointGrapple();
            }
        }
    }

    void StartGrapple()
    {
        Vector2 _direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position).normalized;

        RaycastHit2D _hit = Physics2D.Raycast(player.position, _direction, maxDistance, whatIsGrappleable);
        if (_hit.collider != null)
        {
            grapplePoint = _hit.point;
            isGrapped = true;
            isGrappedToNothing = false;

            ropeGrappin.enabled = true;

            distanceWithGrappedPoint = Vector2.Distance(player.transform.position, grapplePoint);
            grappleDistanceVector = new Vector2(grapplePoint.x - firePoint.position.x, grapplePoint.y - firePoint.position.y).normalized;
        } else
        {
            isGrappedToNothing = true;
        }
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

        // En 2D, connectedAnchor est un Vector2
        joint.connectedAnchor = grapplePoint;

        float _distanceFromPoint = Vector2.Distance(player.position, grapplePoint);

        // Distance que le grappin va maintenir
        joint.distance = _distanceFromPoint;

        // Paramètres physiques
        joint.frequency = 4.5f;   // équivalent du "spring"
        joint.dampingRatio = 0.7f; // équivalent du "damper"
    }

    public void StopGrapple()
    {
        ropeGrappin.enabled = false;
        isGrapped = false;
        grappleDistanceVector = Vector2.zero;
        Destroy(joint);
    }
}
