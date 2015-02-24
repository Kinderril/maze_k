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
        private Rigidbody targetRig;

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
                case FollowType.fullLook:
                    targetRig = target.GetComponent<Rigidbody>();
                    if (targetRig != null)
                    {

                        action = FullFollowLook;
                    }
                    else
                    {
                        action = FullFollow;
                        
                    }
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
        private void FullFollowLook()
        {
            transform.position = target.position + offset;
            transform.LookAt(new Vector3(transform.position.x + targetRig.angularVelocity.x, 0, transform.position.z + targetRig.angularVelocity.z));
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
    fullLook,
}
