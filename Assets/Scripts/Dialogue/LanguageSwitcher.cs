using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageSwitcher : MonoBehaviour
{
    public string[] targetLanguageCodes; // Arreglo de códigos de idioma que se recorrerá
    private int currentLanguageIndex = 0;

    void Start()
    {
        // Verifica el idioma actual al inicio y ajusta currentLanguageIndex en consecuencia
        for (int i = 0; i < targetLanguageCodes.Length; i++)
        {
            if (LocalizationSettings.SelectedLocale.Identifier.Code == targetLanguageCodes[i])
            {
                currentLanguageIndex = i;
                break;
            }
        }
    }

    public void SwitchLanguage()
    {
        // Cambia al siguiente código de idioma en el arreglo
        currentLanguageIndex = (currentLanguageIndex + 1) % targetLanguageCodes.Length;

        // Cambia el idioma al especificado en targetLanguageCodes[currentLanguageIndex]
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(targetLanguageCodes[currentLanguageIndex]);
    }
}
