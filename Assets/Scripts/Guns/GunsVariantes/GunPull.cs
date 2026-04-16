using UnityEngine;

public class GunPull : GunScript
{
    [SerializeField] private float distanceRelease;
    [SerializeField] private float pullForce;
    [SerializeField] private Rigidbody2D rb;

    bool isSoundActive = false;
    const int CANAL = 4;

    protected override void Grapple(Vector2 _pos)
    {
        base.Grapple(_pos);
    }

    public override void UnGrapple()
    {
        if (isGrappled)
            SoundManager.instance.PlayOneShot("ChainRelease", 3, 0.1f);
        SoundManager.instance.StopAudioCanal(CANAL);
        isSoundActive = false;
        base.UnGrapple();
    }   

    protected override void OnActive()
    {
        base.OnActive();
        if (!isSoundActive)
        {
            ActivateWebPullAudio();
        }
        UpdateVolumeWebPull();

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

    void ActivateWebPullAudio()
    {
        SoundManager.instance.PlayCanal("WebPull", CANAL, true);
        SoundManager.instance.GetCanal(CANAL).time = Random.Range(0, SoundManager.instance.GetDurationSound("WebPull"));
        isSoundActive = true;
    }

    void UpdateVolumeWebPull()
    {
        Vector2 _directionPull = grappledPoint - new Vector2(transform.position.x, transform.position.y);
        Vector2 _velocity = rb.linearVelocity;
        float _opposition = (1 - Vector3.Dot(_velocity.normalized, _directionPull.normalized)) / 2f;
        float _volume = _opposition * SoundManager.instance.GetVolumeSound("WebPull") + SoundManager.instance.GetVolumeSound("WebPull") * 0.25f;
        if (!GameManager.instance.inGame)
            _volume = 0;
        SoundManager.instance.SetVolumeCanal(CANAL, _volume);
    }

    private void OnDisable()
    {
        SoundManager.instance.SetVolumeCanal(CANAL, 0);
    }
}
