using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour
{
    public ScrollRect scrollRect; // Référence au Scroll Rect
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
                // Désactive le mouvement de la caméra lors du défilement
                InputManager.isDraggingObject = true; // Indique que l'objet est en cours de drag
            }

            // Mettez à jour la valeur précédente
            previousValue = value;

            // Si une coroutine est déjà en cours, l'arrêter
            if (checkCoroutine != null)
            {
                StopCoroutine(checkCoroutine);
            }
            // Démarrer la nouvelle coroutine
            checkCoroutine = StartCoroutine(CheckIfScrollingStops());
    }

    private IEnumerator CheckIfScrollingStops()
    {
        // Attendre x secondes 
        yield return new WaitForSeconds(0.5f);

        // Vérifiez si la valeur n'a pas changé pendant le délai
        while (true)
        {
            // Si la valeur est la même que celle de la dernière mise à jour, arrêtez le défilement
            if (scrollRect.normalizedPosition == previousValue)
            {
                Debug.Log("Arrêt");
                InputManager.isDraggingObject = false; // Remettre à false
                yield break; // Quitte la coroutine
            }
            yield return null; // Attendre le prochain frame
        }
    }
}