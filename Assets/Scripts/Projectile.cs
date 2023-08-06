using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool explodesOnImpact;
    private bool splitsIntoMoreBullets;
    private int numberOfBounces;

    public GameObject explosionPrefab;

    public void SetProperties(bool explodesOnImpact, bool splitsIntoMoreBullets, int numberOfBounces)
    {
        this.explodesOnImpact = explodesOnImpact;
        this.splitsIntoMoreBullets = splitsIntoMoreBullets;
        this.numberOfBounces = numberOfBounces;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        
        if(explodesOnImpact){
            Explode();
        }
        else{
            Destroy(gameObject);
        }

        
    }
    private void Explode(){
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
