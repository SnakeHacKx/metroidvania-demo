using TMPro;
using UnityEngine;

/// <summary>
/// <para>Mánager de los Diálogos.</para>
/// Contiene todos los métodos referentes a los diálogos del juego.
/// <list type="bullet">
/// <item>
/// <term>ShowDialogue</term>
/// <description>Muestra las líneas de diálogo en pantalla.</description>
/// </item>
/// <item>
/// <term>Update</term>
/// <description>Actúa como receptor en la pantalla de diálogos.</description>
/// </item>
/// </list>
/// </summary>
[RequireComponent(typeof(GameObject))]
[RequireComponent(typeof(TMP_Text))]
[RequireComponent(typeof(HeroController))]
public class DialogueManager : MonoBehaviour
{
    [Tooltip("Inserte la caja de la UI que contendrá los diálogos")]
    public GameObject dialogueBox;

    [Tooltip("...")]
    public TMP_Text dialogueText;

    [Tooltip("El diálogo está activo?")]
    [SerializeField] private bool dialogueActive;

    [Tooltip("Las diferentes líneas de diálogo que puede tener NPC, objeto, etc...")]
    [SerializeField] private string[] dialogueLines;

    [Tooltip("Muestra la línea de diálogo actual")]
    [SerializeField] private int currentDialogueLine;

    // Referencia a la clase HeroController
    private HeroController playerController;
    private QuestManager questManager;

    private void Start()
    {
        dialogueActive = false;
        dialogueBox.SetActive(false);

        playerController = FindObjectOfType<HeroController>();
        questManager = FindObjectOfType<QuestManager>();
    }

    /// <summary>
    /// Actúa como receptor en la pantalla de diálogos.
    /// </summary>
    /// <remarks>
    /// Si presionamos la letra X del teclado o el botón A de un mando de XBOX,
    /// se pasará a la siguiente línea de diálogo, si ya no quedan más, desactiva la caja
    /// y los diálogos.
    /// </remarks>
    private void Update()
    {
        if (dialogueActive && (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Joystick1Button0)))
        {
            currentDialogueLine++;

            if (currentDialogueLine >= dialogueLines.Length)
            {
                dialogueActive = false;
                dialogueBox.SetActive(false);
                FindObjectOfType<UIManager>().hud.SetActive(true);
                currentDialogueLine = 0;
                playerController.isTalking = false;
            }
            else
            {
                dialogueText.text = dialogueLines[currentDialogueLine];
            }
        }
    }

    /// <summary>
    /// Muestra las líneas de diálogo en pantalla.
    /// </summary>
    /// <remarks>
    /// Inicia desde la primera línea, activa los diálogos y su caja.
    /// </remarks>
    /// <param name="lines">Líneas de diálogo recibidas desde el inspector de Unity.</param>
    public void ShowDialogue(string[] lines)
    {
        currentDialogueLine = 0;
        dialogueLines = lines;
        dialogueActive = true;
        FindObjectOfType<UIManager>().hud.SetActive(false);
        HeroController.SharedInstance.SetIdleAnimToPlayer();
        dialogueBox.SetActive(true);
        dialogueText.text = dialogueLines[currentDialogueLine];
        playerController.isTalking = true;
    }
}