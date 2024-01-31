using TMPro;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Localization.Settings;

public class Dialogue : MonoBehaviour
{
    [Header("Config Dialogue")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public GameObject dialogueMark;

    [Header("Config Table")]
    public string tableName;
    public String[] dialogueKeys;
    public float typingTime = 0.05f;

    [Header("Other config")]
    [SerializeField] private bool canBeGrabbed;

    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;
    private string currentLine;
    private void Update()
    {
        if (isPlayerInRange && (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton3)))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if (dialogueText.text == currentLine)
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = currentLine;
            }
        }
    }
    public void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueMark.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0f;

        // Obtén la traducción desde la Localization Table
        LocalizationSettings.StringDatabase.DefaultTable = tableName;
        dialogueText.text = LocalizationSettings.StringDatabase.GetLocalizedString(dialogueKeys[lineIndex]);

        StartCoroutine("ShowLine");
    }
    public void NextDialogueLine()
    {
        lineIndex++;

        if (lineIndex < dialogueKeys.Length)
        {
            StartCoroutine("ShowLine");
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            dialogueMark.SetActive(true);
            Time.timeScale = 1f;
            if (canBeGrabbed)
            {
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator ShowLine()
    {
        // Utiliza la clave para obtener la traducción
        currentLine = LocalizationSettings.StringDatabase.GetLocalizedString(dialogueKeys[lineIndex]);

        Debug.Log("Current Line Content: " + currentLine);
        dialogueText.text = string.Empty;
        foreach (char ch in currentLine)
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueMark.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueMark.SetActive(false);

        }
    }
}