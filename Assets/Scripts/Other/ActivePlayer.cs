using UnityEngine;

public class ActivePlayer : MonoBehaviour
{
    [SerializeField] private GameObject[] toDesactivate;
    [SerializeField] private FinalCameraPos finalCameraPos;

    private void OnDisable()
    {
        foreach(GameObject _obj in toDesactivate)
            _obj.SetActive(false);
        GetComponent<DieFromSpeed>().enabled = false;
        GetComponent<SpeedPlayerManager>().enabled = false;
        PathPlayerAnalyser.instance.StopRecording();
        finalCameraPos.ActiveFinalCam();
    }
}
