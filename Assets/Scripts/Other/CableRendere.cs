using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CableRendere : MonoBehaviour
{
    private Vector2 startPoint;
    private Vector2 endPoint;

    [Header("Cable Settings")]
    public int segmentCount = 20;
    public float sagAmount = 2f; // intensité de la gravité
    public float scale = 1f;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        startPoint = line.GetPosition(0);
        endPoint = line.GetPosition(1);

        line.positionCount = segmentCount;
        line.widthMultiplier = scale / 10;
    }

    void Update()
    {
        if (startPoint == null || endPoint == null) return;

        for (int _i = 0; _i < segmentCount; _i++)
        {
            float _t = _i / (float)(segmentCount - 1);

            // interpolation entre départ et arrivée
            Vector3 _point = Vector3.Lerp(startPoint, endPoint, _t);

            // ajout de la "gravité" (courbe vers le bas)
            float _sag = Mathf.Sin(_t * Mathf.PI) * sagAmount;

            _point.y -= _sag;

            line.SetPosition(_i, _point);
        }
    }
}
