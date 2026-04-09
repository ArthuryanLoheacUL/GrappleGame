using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 20f;
    private SpringJoint2D joint;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple()
    {
        Vector2 _direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position).normalized;

        RaycastHit2D _hit = Physics2D.Raycast(player.position, _direction, maxDistance, whatIsGrappleable);
        if (_hit.collider != null)
        {
            grapplePoint = _hit.point;

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

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }
}
