using UnityEngine;

public class CursorHomeInMotion : MonoBehaviour
{
    public float speed = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (cursorPosition - (Vector2)transform.position).normalized;
        rb.velocity = direction * speed;
    }
}
