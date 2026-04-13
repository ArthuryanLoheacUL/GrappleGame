using UnityEngine;

public class ActivePlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] toDesactivate;
    [SerializeField] private FinalCameraPos finalCameraPos;
    [SerializeField] private GameObject prefabExplosion;

    private void OnDisable()
    {
        foreach(GameObject _obj in toDesactivate)
            _obj.SetActive(false);
        GetComponent<DieFromSpeed>().enabled = false;
        GetComponent<SpeedPlayerManager>().enabled = false;
        PathPlayerAnalyser.instance.StopRecording();
        finalCameraPos.ActiveFinalCam();
    }

    public void Die()
    {
        Instantiate(prefabExplosion, transform.position, Quaternion.identity);
        finalCameraPos.ActiveFinalCam();
        GameManager.instance.GameEnd(false);
        Destroy(gameObject);
    }
}
