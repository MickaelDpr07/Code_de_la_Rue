using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Deplacement : MonoBehaviour
{
    private bool CanDrag = true;
    private bool isDragging = false;

    public bool completed = false;
    private TMP_Text txt_Contexte; // Texte à afficher


    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private RectTransform rectTransform;
    private Canvas canvas;

    public GameObject[] EmplacementDisponilbes; // Emplacements disponibles
    public GameObject EmplacementBon; // Emplacement correct
    public GameObject PopUp;
    public GameObject Contexte;

    [SerializeField]
    public string texte1,texte2; //Texte de contexte

    public GameManager GameManager;

    void Start()
    {
        OpenPopUp();
        // Récupération du RectTransform de l'image
        rectTransform = GetComponent<RectTransform>();

        // Récupération du canvas
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Ce GameObject doit être enfant d'un Canvas.");
        }
    }

    void Update()
    {
        DragAndDrop();
    }

    public void DragAndDrop()
    {
        // Vérifie s'il y a un ou plusieurs touchés sur l'écran
        if (Input.touchCount > 0 && CanDrag)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Début du touch, vérifie si l'utilisateur touche l'objet
                    if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, touch.position, canvas.worldCamera))
                    {
                        isDragging = true;
                        // Conversion des coordonnées de l'écran en coordonnées locales
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, touch.position, canvas.worldCamera, out originalLocalPointerPosition);
                        originalPanelLocalPosition = rectTransform.localPosition;

                        Debug.Log("Drag started");
                    }
                    break;

                case TouchPhase.Moved:
                    // Si l'objet est en cours de drag, déplacez-la avec le doigt
                    if (isDragging)
                    {
                        Vector2 localPointerPosition;
                        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, touch.position, canvas.worldCamera, out localPointerPosition))
                        {
                            Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                            rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;
                        }

                        // Affichage des emplacements
                        foreach (GameObject emplacement in EmplacementDisponilbes)
                        {
                            emplacement.SetActive(true);
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    // Fin du drag
                    if (isDragging)
                    {
                        isDragging = false;
                        Debug.Log("Drag ended");
                    }
                    break;
            }
        }
    }

    // Méthode pour détecter les collisions avec les emplacements
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision detected with: " + other.gameObject.name); // Pour vérifier si la collision est détectée

        if (isDragging)
        {
            foreach (GameObject emplacement in EmplacementDisponilbes)
            {
                if (other.gameObject == emplacement)
                {
                    if (emplacement == EmplacementBon)
                    {
                        Debug.Log("Correct placement!");
                        foreach (GameObject place in EmplacementDisponilbes)
                        {
                            place.SetActive(false); // Cache les emplacements après le drag
                        }
                        GameManager.NbrPuzzleRéussi++;
                        CanDrag = false;
                        Contexte.SetActive(false);
                        completed = true;
                        OpenPopUp();
                    }
                    else
                    {
                        Debug.Log("Incorrect placement");
                        OpenPopUp();
                    }
                }
            }
        }
    }

    public void ClosePopUp()
    {
        PopUp.SetActive(false);
    }

    public void OpenPopUp()
    {
        Debug.Log("oui");
        if (PopUp != null)
        {
            // Affiche ou cache le popup en fonction de son état actuel
            bool isActive = PopUp.activeSelf; // Vérifie si le popup est actuellement actif
            PopUp.SetActive(!isActive); // Inverse l'état

            if (PopUp.activeSelf) // Si le popup est maintenant actif
            {
                // Cherche l'enfant avec le nom "Txt_contexte"
                Transform txtContexteTransform = PopUp.transform.Find("Txt_contexte");

                // Vérifie si l'élément existe et applique le texte
                if (txtContexteTransform != null)
                {
                    // Récupérer le composant TMP_Text et appliquer le texte
                    TMP_Text txtContexte = txtContexteTransform.GetComponent<TMP_Text>();
                    txt_Contexte = txtContexte;

                    if (txtContexte != null)
                    {
                        if (!completed)
                        {
                            txtContexte.text = texte1; // Assigner texte1 au TextMeshPro
                        }
                        else
                        {
                            txtContexte.text = texte2; // Assigner texte1 au TextMeshPro
                        }

                    }
                    else
                    {
                        Debug.LogError("Le composant TMP_Text n'a pas été trouvé sur 'Txt_contexte'.");
                    }
                }
                else
                {
                    Debug.LogError("L'élément 'Txt_contexte' n'a pas été trouvé dans le prefab.");
                }
            }
        }
        else
        {
            Debug.LogError("Le prefab n'est pas assigné.");
        }
    }
}