using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveItem : MonoBehaviour
{
    public GameObject explosionPrefab; // assign your Explosion prefab in the inspector
    public float explosionForce = 1000f; // adjust the force as needed
    public float explosionRadius = 5f; // adjust the radius as needed

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        // Check if the collided object has the "Projectile" tag
        if (collision.gameObject.tag == "Projectile")
        {
            // Instantiate the explosion prefab at the barrel's position
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

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

                if (rb != null)
                {
                    // Calculate direction from the explosion position to the object
                    Vector2 explosionDirection = rb.transform.position - transform.position;

                    // Apply force to this object
                    rb.AddForce(explosionDirection.normalized * explosionForce);
                }
            }

            // Destroy the barrel game object
            Destroy(gameObject);
        }
    }

}
