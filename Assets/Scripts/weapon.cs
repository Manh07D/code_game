using UnityEngine;

public class weapon : MonoBehaviour
{
    public GameObject bulletPrefab;   // Kéo viên đạn vào đây từ Inspector
    public Transform firePoint;       // Điểm xuất phát của đạn
    public float bulletForce = 20f;

    private bool facingRight = true;  // ✅ Thêm dòng này để tránh lỗi CS0103

    void Update()
    {
        facingRight = transform.root.localScale.x > 0;

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 shootDir = facingRight ? firePoint.right : -firePoint.right;
        rb.AddForce(shootDir * bulletForce, ForceMode2D.Impulse);
    }
}
