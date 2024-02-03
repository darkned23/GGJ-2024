using System.Collections;
using UnityEngine;
using Unity.Collections;

public class ChangePlayer : MonoBehaviour
{
    public GameObject player1;
    public GameObject camera1;
    public GameObject player2;
    public GameObject camera2;
    public float cooldownTime = 2f; // Tiempo de cooldown en segundos
    public PlayerController playerControllerCurrent; // agregar forma de saber cual es el player activo

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
        if (canSwitch && (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.JoystickButton1)))
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

            // Deactivate the currently active player and activate the inactive one
            camera1.SetActive(!camera1.activeSelf);
            camera2.SetActive(!camera2.activeSelf);

            StartCoroutine("CoroutineWaitMoveCamera");
        }
    }

    IEnumerator CoroutineWaitMoveCamera()
    {
        yield return new WaitForSeconds(1f);

        player1.SetActive(!player1.activeSelf);
        player2.SetActive(!player2.activeSelf);
    }
}
