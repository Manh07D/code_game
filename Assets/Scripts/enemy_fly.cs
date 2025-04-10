using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectDistance = 1f;
    public LayerMask obstacleLayer;
    private Vector2 moveDirection;

    private Camera mainCamera;
    private Vector2 screenMin;
    private Vector2 screenMax;

    private void Start()
    {
        PickNewDirection();
        mainCamera = Camera.main;
        UpdateCameraBounds();
    }

    private void Update()
    {
        // giới hạncamera di chuyển
        UpdateCameraBounds();

        // Di chuyển quái
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // phát hiện vật cản
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, detectDistance, obstacleLayer);

        // Vẽ ray trong Scene view
        Debug.DrawRay(transform.position, moveDirection * detectDistance, Color.red);

        if (hit.collider != null)
        {
            //Debug.Log("Va chạm với: " + hit.collider.name);
            PickNewDirection();
        }

        // Nếu ra khỏi màn hình → quay về giữa
        if (!IsWithinScreenBounds())
        {
            FlyBackToScreen();
        }
    }

    private void UpdateCameraBounds()
    {
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        screenMin = bottomLeft;
        screenMax = topRight;
    }

    private void PickNewDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

        if (moveDirection.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveDirection.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private bool IsWithinScreenBounds()
    {
        float margin = 0.5f;
        return transform.position.x >= screenMin.x - margin && transform.position.x <= screenMax.x + margin &&
               transform.position.y >= screenMin.y - margin && transform.position.y <= screenMax.y + margin;
    }

    private void FlyBackToScreen()
    {
        Vector2 center = (screenMin + screenMax) / 2f;
        moveDirection = (center - (Vector2)transform.position).normalized;

        if (moveDirection.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = moveDirection.x > 0 ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(moveDirection * detectDistance));
    }
}
