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

        [Header("Look Ahead")]
        [SerializeField]
        float lookAhead = 2.5f;
        [SerializeField]
        float clampDistance = 10f;

        Quaternion targetRotation;
        Vector3 targetPosition, cacheLookAhead3;
        Plane rotatePlane = new Plane();

        private void Start()
        {
            Game.Reticle.CanvasDistance = positionOffset.magnitude;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // Calculate the target position
            targetPosition = Game.Player.transform.position;

            // Grab the closest point the reticle is on the player's plane
            cacheLookAhead3 = Gun.GetPointOnPlane(Game.Player.transform, Game.Reticle.AimPosition, Game.Player.transform.forward, ref rotatePlane);

            // Convert the world position to local
            cacheLookAhead3 = Game.Player.transform.InverseTransformPoint(cacheLookAhead3);
            float sqrMagnitude = cacheLookAhead3.sqrMagnitude;
            if (Mathf.Approximately(sqrMagnitude, 0f) == false)
            {
                // Normalize look-ahead direction
                float scaledMagnitude = Mathf.Sqrt(sqrMagnitude);
                cacheLookAhead3 /= scaledMagnitude;

                // Scale distance by clampDistance
                scaledMagnitude = Mathf.Clamp(scaledMagnitude, 0f, clampDistance);
                scaledMagnitude /= clampDistance;

                // Multiply by LookAhead
                scaledMagnitude *= lookAhead;

                // Offset the target position by direction of reticle
                targetPosition += Game.Player.Orientation * (cacheLookAhead3 * scaledMagnitude);
            }
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
