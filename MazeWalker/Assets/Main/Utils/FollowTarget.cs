using System;
using UnityEngine;

    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);
        public FollowType type = FollowType.full;
        private Action action;

        void Awake()
        {
            switch (type)
            {
                case FollowType.full:
                    action = FullFollow;
                    break;
                case FollowType.xy:
                    action = XZFollow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LateUpdate()
        {
            action();
        }

        private void FullFollow()
        {
            transform.position = target.position + offset;
        }

        private void XZFollow()
        {
            transform.position = new Vector3(target.position.x, 0, target.position.z) + offset;
        }
    }

public enum FollowType
{
    full,
    xy,
}
