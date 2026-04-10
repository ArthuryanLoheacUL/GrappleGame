using UnityEngine;

public class BreakGrabRopeOnCollide : MonoBehaviour
{
    [SerializeField] private GunGrab grabGun;

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (grabGun != null)
        {
            grabGun.DestroyJoint(_collision);
        }
    }
}
