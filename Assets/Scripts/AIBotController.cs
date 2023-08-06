using UnityEngine;

public class AIBotController : MonoBehaviour
{
    public float speed = 5f; // Speed of the AI bot
    public float detectionRange = 10f; // Range within which the AI bot can detect the player
    public Transform player; // Reference to the player's transform
    public GameObject explosion; // Reference to the explosion GameObject
    public Sprite damagedSprite; // Reference to the damaged sprite
    public Sprite normalSprite; // Reference to the normal sprite

    private Rigidbody2D rb;
    private Vector2 roamPosition;
    private SpriteRenderer spriteRenderer;
    private int health = 3;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            // Chase the player
            Vector2 direction = (player.position - transform.position).normalized;
            rb.AddForce(direction * speed, ForceMode2D.Force);
        }
        else
        {
            // Roam around the map
            roamPosition = GetRoamingPosition();
            Vector2 direction = (roamPosition - (Vector2)transform.position).normalized;
            rb.AddForce(direction * speed, ForceMode2D.Force);
        }

        // Flip the sprite based on the direction of movement:
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Moving right
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(1, -1, 1); // Moving left
        }
    }

    private Vector2 GetRoamingPosition()
    {
        // Define the roaming area as a square around the AI bot's starting position
        // You can adjust this to fit your game's map
        float roamRange = 20f;
        return new Vector2(Random.Range(-roamRange, roamRange), Random.Range(-roamRange, roamRange));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile" || collision.gameObject.tag == "Explosion")
        {
            health--;

            if (health == 2)
            {
                spriteRenderer.sprite = damagedSprite;
            }
            else if (health == 1)
            {
                spriteRenderer.sprite = normalSprite;
            }
            else if (health <= 0)
            {
                Die();
            }
        }
    }
    public void Die(){
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
    }
}
