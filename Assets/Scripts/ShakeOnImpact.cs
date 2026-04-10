using UnityEngine;

public class ShakeOnImpact : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float minSpeedShake;

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        Debug.Log("Collide " + rb.linearVelocity.magnitude.ToString() + "/" + minSpeedShake.ToString());
        if (rb.linearVelocity.magnitude  > minSpeedShake)
        {
            float _value = rb.linearVelocity.magnitude / 5f;
            Debug.Log(_value);
            CineShakeManager.instance.Shake(0.2f, _value);
        }
    }
}
