using UnityEngine;

public class SpeedZone : MonoBehaviour
{
    [SerializeField] float speedBoost = 2f;
    [SerializeField] Vector2 directionBoost = Vector2.left;

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.tag == "Player" && _collision.bounds.Contains(_collision.transform.position))
        {
            SpeedBoostPlayer(_collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.tag == "Player" && _collision.bounds.Contains(_collision.transform.position))
        {
            SpeedBoostPlayer(_collision.gameObject);
        }
    }
    
    void SpeedBoostPlayer(GameObject _gameObject)
    {
        Rigidbody2D _rb = _gameObject.GetComponent<Rigidbody2D>();
        Vector2 _directionCurrent = _rb.linearVelocity;
        _directionCurrent += directionBoost.normalized * speedBoost * Time.deltaTime;
        _rb.linearVelocity = _directionCurrent;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 _pos = transform.position;
        Gizmos.DrawLine(transform.position, _pos + directionBoost * 10f);
    }
}
