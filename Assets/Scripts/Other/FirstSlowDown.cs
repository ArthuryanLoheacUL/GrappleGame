using UnityEngine;

public class FirstSlowDown : MonoBehaviour
{
    bool isAlreadyShown = false;
    DieFromSpeed dieFromSpeed;
    [SerializeField] private GameObject prefabUIWarning;

    private void Start()
    {
        isAlreadyShown = PlayerPrefs.GetInt("FirstSlowDown", 0) == 1;
        if (isAlreadyShown)
            Destroy(this);
        dieFromSpeed = GetComponent<DieFromSpeed>();
    }

    private void Update()
    {
        if (dieFromSpeed.timer >= dieFromSpeed.durationBeforeDeath / 3)
        {
            Instantiate(prefabUIWarning);
            PlayerPrefs.SetInt("FirstSlowDown", 1);
            Destroy(this);
        }
    }
}
