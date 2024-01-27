using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public PlayerController playerController;
    void Update()
    {
        // Rotar en el eje Y según la dirección horizontal
        RotatePlayer(playerController.horizontalMovement);
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
