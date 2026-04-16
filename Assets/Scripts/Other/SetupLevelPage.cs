using UnityEngine;
using UnityEngine.UI;

public class SetupLevelPage : MonoBehaviour
{
    [SerializeField] private Image image;
    private Vector2 originalPos = Vector2.zero;

    public void SetupPage(Level _level)
    {
        image.sprite = _level.sprite;
        originalPos = image.GetComponent<RectTransform>().anchoredPosition;
    }

    public void Load(float _value)
    {
        Vector2 _start = new Vector2(originalPos.x, originalPos.y - image.GetComponent<RectTransform>().rect.height);
        image.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(_start, originalPos, _value);
    }
}
