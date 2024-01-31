using UnityEngine;
using System.Collections;

public class VersionAnterior : MonoBehaviour
{
    private CharacterController characterController;
    private PlayerRespawn playerRespawn;

    [Header("Movement")]
    public float speed = 5.0f;
    public float jumpForce = 10f;
    public float jumpCooldown = 0.5f;
    public float gravity = 9.8f;
    public float distanceRayCastGround = 2.0f;
    private float verticalVelocity;

    [Header("Animations")]
    public Animator animator;

    [Header("Attack")]
    public Collider attackCollider;
    public float attackCooldown = 1f;
    public float attackDuration = 0.5f;
    public bool allowAttack;

    [Header("Health")]

    public float moveCooldown = 5.0f;
    public int maxHealth = 100;
    public float invulnerabilityTime = 1.5f;
    public float knockbackForce = 5f;

    private int currentHealth;
    private bool isInvulnerable = false;
    //----------------------------------------------------------------------------------//
    private bool isGrounded;
    public bool canAttack = true;
    [HideInInspector] public bool canJump = true;
    [HideInInspector] public float horizontalMovement;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerRespawn = GetComponent<PlayerRespawn>();

        currentHealth = maxHealth;

        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    void Update()
    {
        // Mover horizontalmente
        horizontalMovement = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalMovement, 0f, 0f);
        characterController.Move(movement * speed * Time.deltaTime);
        animator.SetFloat("Velocidad", Mathf.Abs(horizontalMovement));

        // Saltar
        if (isGrounded && Input.GetButtonDown("Jump") && canJump)
        {
            canJump = false;
            Jump(jumpForce);
        }

        if (Input.GetMouseButtonDown(0) && canAttack && allowAttack)
        {
            canAttack = false;
            Attack();
        }
        // Raycast para la detección de suelo
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceRayCastGround))
        {
            isGrounded = true;

            // Visualizar el raycast en el Inspector
            Debug.DrawRay(transform.position, Vector3.down * distanceRayCastGround, Color.green);
        }
        else
        {
            isGrounded = false;

            // Visualizar el raycast en el Inspector
            Debug.DrawRay(transform.position, Vector3.down * distanceRayCastGround, Color.red);
        }

        // Aplicar gravedad
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Aplicar movimiento vertical
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Verificar si la colisión es con un enemigo
        if (hit.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(Knockback());
        }
        else if (hit.gameObject.CompareTag("Danger"))
        {
            StartCoroutine(Knockback());

            StartCoroutine(playerRespawn.Respawn());
        }
    }

    void Attack()
    {
        // Activa el Collider de ataque
        if (attackCollider != null)
        {
            attackCollider.enabled = true;
        }

        // Espera la duración del ataque
        StartCoroutine(DisableColliderAfterDuration());

        // Inicia el cooldown
        StartCoroutine(CooldownAttack());
    }

    public void Jump(float jumpHeigth)
    {
        verticalVelocity = jumpHeigth;

        animator.SetTrigger("Saltar");

        StartCoroutine(CooldownJump());
    }

    IEnumerator DisableColliderAfterDuration()
    {
        // Espera la duración del ataque
        yield return new WaitForSeconds(attackDuration);

        // Desactiva el Collider de ataque después de la duración
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    public IEnumerator CooldownJump()
    {
        // Espera el tiempo de cooldown
        yield return new WaitForSeconds(jumpCooldown);

        canJump = true;
    }
    public IEnumerator CooldownAttack()
    {
        // Espera el tiempo de cooldown
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    IEnumerator Knockback()
    {
        float currentSpeed = speed;
        speed = 0;
        TakeDamage(10);

        yield return new WaitForSeconds(playerRespawn.waitRespawn);

        speed = currentSpeed;
        animator.ResetTrigger("Saltar");
    }

    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        // Aplicar daño al jugador
        {
            currentHealth -= damage;
            // animator.SetTrigger("RecibirGolpe");

            // Activar la invulnerabilidad y el retroceso
            StartCoroutine(InvulnerabilityTime());

            // Verificar si el jugador está vivo después de recibir daño
            if (currentHealth <= 0)
            {
                Die();
            }
        }
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
    void Die()
    {
        // Puedes agregar aquí lógica de muerte, como reiniciar el nivel o mostrar una pantalla de Game Over
        Debug.Log("Player has died.");
    }
}