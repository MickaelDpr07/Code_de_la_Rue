using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deplacement : MonoBehaviour
{
    private bool isDragging = false;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private RectTransform rectTransform;
    private Canvas canvas;

    public GameObject[] EmplacementDisponilbes; // Emplacements disponibles
    public GameObject EmplacementBon; // Emplacement correct

    void Start()
    {
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
        if (Input.touchCount > 0)
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

                        // Cache les emplacements
                        foreach (GameObject emplacement in EmplacementDisponilbes)
                        {
                            emplacement.SetActive(false); // Cache les emplacements après le drag
                        }
                    }
                    break;
            }
        }
    }

    // Méthode pour détecter les collisions avec les emplacements
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDragging)
        {
            // Parcourir tous les emplacements disponibles
            foreach (GameObject emplacement in EmplacementDisponilbes)
            {
                // Vérifiez si l'objet de collision est l'un des emplacements
                if (other.gameObject == emplacement)
                {
                    // Vérifiez si l'emplacement est le bon
                    if (emplacement == EmplacementBon)
                    {
                        Debug.Log("Correct placement!");
                        // Désactive l'élément glissé
                        gameObject.SetActive(false);
                        // Vous pouvez également gérer la logique de réussite ici
                    }
                    else
                    {
                        Debug.Log("Incorrect placement");
                        // Logique pour gérer un mauvais emplacement (optionnelle)
                    }
                }
            }
        }
    }

    // Assurez-vous d'activer le trigger sur les BoxColliders2D de vos emplacements
}