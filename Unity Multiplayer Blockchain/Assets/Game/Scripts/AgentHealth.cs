using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;

public class AgentHealth : NetworkBehaviour, ITakeGame
{
    [SerializeField] private Image healthBar;
    private const int maxHealth = 100;
    public int CurrentHealth { get; set; }
   

    private void Start()
    {
        CurrentHealth = maxHealth;
    }

    /*[ContextMenu("Take Damage")]
    public void TestTakeDamage()
    {
        UpdateHealth(10);
    }*/

    public void UpdateHealth(int health)
    {
        CurrentHealth = health;
        healthBar.fillAmount = (float)CurrentHealth / maxHealth;
        ObserversUpdateHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
       gameObject.SetActive(false);
    }

    [ObserversRpc(BufferLast = true)]
    private void ObserversUpdateHealth(int health)
    {
        CurrentHealth = health;
        healthBar.fillAmount = (float)CurrentHealth / maxHealth;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
}
