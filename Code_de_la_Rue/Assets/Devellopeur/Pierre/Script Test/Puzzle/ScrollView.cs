using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    public ScrollRect scrollRect; // R�f�rence au Scroll Rect
    private Coroutine checkCoroutine;
    private Vector2 previousValue;

    void Start()
    {
        scrollRect.onValueChanged.AddListener(OnScrollValueChanged);
    }

    public void OnScrollValueChanged(Vector2 value)
    {
        Debug.Log(value);

            if (InputManager.isDraggingObject == false)
            {
                // D�sactive le mouvement de la cam�ra lors du d�filement
                InputManager.isDraggingObject = true; // Indique que l'objet est en cours de drag
            }

            // Mettez � jour la valeur pr�c�dente
            previousValue = value;

            // Si une coroutine est d�j� en cours, l'arr�ter
            if (checkCoroutine != null)
            {
                StopCoroutine(checkCoroutine);
            }
            // D�marrer la nouvelle coroutine
            checkCoroutine = StartCoroutine(CheckIfScrollingStops());
    }

    private IEnumerator CheckIfScrollingStops()
    {
        // Attendre x secondes 
        yield return new WaitForSeconds(0.5f);

        // V�rifiez si la valeur n'a pas chang� pendant le d�lai
        while (true)
        {
            // Si la valeur est la m�me que celle de la derni�re mise � jour, arr�tez le d�filement
            if (scrollRect.normalizedPosition == previousValue)
            {
                Debug.Log("Arr�t");
                InputManager.isDraggingObject = false; // Remettre � false
                yield break; // Quitte la coroutine
            }
            yield return null; // Attendre le prochain frame
        }
    }
}