using FishNet.Object;
using UnityEngine;

public class Damager : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!base.IsOwner) return;
        if (!other.CompareTag("Player")) return;

        AgentHealth agentHealth = other.GetComponent<AgentHealth>();
        if (agentHealth != null)
        {
            ServerDamage(agentHealth);
        }
    }

    [ServerRpc]
    private void ServerDamage(AgentHealth opponentsHealth)
    {
        opponentsHealth.UpdateHealth(opponentsHealth.CurrentHealth - 10);
    }
    
}
