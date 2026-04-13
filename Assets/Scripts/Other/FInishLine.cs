using UnityEngine;

public class FInishLine : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D _collision)
    {
        if (_collision.tag == "Player" && _collision.bounds.Contains(_collision.transform.position))
        {
            if (GameManager.instance.inGame)
                SoundManager.instance.PlayOneShot("Finish", 0);
            GameManager.instance.GameEnd(true);
            _collision.gameObject.GetComponent<ActivePlayer>().enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.tag == "Player" && _collision.bounds.Contains(_collision.transform.position))
        {
            if (GameManager.instance.inGame)
                SoundManager.instance.PlayOneShot("Finish", 0);
            GameManager.instance.GameEnd(true);
            _collision.gameObject.GetComponent<ActivePlayer>().enabled = false;
        }
    }
}
