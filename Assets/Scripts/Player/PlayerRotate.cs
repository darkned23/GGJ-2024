using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public PlayerController playerController;
    public float raycastDistance = 2.0f;

    public float speedPush;
    private float initialJumpForce;
    private float initialSpeed;
    private void Start()
    {
        initialSpeed = playerController.speed;
        initialJumpForce = playerController.jumpForce;
    }
    void Update()
    {
        // Rotar en el eje Y según la dirección horizontal
        RotatePlayer(playerController.horizontalMovement * playerController.canMove);

        // Obtener la posición y la dirección de la cámara u otro objeto desde el que se lanza el raycast
        Vector3 origin = transform.position;
        Vector3 forward = transform.forward;
        // Raycast para detectar objetos en la dirección forward
        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward, out hit, raycastDistance))
        {
            // Verificar si el objeto tiene el tag "Movable"
            if (hit.collider.CompareTag("Movable"))
            {
                Debug.DrawRay(origin, forward * raycastDistance, Color.green);

                playerController.animator.SetBool("Arrastrando", true);

                playerController.speed = speedPush;
                playerController.animator.ResetTrigger("Saltar");

                playerController.jumpForce = 0f;
            }
            else
            {
                playerController.animator.SetBool("Arrastrando", false);

                playerController.speed = initialSpeed;
                playerController.jumpForce = initialJumpForce;

                Debug.DrawRay(origin, forward * raycastDistance, Color.red);
            }
        }
        else
        {
            playerController.animator.SetBool("Arrastrando", false);

            playerController.speed = initialSpeed;
            playerController.jumpForce = initialJumpForce;

            Debug.DrawRay(origin, forward * raycastDistance, Color.gray);
        }
    }
    void RotatePlayer(float horizontalMovement)
    {
        if (horizontalMovement != 0f)
        {
            float targetAngle = Mathf.Atan2(0f, horizontalMovement) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle + 90, 0f);
        }
    }
}
