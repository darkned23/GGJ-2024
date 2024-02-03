using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform teleportTarget;
    public GameObject indicationObject;
    public KeyCode teleportKey = KeyCode.F;

    private bool isInTrigger = false;
    private GameObject currentPlayer;

    private void Update()
    {
        if (Input.GetKeyDown(teleportKey) && isInTrigger && currentPlayer != null)
        {
            TeleportPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = true;
            indicationObject.SetActive(true);
            currentPlayer = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTrigger = false;
            indicationObject.SetActive(false);
            currentPlayer = null;
        }
    }

    private void TeleportPlayer()
    {
        if (teleportTarget != null && currentPlayer != null)
        {
            CharacterController characterController = currentPlayer.GetComponent<CharacterController>();

            if (characterController != null)
            {
                characterController.enabled = false; // Desactiva temporalmente el CharacterController para cambiar la posición
                characterController.transform.position = teleportTarget.position;
                characterController.enabled = true; // Vuelve a activar el CharacterController
                indicationObject.SetActive(false);
                currentPlayer = null;
            }
            else
            {
                Debug.LogError("El objeto del jugador no tiene un CharacterController adjunto.");
            }
        }
        else
        {
            Debug.LogError("El punto de destino no está asignado o el jugador actual no está definido en el script de teleportación.");
        }
    }
}
