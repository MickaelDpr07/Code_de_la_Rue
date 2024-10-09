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
    float zoomMax = 2f, zoomMin = 10f;

    [SerializeField]
    RectTransform canvasRect;

    Vector3 lastSingleTouchPos;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.touchCount == 2 && InputManager.isDraggingObject == false) // Ignorer si un objet est déplacé
        {
            // Zoom à deux doigts
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
        else if (Input.touchCount == 1 && InputManager.isDraggingObject == false) // Ignorer si un objet est déplacé
        {
            // Déplacement avec un doigt
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastSingleTouchPos = mainCamera.ScreenToWorldPoint(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 currentTouchPos = mainCamera.ScreenToWorldPoint(touch.position);
                Vector3 difference = lastSingleTouchPos - currentTouchPos;

                mainCamera.transform.position += difference;
                ApplyCameraBounds();

                lastSingleTouchPos = mainCamera.ScreenToWorldPoint(touch.position);
            }
        }
    }

    void ApplyCameraBounds()
    {
        Vector3 canvasMin = canvasRect.TransformPoint(canvasRect.rect.min);
        Vector3 canvasMax = canvasRect.TransformPoint(canvasRect.rect.max);

        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector3 camPos = mainCamera.transform.position;

        float minX = canvasMin.x + cameraWidth / 2f;
        float maxX = canvasMax.x - cameraWidth / 2f;
        float minY = canvasMin.y + cameraHeight / 2f;
        float maxY = canvasMax.y - cameraHeight / 2f;

        camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
        camPos.y = Mathf.Clamp(camPos.y, minY, maxY);

        mainCamera.transform.position = camPos;
    }
}