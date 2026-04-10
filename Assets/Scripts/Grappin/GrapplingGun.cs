using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, player;
    private float maxDistance = 20f;
    private SpringJoint2D joint;
    private Vector3 currentGrapplePosition;

    [SerializeField] private KeyCode key;
    private bool isGrapped = false;
    private float distanceWithGrappedPoint = 0f;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        isGrapped = false;
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
        if (isGrapped && joint == null)
        {
            float _distance = Vector2.Distance(player.transform.position, grapplePoint);
            Debug.Log("Dist : " +  _distance.ToString() + " Max " + distanceWithGrappedPoint.ToString());

            if (_distance <= distanceWithGrappedPoint)
            {
                distanceWithGrappedPoint = _distance;
            } else
            {
                InitJointGrapple();
            }
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    void StartGrapple()
    {
        Vector2 _direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position).normalized;

        RaycastHit2D _hit = Physics2D.Raycast(player.position, _direction, maxDistance, whatIsGrappleable);
        if (_hit.collider != null)
        {
            grapplePoint = _hit.point;
            isGrapped = true;

            distanceWithGrappedPoint = Vector2.Distance(player.transform.position, grapplePoint);

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
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

    void StopGrapple()
    {
        lr.positionCount = 0;
        isGrapped = false;
        Destroy(joint);
    }


    void DrawRope()
    {
        if (!isGrapped)
            return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
}
