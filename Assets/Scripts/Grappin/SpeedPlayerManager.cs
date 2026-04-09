using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpeedPlayerManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Light2D lightBall;

    [SerializeField] private Color underSpeedColor;
    [SerializeField] private Color speedColor;
    [SerializeField] private Color maxSpeedColor;

    [SerializeField] private float speedThreshold;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxSpeedColorThreshold;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private AnimationCurve trailSpeed;
    [SerializeField] private AnimationCurve trailSpeedMax;
    [SerializeField] private AnimationCurve mergeTrailCurve;
    [SerializeField] private AnimationCurve lengthCurve;

    private enum StateSpeed
    {
        Slow,
        Normal,
        Max
    };
    private StateSpeed stateSpeed;

    void UpdateStateSpeed(float _magnitude)
    {
        if (_magnitude < speedThreshold)
        {
            stateSpeed = StateSpeed.Slow;
        }
        else if (_magnitude > maxSpeedColorThreshold)
        {
            stateSpeed = StateSpeed.Max;
        }
        else
        {
            stateSpeed = StateSpeed.Normal;
        }
    }

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
            UpdateStateSpeed(_magnitude);
            UpdateColor();
            UpdateTrail();
        }
    }

    void UpdateColor()
    {
        switch (stateSpeed)
        {
            case StateSpeed.Normal:
                SetColor(speedColor);
                break;
            case StateSpeed.Max:
                SetColor(maxSpeedColor);
                break;
            case StateSpeed.Slow:
                SetColor(underSpeedColor);
                break;
        }
    }

    AnimationCurve BlendCurves(AnimationCurve _c1, AnimationCurve _c2, float _weight, int _samples = 50)
    {
        if (_c1 == null || _c2 == null)
            return null;

        Keyframe[] _keys = new Keyframe[_samples];

        float _start = Mathf.Min(_c1.keys[0].time, _c2.keys[0].time);
        float _end = Mathf.Max(_c1.keys[_c1.length - 1].time, _c2.keys[_c2.length - 1].time);

        for (int _i = 0; _i < _samples; _i++)
        {
            float _t = Mathf.Lerp(_start, _end, (float)_i / (_samples - 1));

            float _v1 = _c1.Evaluate(_t);
            float _v2 = _c2.Evaluate(_t);

            float _blendedValue = Mathf.Lerp(_v1, _v2, _weight); // 0.2 = 20% curve2

            _keys[_i] = new Keyframe(_t, _blendedValue);
        }

        return new AnimationCurve(_keys);
    }

    void UpdateTrail()
    {
        if (stateSpeed == StateSpeed.Slow)
        {
            trailRenderer.enabled = false;
            lightBall.enabled = false;
        }
        else
        {
            trailRenderer.enabled = true;
            lightBall.enabled = true;
            if (stateSpeed == StateSpeed.Normal)
            {
                float _t = rb.linearVelocity.magnitude - speedThreshold / maxSpeedColorThreshold - speedThreshold;
                trailRenderer.widthCurve = BlendCurves(trailSpeed, trailSpeedMax, _t);
                trailRenderer.time = lengthCurve.Evaluate(mergeTrailCurve.Evaluate(_t));
            }
            else
            {
                trailRenderer.widthCurve = trailSpeedMax;
                trailRenderer.time = lengthCurve.Evaluate(1);
            }
        }
    }

    void SetColor(Color _color)
    {
        spriteRenderer.color = _color;
        lightBall.color = _color;
        _color.a = trailRenderer.startColor.a;
        trailRenderer.startColor = _color;
        _color.a = trailRenderer.endColor.a;
        trailRenderer.endColor = _color;
    }
}
