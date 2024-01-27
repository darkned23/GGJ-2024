using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float distanceRayCast = 2.0f;
    private bool isGrounded;
    private CharacterController characterController;

    // La velocidad vertical del personaje
    private float verticalVelocity;

    //Attack variables
    public Collider attackCollider;
    public float attackCooldown = 1f;
    public float attackDuration = 0.5f;

    private bool canAttack = true;
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

        // Saltar
        if (isGrounded && Input.GetButtonDown("Jump"))
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
        if (Input.GetMouseButtonDown(0) && canAttack)
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
        StartCoroutine(StartCooldown());
    }

    public void Jump(float jumpHeigth)
    {
        verticalVelocity = jumpHeigth;
    }

    System.Collections.IEnumerator DisableColliderAfterDuration()
    {
        // Espera la duración del ataque
        yield return new WaitForSeconds(attackDuration);

        // Desactiva el Collider de ataque después de la duración
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }

    System.Collections.IEnumerator StartCooldown()
    {
        // Establece la bandera de ataque a falso
        canAttack = false;

        // Espera el tiempo de cooldown
        yield return new WaitForSeconds(attackCooldown);

        // Restablece la bandera de ataque a verdadero después del cooldown
        canAttack = true;
    }
}
