using UnityEngine;

public class AIBotController : MonoBehaviour
{
    public float speed = 5f; // Speed of the AI bot
    public float detectionRange = 10f; // Range within which the AI bot can detect the player
    public Transform player; // Reference to the player's transform
    public GameObject explosion; // Reference to the explosion GameObject
    public Sprite damagedSprite; // Reference to the damaged sprite
    public Sprite normalSprite; // Reference to the normal sprite
    public int RewardOnKill = 5;

    private Rigidbody2D rb;
    private Vector2 roamPosition;
    private SpriteRenderer spriteRenderer;
    
    private int health = 3;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found in the scene.");
        }
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
            
            // Check for walls in the direction of movement
            if (!IsWallInDirection(direction))
            {
                rb.AddForce(direction * speed, ForceMode2D.Force);
            }
        }
        else
        {
            // Roam around the map
            roamPosition = GetRoamingPosition();
            Vector2 direction = (roamPosition - (Vector2)transform.position).normalized;
            
            // Check for walls in the direction of movement
            if (!IsWallInDirection(direction))
            {
                rb.AddForce(direction * speed, ForceMode2D.Force);
            }
        }
        // Limit the speed
        if (rb.velocity.magnitude > speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
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

    private bool IsWallInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange);
        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            // A wall is detected in the direction of movement
            return true;
        }
        return false;
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
                // Increase the score by 1 (or any other value) when the enemy dies
                ScoreManager.instance.IncreaseScore(RewardOnKill);
                Instantiate(explosion, transform.position, Quaternion.identity);
                Destroy(gameObject);
    }
}
