using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint;
    public float waitRespawn = 1.5f;

    void Start()
    {
        // Establecer el punto de respawn inicial
        respawnPoint = transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el jugador ha pasado por cierto punto
        if (other.CompareTag("RespawnPoint"))
        {
            respawnPoint = other.transform.position;
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(waitRespawn);
        // Hacer respawn al último punto guardado
        transform.position = respawnPoint;
    }
}
