using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerRespawn playerRespawn;
    public float moveCooldown = 5.0f;
    public int maxHealth = 100;
    public float invulnerabilityTime = 1.5f;
    public float knockbackForce = 5f;

    private int currentHealth;
    private bool isInvulnerable = false;

    void Start()
    {
        currentHealth = maxHealth;
        playerRespawn = GetComponent<PlayerRespawn>();
        playerController = GetComponent<PlayerController>();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Verificar si la colisión es con un enemigo
        if (hit.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20); // Puedes ajustar la cantidad de daño según tus necesidades
        }
        else if (hit.gameObject.CompareTag("Danger"))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            // Aplicar daño al jugador
            currentHealth -= damage;
            playerController.animator.SetTrigger("RecibirGolpe");
            playerController.animator.ResetTrigger("Saltar");

            StartCoroutine(StopMove());
            StartCoroutine(playerRespawn.Respawn());
            // Activar la invulnerabilidad y el retroceso
            StartCoroutine(InvulnerabilityTime());
            Knockback();

            // Puedes agregar aquí lógica adicional, como reproducir efectos de sonido o mostrar efectos visuales de daño

            // Verificar si el jugador está vivo después de recibir daño
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }
    IEnumerator StopMove()
    {
        playerController.enabled = false;

        yield return new WaitForSeconds(moveCooldown);

        playerController.enabled = true;
    }

    void Die()
    {
        // Puedes agregar aquí lógica de muerte, como reiniciar el nivel o mostrar una pantalla de Game Over
        Debug.Log("Player has died.");
    }

    void Knockback()
    {
        // Aplicar un retroceso hacia atrás (usando la dirección actual del jugador)
        playerController.Jump(knockbackForce);
    }

    IEnumerator InvulnerabilityTime()
    {
        // Activar la invulnerabilidad
        isInvulnerable = true;


        // Esperar el tiempo de invulnerabilidad
        yield return new WaitForSeconds(invulnerabilityTime);

        // Desactivar la invulnerabilidad después del tiempo especificado
        isInvulnerable = false;
    }
}
