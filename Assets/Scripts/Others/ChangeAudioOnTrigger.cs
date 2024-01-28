using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ChangeAudioOnTrigger : MonoBehaviour
{
    public AudioClip nuevoAudioClip;
    public AudioSource audioSource;
    private AudioClip audioOriginal;

    private void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("El GameObject debe tener un AudioSource adjunto.");
            return;
        }

        // Guarda el AudioClip original
        audioOriginal = audioSource.clip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Cambia "Player" por la etiqueta del objeto que activará el cambio
        {
            // Cambia el AudioClip al nuevo cuando el objeto entra en el trigger
            if (nuevoAudioClip != null)
            {
                audioSource.clip = nuevoAudioClip;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning("El nuevo AudioClip no está asignado.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Cambia "Player" por la etiqueta del objeto que activará el cambio
        {
            // Restaura el AudioClip original cuando el objeto sale del trigger
            audioSource.clip = audioOriginal;
            audioSource.Play();
        }
    }
}
