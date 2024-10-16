using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Swipe_menu : MonoBehaviour
{
    // R�f�rence � l'objet de la barre de d�filement
    public GameObject scrollbar;

    // Position actuelle du d�filement
    private float scroll_pos = -100;

    // Tableau contenant les positions normalis�es des �l�ments dans le d�filement
    private float[] pos;

    // Sensibilit� du d�filement
    public float sensitivity = 0.5f; // Modifier la sensibilit�

    // �tat de d�filement
    private bool isScrolling = false;

    // Start est appel� avant la premi�re frame
    void Start()
    {
        InitializePositions();
    }

    // Initialiser les positions des �l�ments
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

    // Update est appel� une fois par frame
    void Update()
    {
        // R�cup�rer la position actuelle de la barre de d�filement une seule fois
        scroll_pos = scrollbar.GetComponent<Scrollbar>().value;

        // Ajustement automatique vers la position la plus proche
        for (int i = 0; i < pos.Length; i++)
        {
            if (IsCloseEnough(scroll_pos, pos[i]))
            {
                // Interpolation lin�aire pour ajuster la valeur de la barre de d�filement vers la position cible
                if (!isScrolling) // Ne fait pas l'interpolation si d�j� en train de scroller
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }

                // Mise � jour de l'�chelle des �l�ments
                UpdateScale(i);
                break; // Sortir de la boucle apr�s avoir trouv� la position la plus proche
            }
        }

        // G�rer le d�filement
        if (Input.GetMouseButton(0))
        {
            isScrolling = true; // Indique que l'utilisateur d�file
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isScrolling = false; // R�initialise l'�tat lorsque le doigt est rel�ch�
        }
    }

    // V�rifie si la position actuelle est proche d'une position cible
    private bool IsCloseEnough(float currentPosition, float targetPosition)
    {
        float distance = 1f / (pos.Length - 1f) * sensitivity / 2.5f;
        return currentPosition < targetPosition + distance && currentPosition > targetPosition - distance;
    }

    // Mise � jour de l'�chelle des �l�ments en fonction de leur proximit�
    private void UpdateScale(int currentIndex)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            Transform child = transform.GetChild(i); // R�cup�rer le transform une seule fois
            if (i == currentIndex)
            {
                // Met � l'�chelle l'�l�ment actuel pour le mettre en �vidence
                child.localScale = Vector2.Lerp(child.localScale, new Vector2(1f, 1f), 0.1f);
            }
            else
            {
                // R�duit l'�chelle des autres �l�ments
                child.localScale = Vector2.Lerp(child.localScale, new Vector2(0.8f, 0.8f), 0.1f);
            }
        }
    }
}