using UnityEngine;

/// <summary>
/// Contiene todos los métodos referentes a los diálogos de los NPC's.
/// <list type="bullet">
/// <item>
/// <term>OnTriggerEnter2D</term>
/// <description>Analiza si el jugador entro en la zona de diálogo.</description>
/// </item>
/// <item>
/// <term>OnTriggerEnter2D</term>
/// <description>Analiza si el jugador salió de la zona de diálogo.</description>
/// </item>
/// </list>
/// </summary>
[RequireComponent(typeof(CircleCollider2D))]
public class NPCDialogue : MonoBehaviour
{
    [Tooltip("Nombre que desea que posea el NPC")]
    [SerializeField] private string npcName;

    [Tooltip("Son las diferentes líneas de diálogo del NPC")]
    [SerializeField] private string[] npcDialogueLines;

    // Referencia al manager de los diálogos
    private DialogueManager dialogueManager;

    // Variable booleana que indica si el jugador está o no dentro de la zona de diálogo
    // de un NPC
    private bool playerInTheZone;

    private void Start()
    {
        // Recordar que: FindObjectOfType sólo se usa cuando solamente hay un game object
        // en toda la escena que tenga el script llamado entre diamantes < >
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    /// <summary>
    /// Analiza si el jugador entro en la zona de diálogo.
    /// </summary>
    /// <param name="collision">Guarda la colisión</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInTheZone = true;
            InteractWarning.SharedInstance.ShowInteractWarning();
        }
    }

    /// <summary>
    /// Analiza si el jugador salió de la zona de diálogo.
    /// </summary>
    /// <param name="collision">Guarda la colisión</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInTheZone = false;
            InteractWarning.SharedInstance.HideInteractWarning();
        }
    }

    /// <summary>
    /// Envía las líneas de diálogo al método <c>ShowDialogue()</c> de
    /// la clase mánager de los diálogos
    /// </summary>
    private void Update()
    {
        if (playerInTheZone && (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.Joystick1Button3)))
        {
            string[] finalDialogue = new string[npcDialogueLines.Length];

            int i = 0;
            // Para cada línea de diálogo en npcDialogueLines
            foreach (string line in npcDialogueLines)
            {
                finalDialogue[i++] = ((npcName != null) ? npcName + "\n" : "") + line;
            }

            dialogueManager.ShowDialogue(finalDialogue);
        }
    }
}