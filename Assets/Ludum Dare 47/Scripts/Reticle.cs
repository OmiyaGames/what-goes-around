using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare47
{
    [RequireComponent(typeof(Canvas))]
    public class Reticle : MonoBehaviour
    {
        [SerializeField]
        Canvas uiCanvas;
        [SerializeField]
        Image reticle;
        [SerializeField]
        float smoothFactor = 20f;

        Vector3 targetPosition;

        // Update is called once per frame
        void Update()
        {
            // Calculate the target position to move the reticle to
            var screenPoint = Input.mousePosition;
            screenPoint.z = uiCanvas.planeDistance;
            targetPosition = Camera.main.ScreenToWorldPoint(screenPoint);

            // Actually place the reticle
            reticle.transform.position = Vector3.Lerp(reticle.transform.position, targetPosition, (Time.deltaTime * smoothFactor));
        }

        private void Reset()
        {
            uiCanvas = GetComponent<Canvas>();
        }
    }
}
