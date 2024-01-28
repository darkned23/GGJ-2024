using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 10f;
    public float jumpCooldown = 0.5f;
    public float gravity = 9.8f;
    public float distanceRayCast = 2.0f;
    public Animator animator;
    private bool isGrounded;
    private CharacterController characterController;

    // La velocidad vertical del personaje
    private float verticalVelocity;

    //Attack variables
    public Collider attackCollider;
    public float attackCooldown = 1f;
    public float attackDuration = 0.5f;
    public bool allowAttack;

    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public bool canJump = true;
    [HideInInspector] public float horizontalMovement;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
            Jump(jumpForce);
            isGrounded = false;
        }

        // Raycast para la detección de suelo
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, distanceRayCast))
        {
            isGrounded = true;

            // Visualizar el raycast en el Inspector
            Debug.DrawRay(transform.position, Vector3.down * distanceRayCast, Color.green);
        }
        else
        {
            isGrounded = false;

            // Visualizar el raycast en el Inspector
            Debug.DrawRay(transform.position, Vector3.down * distanceRayCast, Color.red);
        }

        // Aplicar gravedad
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Si el clic izquierdo está presionado y el ataque está disponible, realiza el ataque
        if (Input.GetMouseButtonDown(0) && canAttack && allowAttack)
        {
            Attack();
        }

        // Aplicar movimiento vertical
        characterController.Move(Vector3.up * verticalVelocity * Time.deltaTime);
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
        StartCoroutine(Cooldown(canAttack, attackCooldown));
    }

    public void Jump(float jumpHeigth)
    {
        verticalVelocity = jumpHeigth;

        animator.SetTrigger("Saltar");

        StartCoroutine(Cooldown(canJump, jumpCooldown));
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

    public IEnumerator Cooldown(bool canDo, float timeCooldown)
    {
        canDo = false;

        // Espera el tiempo de cooldown
        yield return new WaitForSeconds(timeCooldown);

        canDo = true;
    }
}
