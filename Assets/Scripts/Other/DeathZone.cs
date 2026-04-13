using System.Drawing;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.tag == "Player" && _collision.bounds.Contains(_collision.transform.position))
        {
            _collision.gameObject.GetComponent<ActivePlayer>().Die();
        }
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.tag == "Player" && _collision.bounds.Contains(_collision.transform.position))
        {
            _collision.gameObject.GetComponent<ActivePlayer>().Die();
        }
    }
}
