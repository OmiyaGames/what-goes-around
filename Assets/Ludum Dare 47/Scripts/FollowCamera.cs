using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare47
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        PlayerShip follow;

        [Header("Positioning")]
        [SerializeField]
        Vector3 positionOffset = new Vector3(0, 0, -5f);
        [SerializeField]
        float positionSmoothFactor = 10f;
        [SerializeField]
        float rotationSmoothFactor = 10f;

        Quaternion targetRotation;
        Vector3 targetPosition;

        // Update is called once per frame
        void FixedUpdate()
        {
            // Calculate the target position
            targetPosition = follow.transform.position;
            targetPosition += (follow.Orientation * positionOffset);

            // Calculate the target rotation
            targetRotation = follow.Orientation;

            // Update the camera
            transform.SetPositionAndRotation(
                Vector3.Lerp(transform.position, targetPosition, (positionSmoothFactor * Time.deltaTime)),
                Quaternion.Lerp(transform.rotation, targetRotation, (rotationSmoothFactor * Time.deltaTime))
                );
        }
    }
}
