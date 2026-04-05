using UnityEngine;

public class SlingShot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputManager Input;
    [SerializeField] private Transform Object;

    [Header("Line Renderer")]
    [SerializeField] private LineRenderer line;

    [Header("Trajectory Line")]
    [SerializeField] private LineRenderer trajectoryLine;
    [SerializeField] private int trajectoryPoints = 30;
    [SerializeField] private float timeStep = 0.1f;

    [Header("Settings")]
    [SerializeField] private float maxDragDistance = 2f;
    [SerializeField] private float forceMultiplier = 10f;
    [SerializeField] private LayerMask layerMask;

    private Vector2 anchorPoint;
    private Vector2 direction;
    private bool isDragging;
    private Rigidbody2D rb;

    // ================= INIT =================
    void Start()
    {
        Subscribe();
    }

    void Subscribe()
    {
        Input.Click += ClickOn;
    }

    void Unsubscribe()
    {
        Input.Click -= ClickOn;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    // ================= GIZMOS =================
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(anchorPoint, 0.1f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(anchorPoint, maxDragDistance);

        if (isDragging)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(anchorPoint, anchorPoint + direction);
        }
    }

    // ================= LINE =================
    void DrawLine()
    {
        if (line == null) return;

        line.positionCount = 2;
        line.SetPosition(0, anchorPoint);
        line.SetPosition(1, Object.position);

        float distance = Vector2.Distance(anchorPoint, Object.position);
        line.textureMode = LineTextureMode.Tile;
        line.material.mainTextureScale = new Vector2(distance * 10f, 1);
    }

    void ClearLine()
    {
        if (line == null) return;
        line.positionCount = 0;
    }

    // ================= TRAJECTORY =================
    void DrawTrajectory()
    {
        if (trajectoryLine == null || rb == null) return;

        trajectoryLine.positionCount = trajectoryPoints;

        Vector2 startPos = Object.position;
        Vector2 velocity = (-direction * forceMultiplier) / rb.mass;

        for (int i = 0; i < trajectoryPoints; i++)
        {
            float t = i * timeStep;

            Vector2 pos = startPos +
                          velocity * t +
                          0.5f * Physics2D.gravity * t * t;

            trajectoryLine.SetPosition(i, pos);
        }

        float distance = Vector2.Distance(Object.position, trajectoryLine.GetPosition(trajectoryPoints - 1));
        trajectoryLine.textureMode = LineTextureMode.Tile;
        trajectoryLine.material.mainTextureScale = new Vector2(distance * 10f, 1);
    }

    void ClearTrajectory()
    {
        if (trajectoryLine == null) return;
        trajectoryLine.positionCount = 0;
    }

    // ================= INPUT =================
    void ClickOn(bool click, Vector2 mousePosition)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(mousePosition);

        if (click)
        {
            // ✅ FIX: pakai OverlapPoint + LayerMask
            Collider2D hit = Physics2D.OverlapPoint(worldPoint, layerMask);

            if (hit != null)
            {
                if (!isDragging)
                {
                    Object = hit.transform;
                    rb = Object.GetComponent<Rigidbody2D>();

                    if (rb == null) return;

                    anchorPoint = Object.position;
                    rb.isKinematic = true;
                }

                isDragging = true;
            }

            if (isDragging && Object != null)
            {
                direction = worldPoint - anchorPoint;
                direction = Vector2.ClampMagnitude(direction, maxDragDistance);

                // smooth move
                rb.MovePosition(anchorPoint + direction);

                DrawLine();
                DrawTrajectory();
            }
        }
        else
        {
            if (isDragging && Object != null && rb != null)
            {
                rb.isKinematic = false;
                rb.linearVelocity = Vector2.zero;

                rb.AddForce(-direction * forceMultiplier, ForceMode2D.Impulse);

                ClearLine();
                ClearTrajectory();

                isDragging = false;
            }
        }
    }
}