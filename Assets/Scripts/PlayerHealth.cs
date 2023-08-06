using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 5; // Set your initial health value
    public Image[] hearts; // Array of UI Images to display hearts
    public Sprite fullHeart; // Full heart sprite
    public Sprite halfHeart; // Half heart sprite
    public Sprite emptyHeart; // Empty heart sprite
    public GameObject gotHitVFX ;

    public AudioClip gotHitSound; // Sound clip for barrel explosion
    public AudioClip gotHealthSound; // Sound clip for enemy explosion
    private AudioSource audioSource; // Reference to the AudioSource component

    void Start(){
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        UpdateHearts();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            health -= 1;
            audioSource.clip = gotHitSound;
            audioSource.PlayOneShot(gotHitSound);
            Instantiate(gotHitVFX, transform.position, Quaternion.identity);
            CameraShakeManager.instance.Shake(0.5f);
            
        }

        if (collision.gameObject.tag == "Health")
        {
            audioSource.clip = gotHitSound;
            audioSource.PlayOneShot(gotHealthSound);
            health = Mathf.Min(health + 2, hearts.Length * 2);
            UpdateHearts();
            
        }
    }

    public void Die()
    {
        health = health - 2;
        UpdateHearts();
        
    }

    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (health >= ((i+1) * 2))
            {
                hearts[i].sprite = fullHeart;
            }
            else if (health == ((i * 2) + 1))
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
        if (health <= 0){
            takeMeToMainMenu();
        }
    }

    void takeMeToMainMenu(){
        SceneChanger sc = new SceneChanger();
        sc.ChangeScene("Main Menu");
    }
}
