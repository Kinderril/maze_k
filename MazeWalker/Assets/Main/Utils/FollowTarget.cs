using System;
using UnityEngine;

    public class FollowTarget : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);
        public FollowType type = FollowType.full;
        private Action action;
        public Animator anim;
        private bool isAnim = false;

        void Awake()
        {
            anim = GetComponent<Animator>();
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
            //if (!isAnim)
                action();
        }

        private void FullFollow()
        {
            transform.position = target.position + offset;
        }

        private void XZFollow()
        {
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);// + offset;
        }

        public void StartFly()
        {
            anim.SetBool("go",true);
            isAnim = true;
        }

        public void EndFly()
        {
            anim.SetBool("go", false);
            isAnim = false;
        }

    }

public enum FollowType
{
    full,
    xy,
}
