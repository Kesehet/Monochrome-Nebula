using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveItem : MonoBehaviour
{
    public GameObject explosionPrefab; // assign your Explosion prefab in the inspector
    public float explosionForce = 1000f; // adjust the force as needed
    public float explosionRadius = 5f; // adjust the radius as needed
    private Vector3 initialPosition; // to store the initial position of the barrel

    private void Start()
    {
        initialPosition = transform.position; // store the initial position of the barrel
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the "Projectile" tag
        if (collision.gameObject.tag == "Projectile")
        {
            CameraShakeManager.instance.Shake(1f); // Shakes the camera with an intensity of 0.5
            
            // Instantiate the explosion prefab at the barrel's position
            Instantiate(explosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosion>().isBarrelExplosion = true;;

            // Find all colliders in the explosion radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

            // Apply a force to all rigidbodies within the explosion radius
            foreach (Collider2D hit in colliders)
            {
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();

                AIBotController bc = hit.GetComponent<AIBotController>();
                if(bc != null)
                {
                    bc.Die();
                }
                PlayerHealth ph = hit.GetComponent<PlayerHealth>();
                if (ph != null)
                {
                    ph.Die();
                }

                if (rb != null)
                {
                    // Calculate direction from the explosion position to the object
                    Vector2 explosionDirection = rb.transform.position - transform.position;

                    // Apply force to this object
                    rb.AddForce(explosionDirection.normalized * explosionForce);
                }
            }

            // Disable the barrel instead of destroying it
            gameObject.SetActive(false);

            // Respawn the barrel after 15 seconds
            Invoke("RespawnBarrel", 15f);
        }
    }

    private void RespawnBarrel()
    {
        // Check if there's another Rigidbody2D in the respawn location
        Collider2D otherRigidbody = Physics2D.OverlapCircle(initialPosition, 0.3f, LayerMask.GetMask("Default"));

        // If there's another Rigidbody2D detected, delay the respawn by an additional 15 seconds
        if (otherRigidbody && otherRigidbody.GetComponent<Rigidbody2D>())
        {
            Invoke("RespawnBarrel", 15f);
            return;
        }

        // Reset the barrel's position and re-enable it
        transform.position = initialPosition;
        gameObject.SetActive(true);
    }

}
