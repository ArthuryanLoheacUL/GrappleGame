using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Blinking : MonoBehaviour
{
    private TMP_Text text;
    float speed = 3f;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        Color _c = text.color;  
        _c.a = (Mathf.Sin(Time.time * speed) + 1) / 2;
        text.color = _c;
    }
}
