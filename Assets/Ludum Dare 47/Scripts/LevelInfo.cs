using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LudumDare47
{
    public class LevelInfo : MonoBehaviour
    {
        [SerializeField]
        Transform center;

        /// <summary>
        /// Get the rotation info
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Quaternion GetOrientation(Vector3 worldPosition)
        {
            return Quaternion.FromToRotation(Vector3.forward, GetGravityDirection(worldPosition));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Quaternion GetOrientation(Transform transform)
        {
            return GetOrientation(transform.position);
        }

        /// <summary>
        /// The direction for gravity to affect a location
        /// </summary>
        /// <param name="worldPosition"></param>
        /// <returns></returns>
        public Vector3 GetGravityDirection(Vector3 worldPosition)
        {
            Vector3 returnDirection = center.position - worldPosition;
            returnDirection.Normalize();
            return returnDirection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Vector3 GetGravityDirection(Transform transform)
        {
            return GetGravityDirection(transform.position);
        }
    }
}
