using UnityEngine;

public class FlyOnCollidePlayer : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speedJumpFactorMin = 5.0f;
    [SerializeField] private float speedJumpFactorMax = 10.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        float _s = Random.Range(transform.localScale.x - 0.1f, transform.localScale.x + 0.1f);
        transform.localScale = new Vector3(_s, _s, _s);
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.tag == "Player")
        {
            Rigidbody2D _playerRB = _collision.gameObject.GetComponent<Rigidbody2D>();
            Vector2 _vel = _playerRB.linearVelocity * Random.Range(-1f, 1f);
            _vel = (_vel.normalized) + Vector2.up;
            float _speed = Random.Range(speedJumpFactorMin, speedJumpFactorMax);
            rb.linearVelocity = _vel.normalized * _speed * _playerRB.linearVelocity.magnitude;
        }
    }
}
