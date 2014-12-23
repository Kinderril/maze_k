using System;
using UnityEngine;

namespace UnitySampleAssets.Vehicles.Ball
{
    public class Ball : MonoBehaviour
    {
        [SerializeField] private bool m_UseTorque = true; // Whether or not to use torque to move the ball.
        [SerializeField] private float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.
        [SerializeField] public float power = 1;

        private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.
        private Rigidbody m_Rigidbody;


        private void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            // Set the maximum angular velocity.
            GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
        }
        public void Move(Vector2 moveDirection)
        {
            moveDirection *= power;
            // If using torque to rotate the ball...
            if (m_UseTorque)
            {
                // ... add torque around the axis defined by the move direction.
                m_Rigidbody.AddTorque(new Vector3(moveDirection.x, 0, moveDirection.y));
            }
            else
            {
                // Otherwise add force in the move direction.
                m_Rigidbody.AddForce(new Vector3(moveDirection.x, 0, moveDirection.y));
            }

        }
    }
}
