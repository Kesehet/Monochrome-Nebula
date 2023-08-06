using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [Header("Projectile Properties")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float straightProjectileOffset = 0.1f;
    [SerializeField] private int numberOfBounces = 3;
    [SerializeField] private bool explodesOnImpact = false;
    [SerializeField] private bool splitsIntoMoreBullets = false;
    [SerializeField] private bool doubleBarrels = false;
    [SerializeField] private SpriteRenderer gunImage;
    [SerializeField] private Sprite[] gunSprites;

    [Header("Projectile Movement Patterns")]
    [SerializeField] private bool moveStraight = true;
    [SerializeField] private bool shotgun = false;
    [SerializeField] private bool moveInSineWave = false;
    [SerializeField] private bool homeInOnCursor = false;
    [SerializeField] private bool homeInOnEnemy = false;
    [SerializeField] private bool instantaneousBullets = false;
    [SerializeField] private bool bouncingBullets = false;

    
    public AudioClip[] Sounds; // Sound clip for barrel explosion
    private AudioSource audioSource; // Reference to the AudioSource component

    [Header("Firing Rate")]
    [SerializeField] private float fireRate = 1f; // Number of shots per second
    private float nextFireTime = 0f;

    [Header("Projectile List")]
    [SerializeField] private List<GameObject> projectiles = new List<GameObject>();

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;

            int gunshotIndex = 0;
            if (shotgun)
            {
                gunshotIndex = 1;
                // Example: Shoot 5 projectiles in a 90-degree range
                for (int i = -2; i <= 2; i++)
                {
                    float angle = 18 * i; // 18 degrees between each projectile
                    Quaternion rotation = Quaternion.Euler(0, 0, angle);
                    LaunchProjectile(direction, gunshotIndex);
                }
            }
            else if (moveInSineWave) {
                LaunchProjectile(direction, 2);
            }
            else
            {
                LaunchProjectile(direction, gunshotIndex);
            }
        }
    }

    private void LaunchProjectile(Vector2 direction, int gunshotIndex)
    {
        
        // Calculate the angle of the direction vector
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create a quaternion for the rotation, subtracting 90 degrees to align the sprite with the direction
        Quaternion projectileRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Create the projectile
        GameObject projectile = Instantiate(projectiles[gunshotIndex], transform.position, projectileRotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Add a tiny random offset to the direction
        
        Vector2 offset = new Vector2(Random.Range(straightProjectileOffset*-1f, straightProjectileOffset), Random.Range(straightProjectileOffset*-1f, straightProjectileOffset));
        
        rb.velocity = (direction + offset).normalized * projectileSpeed;

        // Set other properties on the projectile, such as damage, explosion behavior, etc.
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.SetProperties(explodesOnImpact, splitsIntoMoreBullets, numberOfBounces);
        audioSource.clip = Sounds[gunshotIndex];
        audioSource.PlayOneShot(Sounds[gunshotIndex]);
    }


    private void changeGunImage(int index)
    {
        if (gunSprites.Length > 0)
        {
            gunImage.sprite = gunSprites[index];
        }
        else
        {
            Debug.LogWarning("No gun sprites assigned to the array.");
        }
    }

    public void changeGunType()
    {
        int gunType = Random.Range(0, 3);

        shotgun = gunType == 0;
        moveStraight = gunType == 1;
        moveInSineWave = gunType == 2;

        if(moveInSineWave){
            
            fireRate = 10;
        }
        else if(moveStraight){
            
            fireRate = 7;
        }
        else{
            
            fireRate = 5;
        }

        changeGunImage(gunType);
    }

    public void upgradeGunType()
    {
        // Set random values for each variable
        explodesOnImpact = Random.Range(0, 2) == 1; // Random true or false
        splitsIntoMoreBullets = Random.Range(0, 2) == 1; // Random true or false
        if(!explodesOnImpact){
            numberOfBounces = Random.Range(1, 6); // Random integer between 1 and 5
        }
        
    }  



}
