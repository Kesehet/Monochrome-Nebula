using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUpgrade : MonoBehaviour
{
    private bool isDisabled = false;
    [SerializeField] private float delay = 5f;

    public ProjectileLauncher pl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDisabled)
        {
            // Handle the Gun Swap logic here
            pl.upgradeGunType();

            // Disable the pickup
            DisablePickup();

            // Re-enable the pickup after 15 seconds
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
