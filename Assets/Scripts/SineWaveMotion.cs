using UnityEngine;

public class SineWaveMotion : MonoBehaviour
{
    public float frequency = 50f;
    public float magnitude = 40f;

    private Rigidbody2D rb;
    private Vector2 initialDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialDirection = rb.velocity.normalized;
    }

    void FixedUpdate()
    {
        float wave = Mathf.Sin(Time.time * frequency) * magnitude;
        Vector2 force = new Vector2(-initialDirection.y, initialDirection.x) * wave;
        rb.AddForce(force);
    }
}
