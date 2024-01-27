using UnityEngine;

public class ChangePlayer : MonoBehaviour
{
    public GameObject player1;
    public GameObject camera1;
    public GameObject player2;
    public GameObject camera2;
    public float cooldownTime = 2f; // Tiempo de cooldown en segundos

    private bool canSwitch = true;
    private float lastSwitchTime;

    void Start()
    {
        // Ensure that the GameObjects are assigned correctly
        if (player1 == null || player2 == null || camera1 == null || camera2 == null)
        {
            Debug.LogError("Make sure to assign the players in the Inspector.");
        }


        // Initially activate Player1 and deactivate Player2
        player1.SetActive(true);
        camera1.SetActive(true);
        player2.SetActive(false);
        camera2.SetActive(false);
        // Initialize the last switch time
        lastSwitchTime = -cooldownTime;
    }

    void Update()
    {
        // Switch characters when a key is pressed and cooldown has expired
        if (canSwitch && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            SwitchCharacters();
        }
    }

    void SwitchCharacters()
    {
        // Check if enough time has passed since the last switch
        if (Time.time - lastSwitchTime > cooldownTime)
        {
            // Update the last switch time
            lastSwitchTime = Time.time;

            // Store the current position of the player being deactivated
            Vector3 currentPlayerPosition = player1.activeSelf ? player1.transform.position : player2.transform.position;

            // Deactivate the currently active player and activate the inactive one
            player1.SetActive(!player1.activeSelf);
            player2.SetActive(!player2.activeSelf);

            // Set the position of the newly activated player to the position of the previously deactivated player
            if (player1.activeSelf)
            {
                player1.transform.position = currentPlayerPosition;
            }
            else
            {
                player2.transform.position = currentPlayerPosition;
            }
        }
    }
}
