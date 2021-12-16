using UnityEngine;

namespace AI
{
    public class PhysicsHelpers
    {
        public static Vector3 CalculateLaunchVelocity(Vector3 target, Transform fireTransform)
        {
            Vector3 ownerFirePosition = fireTransform.position;
            const float fireArcPeak = TankShooting.fireArcPeak;

            //Get the difference in positions between the target and this tank
            float displacementY = target.y - ownerFirePosition.y;
            Vector3 displacementXZ =
                new Vector3(target.x - ownerFirePosition.x, 0, target.z - ownerFirePosition.z);

            //Calculate Vertical and Horizontal Velocity
            //V(up) = sqrt(-2gh) from Kinematic Equasions
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics.gravity.y * fireArcPeak);
            //V(right) = Px / srqt(-(2h)/g) + sqrt(2(Py - h) / g) from Kinematic Equasions
            Vector3 velocityXZ =
                displacementXZ / (Mathf.Sqrt(-2 * fireArcPeak / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementY - fireArcPeak)/Physics.gravity.y));

            return velocityY + velocityXZ;
        }
    }
}