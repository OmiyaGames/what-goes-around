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
        public Quaternion GetOrientation(Vector3 worldPosition, Vector3 localUp)
        {
            return Quaternion.LookRotation(GetGravityDirection(worldPosition), localUp);
            //return Quaternion.FromToRotation(Vector3.forward, GetGravityDirection(worldPosition));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public Quaternion GetOrientation(Transform transform, Vector3 localUp)
        {
            return GetOrientation(transform.position, localUp);
        }

        public Quaternion GetOrientation(Transform transform)
        {
            return GetOrientation(transform, transform.up);
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
