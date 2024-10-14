using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe_menu : MonoBehaviour
{
    // Référence à l'objet de la barre de défilement
    public GameObject scrollbar;

    // Position actuelle du défilement
    private float scroll_pos = 0;

    // Tableau contenant les positions normalisées des éléments dans le défilement
    private float[] pos;

    // Sensibilité du défilement
    public float sensitivity = 0.5f; // Modifier la sensibilité

    // État de défilement
    private bool isScrolling = false;

    // Start est appelé avant la première frame
    void Start()
    {
        InitializePositions();
    }

    // Initialiser les positions des éléments
    private void InitializePositions()
    {
        int childCount = transform.childCount;
        pos = new float[childCount];
        float distance = 1f / (childCount - 1f);

        for (int i = 0; i < childCount; i++)
        {
            pos[i] = distance * i;
        }
    }

    // Update est appelé une fois par frame
    void Update()
    {
        // Récupérer la position actuelle de la barre de défilement une seule fois
        scroll_pos = scrollbar.GetComponent<Scrollbar>().value;

        // Ajustement automatique vers la position la plus proche
        for (int i = 0; i < pos.Length; i++)
        {
            if (IsCloseEnough(scroll_pos, pos[i]))
            {
                // Interpolation linéaire pour ajuster la valeur de la barre de défilement vers la position cible
                if (!isScrolling) // Ne fait pas l'interpolation si déjà en train de scroller
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }

                // Mise à jour de l'échelle des éléments
                UpdateScale(i);
                break; // Sortir de la boucle après avoir trouvé la position la plus proche
            }
        }

        // Gérer le défilement
        if (Input.GetMouseButton(0))
        {
            isScrolling = true; // Indique que l'utilisateur défile
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isScrolling = false; // Réinitialise l'état lorsque le doigt est relâché
        }
    }

    // Vérifie si la position actuelle est proche d'une position cible
    private bool IsCloseEnough(float currentPosition, float targetPosition)
    {
        float distance = 1f / (pos.Length - 1f) * sensitivity / 2.5f;
        return currentPosition < targetPosition + distance && currentPosition > targetPosition - distance;
    }

    // Mise à jour de l'échelle des éléments en fonction de leur proximité
    private void UpdateScale(int currentIndex)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            Transform child = transform.GetChild(i); // Récupérer le transform une seule fois
            if (i == currentIndex)
            {
                // Met à l'échelle l'élément actuel pour le mettre en évidence
                child.localScale = Vector2.Lerp(child.localScale, new Vector2(1f, 1f), 0.1f);
            }
            else
            {
                // Réduit l'échelle des autres éléments
                child.localScale = Vector2.Lerp(child.localScale, new Vector2(0.8f, 0.8f), 0.1f);
            }
        }
    }
}