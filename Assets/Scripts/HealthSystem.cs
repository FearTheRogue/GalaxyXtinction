using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public bool canDie = true;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [SerializeField] private GameObject explosionFX;

    [SerializeField] private float timeToWait;

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

    public void TakeDamage(int damage)
    {
        if (!canDie)
            return;

        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }

    public void ApplyHealth(int health)
    {
        currentHealth += health;

        healthBar.SetHealth(currentHealth);
    }

    private void Update()
    {
        if (!canDie)
            return;

        if(currentHealth <= 0)
        {
            if (dropLoot != null)
                dropLoot.DropLoot();

            if(this.transform.gameObject.name == "Player Ship")
            {
                PlayerDead();
                return;
            }

            if (explosionFX != null)
            {
                AudioManager.instance.Play("Explosion");
                Instantiate(explosionFX, this.transform.position, this.transform.rotation);
            }

            Dead();
        }
    }

    public void PlayerDead()
    {
        if (explosionFX != null)
        {
            AudioManager.instance.Play("Explosion");
            Instantiate(explosionFX, this.transform.position, this.transform.rotation);
        }

        if (AudioManager.instance.IsClipPlaying("Thruster Boost"))
            AudioManager.instance.Stop("Thruster Boost");

        Camera cam;
        cam = Camera.main;

        cam.transform.parent = null;

        InGameMenu.instance.PlayerIsDead = true;

        Destroy(gameObject);
    }

    public void Dead()
    {
        Debug.Log("Entity is dead");

        Destroy(gameObject);
    }
}