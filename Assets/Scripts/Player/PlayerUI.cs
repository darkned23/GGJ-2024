using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerController playerController;  // Asigna el componente PlayerController desde el inspector
    public Image[] heartSprites;  // Arreglo de sprites de corazones

    void Start()
    {
        // Asegúrate de que el tamaño del arreglo coincida con la cantidad máxima de vida
        if (heartSprites.Length != playerController.maxHealth)
        {
            Debug.LogError("El tamaño del arreglo de sprites de corazones no coincide con la cantidad máxima de vida.");
            return;
        }

        // Inicializa la representación visual de la vida
        UpdateHeartSprites();
    }

    void Update()
    {
        // Actualiza la representación visual de la vida si ha habido cambios
        if (playerController.currentHealth != GetActiveHearts())
        {
            UpdateHeartSprites();
        }
    }

    void UpdateHeartSprites()
    {
        int activeHearts = GetActiveHearts();

        // Activa o desactiva los sprites de corazones según la vida actual
        for (int i = 0; i < heartSprites.Length; i++)
        {
            heartSprites[i].enabled = i < activeHearts;
        }
    }

    int GetActiveHearts()
    {
        // Calcula la cantidad de sprites de corazones activos según la vida actual
        return Mathf.Clamp(playerController.currentHealth, 0, heartSprites.Length);
    }
}
