using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private float moveInput;
    public Animator animator;
    private bool isGrounded;

    // lướt
    public float dashBoost;
    public float dashTime;
    private float _dashTime;
    private bool isDashing = false;
    public GameObject ghostEffect;
    public float ghostDelaySeconds;
    private Coroutine dashEffectCoroutine;
    private SpriteRenderer characterSR;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        characterSR = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Lấy input trái/phải
        moveInput = Input.GetAxis("Horizontal");

        // Gửi tốc độ vào animator
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Lật hướng
        if (moveInput != 0)
        {
            if (moveInput > 0)
                transform.localScale = new Vector3(0.2f, 0.2f, 1);
            else
                transform.localScale = new Vector3(-0.2f, 0.2f, 1);
        }

        // Gọi hàm nhảy
        HandleJump();

        // Lướt
        if (Input.GetKeyDown(KeyCode.Space) && _dashTime <= 0 && isDashing == false)
        {
            moveSpeed += dashBoost;
            _dashTime = dashTime;
            isDashing = true;
            StartDashEffect();
        }

        if (_dashTime <= 0 && isDashing == true)
        {
            moveSpeed -= dashBoost;
            isDashing = false;
            StopDashEffect();
        }
        else
        {
            _dashTime -= Time.deltaTime;
        }
    }

    void StartDashEffect()
    {
        if (dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
        dashEffectCoroutine = StartCoroutine(DashEffectCoroutine());
    }

    void StopDashEffect()
    {
        if (dashEffectCoroutine != null) StopCoroutine(dashEffectCoroutine);
    }

    IEnumerator DashEffectCoroutine()
    {
        while (true)
        {
            GameObject ghost = Instantiate(ghostEffect, transform.position, transform.rotation);
            Sprite currentSprite = characterSR.sprite;
            ghost.GetComponentInChildren<SpriteRenderer>().sprite = currentSprite;
            Destroy(ghost, 0.5f);
            yield return new WaitForSeconds(ghostDelaySeconds);
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}
