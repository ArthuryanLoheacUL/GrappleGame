using UnityEngine;

abstract public class GunScript : MonoBehaviour
{
    [Header("Scripts Ref:")]
    [SerializeField] private RopeGun rope;

    [Header("Refs")]
    public Transform firePoint;
    private Camera cam;

    [Header("Pull Grappin Settings")]
    [SerializeField] private LayerMask layers;
    [SerializeField] private KeyCode[] keys;
    const float MAX_DIST = 20f;

    [Header("Private Params & States:")]
    [HideInInspector] public Vector2 grappledPoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    protected bool isGrappled;
    [HideInInspector] public bool isGrappedToNothing;
    private bool isActive;

    void Start()
    {
        cam = Camera.main;
        UnGrapple();
    }

    bool IsPointOnScreen(Vector2 _point)
    {
        Vector3 _viewportPos = Camera.main.WorldToViewportPoint(_point);

        return _viewportPos.z > 0 &&
            _viewportPos.x >= 0 && _viewportPos.x <= 1 &&
            _viewportPos.y >= 0 && _viewportPos.y <= 1;
    }

    RaycastHit2D GetSnappedShoot(Vector2 _direction)
    {
        float[] _angles = { -5, -4, -3, -2, -1, 1, 2, 3, 4, 5 };
        RaycastHit2D _originalHit = Physics2D.Raycast(firePoint.position, _direction.normalized, Mathf.Infinity, layers);
        RaycastHit2D _minHit = _originalHit;
        float _minDistance = float.MaxValue;
        if (_minHit.collider)
            _minDistance = _minHit.distance;

        foreach (var _angle in _angles)
        {
            Vector2 _newVec = Quaternion.Euler(0, 0, _angle) * _direction;
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, _newVec.normalized, Mathf.Infinity, layers);
            if (_hit.collider && _hit.distance < _minDistance)
            {
                _minDistance = _hit.distance;
                _minHit = _hit;
            }
        }
        if (_originalHit.collider && Vector2.Distance(_minHit.point, _originalHit.point) <= 2f)
            return _originalHit;
        return _minHit;
    }

    void Shoot()
    {
        SoundManager.instance.PlayOneShot("Shoot", 3, 0.1f);

        Vector2 _direction = cam.ScreenToWorldPoint(Input.mousePosition) - firePoint.position;
        RaycastHit2D _hit = GetSnappedShoot(_direction);

        if (_hit.collider && IsPointOnScreen(_hit.point))
        {
            isGrappedToNothing = false;
            Grapple(_hit.point);
        }
        else
        {
            isGrappedToNothing = true;
            Vector2 _newDir = new Vector2(_direction.normalized.x * MAX_DIST, _direction.normalized.y * MAX_DIST);
            Vector2 _newPos = new Vector2(firePoint.position.x + _newDir.x, firePoint.position.y + _newDir.y);
            Grapple(_newPos);
        }
    }

    bool IsKeysDown()
    {
        foreach (var _key in keys)
        {
            if (Input.GetKeyDown(_key))
            {
                return true;
            }
        }
        return false;
    }

    bool IsKeysUp()
    {
        foreach (var _key in keys)
        {
            if (Input.GetKeyUp(_key))
            {
                return true;
            }
        }
        return false;
    }

    bool IsKeys()
    {
        foreach (var _key in keys)
        {
            if (Input.GetKey(_key))
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (IsKeysDown())
        {
            Shoot();
        }
        if (IsKeysUp())
        {
            UnGrapple();
        }
        if (IsKeys() && isGrappled && isActive && !isGrappedToNothing)
        {
            OnActive();
        }
    }

    virtual protected void OnActive()
    {

    }

    public void Activate()
    {
        isActive = true;
        if (isGrappedToNothing)
            UnGrapple();
    }
    virtual protected void Grapple(Vector2 _pos)
    {
        isGrappled = true;
        grappledPoint = _pos;
        rope.enabled = true;
        grappleDistanceVector = new Vector2(_pos.x - firePoint.position.x, _pos.y - firePoint.position.y).normalized;
    }
    virtual public void UnGrapple()
    {
        isGrappled = false;
        grappledPoint = Vector2.zero;
        rope.enabled = false;
        isActive = false;
        grappleDistanceVector = Vector2.zero;
    }
}
