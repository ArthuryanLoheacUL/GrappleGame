using UnityEngine;

public class SpeedPlayerManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color underSpeedColor;
    [SerializeField] private Color speedColor;
    [SerializeField] private Color maxSpeedColor;

    [SerializeField] private float speedThreshold;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxSpeedColorThreshold;

    [SerializeField] private Rigidbody2D rb;

    private void FixedUpdate()
    {
        if (rb != null)
        {
            float _magnitude = rb.linearVelocity.magnitude;
            if (_magnitude > maxSpeed)
            {
                Vector2 _newLinear = rb.linearVelocity.normalized * maxSpeed;
                rb.linearVelocity = _newLinear;
                _magnitude = maxSpeed;
            }
            Debug.Log(_magnitude);
            UpdateColor(_magnitude);
        }
    }

    void UpdateColor(float _magnitude)
    {
        if (_magnitude < speedThreshold)
        {
            spriteRenderer.color = underSpeedColor;
        }
        else if (_magnitude > maxSpeedColorThreshold)
        {
            spriteRenderer.color = maxSpeedColor;
        } else
        {
            spriteRenderer.color = speedColor;
        }
    }
}
