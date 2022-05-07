using System.Collections;
using UnityEngine;

/// <summary>
/// 
/// Handles the health system, used by player and enemies.
/// 
/// </summary>

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public bool canDie = true;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [SerializeField] private GameObject explosionFX;

    [SerializeField] private float timeToWait;

    // Returns the current health value
    public int GetCurrentHealth() { return currentHealth; }

    [SerializeField] private HealthBar healthBar;
    private DropLookSystem dropLoot;

    private void Awake()
    {
        dropLoot = GetComponent<DropLookSystem>();
        
        if (dropLoot == null)
            return;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Decreases health by the damage value
    public void TakeDamage(int damage)
    {
        // For Testing Purposes
        if (!canDie)
            return;

        currentHealth -= damage;

        // Updates Health bar UI
        healthBar.SetHealth(currentHealth);
    }

    // Increases health by the health value
    public void ApplyHealth(int health)
    {
        currentHealth += health;

        // Updates the heath bar UI
        healthBar.SetHealth(currentHealth);
    }

    private void Update()
    {
        if (!canDie)
            return;

        // Checks if health is 0
        if(currentHealth <= 0)
        {
            // if gameobject has the 'DropLootSystem' script
            if (dropLoot != null)
                dropLoot.DropLoot();

            // If player dies
            if(this.transform.gameObject.name == "Player Ship")
            {
                PlayerDead();
                return;
            }

            // Instantiate explosion
            if (explosionFX != null)
            {
                AudioManager.instance.Play("Explosion");
                Instantiate(explosionFX, this.transform.position, this.transform.rotation);
            }

            Dead();
        }
    }

    // Specific method to handle the player death event
    public void PlayerDead()
    {
        // Instantiates explosion
        if (explosionFX != null)
        {
            AudioManager.instance.Play("Explosion");
            Instantiate(explosionFX, this.transform.position, this.transform.rotation);
        }

        // Stop audio sound
        if (AudioManager.instance.IsClipPlaying("Thruster Boost"))
            AudioManager.instance.Stop("Thruster Boost");

        Camera cam;
        cam = Camera.main;

        // Isolate main camera
        cam.transform.parent = null;

        InGameMenu.instance.PlayerIsDead = true;

        Destroy(gameObject);
    }

    // Destroys game object
    public void Dead()
    {
        Debug.Log("Entity is dead");

        Destroy(gameObject);
    }
}