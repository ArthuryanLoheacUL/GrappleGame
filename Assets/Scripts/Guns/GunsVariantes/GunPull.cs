using UnityEngine;

public class GunPull : GunScript
{
    [SerializeField] private float distanceRelease;
    [SerializeField] private float pullForce;
    [SerializeField] private Rigidbody2D rb;

    protected override void Grapple(Vector2 _pos)
    {
        base.Grapple(_pos);
    }

    public override void UnGrapple()
    {
        if (isGrappled)
            SoundManager.instance.PlayOneShot("ChainRelease", 3, 0.1f);
        base.UnGrapple();
    }   

    protected override void OnActive()
    {
        base.OnActive();
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
