using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private bool isDisabled = false;
    [SerializeField] private float delay = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDisabled)
        {
            // Handle the health pickup logic here
            // For example: collision.GetComponent<PlayerController>().IncreaseHealth(10);

            // Disable the pickup
            DisablePickup();

            // Re-enable the pickup after number of seconds
            Invoke("EnablePickup", delay);
        }
    }

    private void DisablePickup()
    {
        isDisabled = true;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
    }

    private void EnablePickup()
    {
        isDisabled = false;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
    }
}
