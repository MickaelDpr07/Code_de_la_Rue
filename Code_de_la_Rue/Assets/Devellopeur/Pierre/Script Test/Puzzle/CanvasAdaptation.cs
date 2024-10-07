using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSizeAdjuster : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvasRectTransform;  // Le RectTransform du Canvas

    [SerializeField]
    private SpriteRenderer imageSpriteRenderer; // Le SpriteRenderer de l'image

    void Start()
    {
        if (canvasRectTransform == null)
        {
            Debug.LogError("RectTransform du Canvas non assigné.");
            return;
        }

        if (imageSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer de l'image non assigné.");
            return;
        }

        AdjustCanvasSize();
    }

    // Ajuste la taille du Canvas en fonction de la taille de l'image
    private void AdjustCanvasSize()
    {
        // Récupère les dimensions du Sprite (largeur et hauteur)
        float imageWidth = imageSpriteRenderer.sprite.bounds.size.x;
        float imageHeight = imageSpriteRenderer.sprite.bounds.size.y;

        // Ajuste la taille du RectTransform du Canvas en fonction des dimensions de l'image
        canvasRectTransform.sizeDelta = new Vector2(imageWidth, imageHeight);

        // Optionnel : centrer le Canvas sur l'image
        canvasRectTransform.position = imageSpriteRenderer.transform.position;
    }
}