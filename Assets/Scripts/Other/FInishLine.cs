using UnityEngine;

public class FInishLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.tag == "Player")
        {
            GameManager.instance.GameEnd(true);
            _collision.gameObject.GetComponent<ActivePlayer>().enabled = false;
        }
    }
}
