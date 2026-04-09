using UnityEngine;

public class DisapearingSprite : MonoBehaviour
{
    public float timeDisapearing = 0.2f;
    private float timer = 0f;

    [SerializeField] private AnimationCurve alphaCurve;
    [SerializeField] private AnimationCurve scaleCurve;

    [SerializeField] private SpriteRenderer spriteRenderer;
    private float originalScale = 1f;

    void Start()
    {
        originalScale = transform.localScale.x;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeDisapearing )
        {
            Destroy(gameObject);
        }
        float _t = timer / timeDisapearing;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,
                                         alphaCurve.Evaluate(_t));
        transform.localScale = originalScale * new Vector3(scaleCurve.Evaluate(_t), scaleCurve.Evaluate(_t), scaleCurve.Evaluate(_t));
    }
}
