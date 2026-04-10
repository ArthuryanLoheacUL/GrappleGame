using UnityEngine;

public class InitialVelocity : MonoBehaviour
{
    [SerializeField] private Vector2 direction;
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    [Header("Prediction")]
    [SerializeField] private int steps = 30;
    [SerializeField] private float timeStep = 0.1f;

    private void Start()
    {
        rb.linearVelocity = direction.normalized * speed;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 _velocity = direction.normalized * speed;
        Vector2 _position = transform.position;

        Gizmos.color = Color.yellow;

        for (int _i = 0; _i < steps; _i++)
        {
            Vector2 _nextPosition = _position + _velocity * timeStep;

            Gizmos.DrawLine(_position, _nextPosition);

            _velocity += Physics2D.gravity * timeStep;

            _position = _nextPosition;
        }
    }
}