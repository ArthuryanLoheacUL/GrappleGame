using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData _e)
    {
        SoundManager.instance.PlayOneShot("Hover", 10);
    }

    public void OnPointerDown(PointerEventData _e)
    {
        SoundManager.instance.PlayOneShot("Click", 10);
    }
}
