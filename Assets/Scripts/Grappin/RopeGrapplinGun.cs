using UnityEngine;

public class RopeGrapplinGun : MonoBehaviour
{
    [Header("General Refernces:")]
    public GrapplingGun grapplingGun;
    public LineRenderer lineRenderer;

    [Header("General Settings:")]
    [SerializeField] private int percision = 40;
    [Range(0, 20)][SerializeField] private float straightenLineSpeed = 5;

    [Header("Rope Animation Settings:")]
    public AnimationCurve ropeAnimationCurve;
    [Range(0.01f, 4)][SerializeField] private float startWaveSize = 2;
    float waveSize = 0;

    [Header("Rope Progression:")]
    public AnimationCurve ropeProgressionCurve;
    [SerializeField][Range(1, 50)] private float ropeProgressionSpeed = 1;

    float moveTime = 0;

    [HideInInspector] public bool isGrappling = true;

    bool strightLine = true;

    private void OnEnable()
    {
        moveTime = 0;
        lineRenderer.positionCount = percision;
        waveSize = startWaveSize;
        strightLine = false;

        LinePointsToFirePoint();

        lineRenderer.enabled = true;
    }

    private void OnDisable()
    {
        lineRenderer.enabled = false;
        isGrappling = false;
    }

    private void LinePointsToFirePoint()
    {
        for (int _i = 0; _i < percision; _i++)
        {
            lineRenderer.SetPosition(_i, grapplingGun.firePoint.position);
        }
    }

    private void Update()
    {
        moveTime += Time.deltaTime;
        DrawRope();
    }

    void DrawRope()
    {
        if (!strightLine)
        {
            if (Mathf.Abs(lineRenderer.GetPosition(percision - 1).x - grapplingGun.grappledPoint.x) <= 0.02f)
            {
                strightLine = true;
                if (grapplingGun.isGrappedToNothing)
                    grapplingGun.UnGrapple();
            }
            else
            {
                DrawRopeWaves();
            }
        }
        else
        {
            if (!isGrappling)
            {
                grapplingGun.Pull();
                isGrappling = true;
            }
            if (waveSize > 0)
            {
                waveSize -= Time.deltaTime * straightenLineSpeed;
                DrawRopeWaves();
            }
            else
            {
                waveSize = 0;

                if (lineRenderer.positionCount != 2) { lineRenderer.positionCount = 2; }

                DrawRopeNoWaves();
            }
        }
    }

    void DrawRopeWaves()
    {
        for (int _i = 0; _i < percision; _i++)
        {
            float _delta = (float)_i / ((float)percision - 1f);
            Vector2 _offset = Vector2.Perpendicular(grapplingGun.grappleDistanceVector).normalized * ropeAnimationCurve.Evaluate(_delta) * waveSize;
            Vector2 _targetPosition = Vector2.Lerp(grapplingGun.firePoint.position, grapplingGun.grappledPoint, _delta) + _offset;
            Vector2 _currentPosition = Vector2.Lerp(grapplingGun.firePoint.position, _targetPosition, ropeProgressionCurve.Evaluate(moveTime) * ropeProgressionSpeed);

            lineRenderer.SetPosition(_i, _currentPosition);
        }
    }

    void DrawRopeNoWaves()
    {
        lineRenderer.SetPosition(0, grapplingGun.firePoint.position);
        lineRenderer.SetPosition(1, grapplingGun.grappledPoint);
    }
}
