using UnityEngine;

public class GunGrab : GunScript
{
    private float distanceWithGrappedPoint = 0f;
    private SpringJoint2D joint;
    [SerializeField] private Transform player;

    protected override void Grapple(Vector2 _pos)
    {
        base.Grapple(_pos);
        distanceWithGrappedPoint = Vector2.Distance(player.transform.position, grappledPoint);
    }

    public override void UnGrapple()
    {
        Destroy(joint);
        base.UnGrapple();
    }

    protected override void OnActive()
    {
        base.OnActive();
        if (joint != null)
            return;
        float _distance = Vector2.Distance(player.transform.position, grappledPoint);

        if (_distance <= distanceWithGrappedPoint)
        {
            distanceWithGrappedPoint = _distance;
        }
        else if (_distance - distanceWithGrappedPoint >= 0.1f)
        {
            InitJointGrapple();
        }
    }
    void InitJointGrapple()
    {
        joint = player.gameObject.AddComponent<SpringJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.enableCollision = true;
        joint.connectedAnchor = grappledPoint;

        float _distanceFromPoint = Vector2.Distance(player.position, grappledPoint);
        joint.distance = _distanceFromPoint;
        joint.frequency = 4.5f;
        joint.dampingRatio = 0.7f;
    }
    public void DestroyJoint(Collision2D _collision)
    {
        Destroy(joint);
        distanceWithGrappedPoint = Vector2.Distance(player.transform.position, grappledPoint);
    }
}
