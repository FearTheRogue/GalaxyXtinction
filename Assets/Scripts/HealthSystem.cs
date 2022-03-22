using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private bool canDie = true;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

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

            Dead();
        }
    }

    public void Dead()
    {
        Debug.Log("Entity is dead");
        Destroy(gameObject);
    }
}