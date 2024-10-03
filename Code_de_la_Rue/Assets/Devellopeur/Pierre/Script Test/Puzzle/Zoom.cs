using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomInZoomOut : MonoBehaviour
{
    Camera mainCamera;

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;
    Vector2 firstTouchPrevPos, secondTouchPrevPos;
    Vector2 midPointPrev, midPointCur;
    Vector3 camOriginalPos;

    [SerializeField]
    float zoomModifierSpeed = 0.1f;

    [SerializeField]
    float zoomMax = 2f, zoomMin = 10f; // Inversé : zoomMax est la plus petite taille de zoom, zoomMin est la plus grande

    [SerializeField]
    RectTransform canvasRect; // Le RectTransform du Canvas

    [SerializeField]
    Text text;

    // Use this for initialization
    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            Vector2 firstTouchCurPos = firstTouch.position;
            Vector2 secondTouchCurPos = secondTouch.position;

            midPointPrev = (firstTouchPrevPos + secondTouchPrevPos) / 2;
            midPointCur = (firstTouchCurPos + secondTouchCurPos) / 2;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouchCurPos - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            camOriginalPos = mainCamera.ScreenToWorldPoint(midPointCur);

            if (touchesPrevPosDifference > touchesCurPosDifference)
                mainCamera.orthographicSize += zoomModifier;
            if (touchesPrevPosDifference < touchesCurPosDifference)
                mainCamera.orthographicSize -= zoomModifier;

            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, zoomMax, zoomMin);

            Vector3 camNewPos = mainCamera.ScreenToWorldPoint(midPointCur);
            Vector3 difference = camOriginalPos - camNewPos;
            mainCamera.transform.position += difference;

            ApplyCameraBounds();
        }

        text.text = "Camera size " + mainCamera.orthographicSize;
    }

    // Limite le mouvement et le zoom de la caméra pour rester dans les limites du Canvas
    void ApplyCameraBounds()
    {
        // Taille du canvas
        Vector3 canvasMin = canvasRect.TransformPoint(canvasRect.rect.min);
        Vector3 canvasMax = canvasRect.TransformPoint(canvasRect.rect.max);

        // Dimensions visibles de la caméra
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Position de la caméra actuelle
        Vector3 camPos = mainCamera.transform.position;

        // Appliquer les limites pour que la caméra ne sorte pas des bords du Canvas
        float minX = canvasMin.x + cameraWidth / 2f;
        float maxX = canvasMax.x - cameraWidth / 2f;
        float minY = canvasMin.y + cameraHeight / 2f;
        float maxY = canvasMax.y - cameraHeight / 2f;

        // Clamping de la position de la caméra
        camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
        camPos.y = Mathf.Clamp(camPos.y, minY, maxY);

        // Appliquer la position corrigée à la caméra
        mainCamera.transform.position = camPos;
    }
}