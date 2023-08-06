using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool explodesOnImpact;
    private bool splitsIntoMoreBullets;
    private int numberOfBounces;

    public GameObject explosionPrefab;
    public GameObject splitBulletPrefab;
    public int numberOfSplitBullets;

    public void SetProperties(bool explodesOnImpact, bool splitsIntoMoreBullets, int numberOfBounces)
    {
        this.explodesOnImpact = explodesOnImpact;
        this.splitsIntoMoreBullets = splitsIntoMoreBullets;
        this.numberOfBounces = numberOfBounces;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (splitsIntoMoreBullets && collision.gameObject.CompareTag("Wall"))
        {
            SplitBullet();
        }
        else if (explodesOnImpact)
        {
            Explode();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void SplitBullet()
    {
        for (int i = 0; i < numberOfSplitBullets; i++)
        {
            Instantiate(splitBulletPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
