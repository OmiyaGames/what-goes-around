using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare47
{
    public class FollowCamera : MonoBehaviour
    {
        [Header("Positioning")]
        [SerializeField]
        Vector3 positionOffset = new Vector3(0, 0, -5f);
        [SerializeField]
        float positionSmoothFactor = 10f;
        [SerializeField]
        float rotationSmoothFactor = 10f;

        Quaternion targetRotation;
        Vector3 targetPosition;

        private void Start()
        {
            Game.Reticle.CanvasDistance = positionOffset.magnitude;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Calculate the target position
            targetPosition = Game.Player.transform.position;
            targetPosition += (Game.Player.Orientation * positionOffset);

            // Calculate the target rotation
            targetRotation = Game.Player.Orientation;

            // Update the camera
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, targetPosition, (positionSmoothFactor * Time.deltaTime)),
                Quaternion.Lerp(transform.rotation, targetRotation, (rotationSmoothFactor * Time.deltaTime))
                );
        }
    }
}
