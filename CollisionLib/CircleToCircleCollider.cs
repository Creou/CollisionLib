using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollisionLib
{
    public class CircleToCircleCollider : ICollider
    {
        private Game _game;
        public CircleToCircleCollider(Game game)
        {
            _game = game;
        }

        public bool CanCollide(ICollidable object1, ICollidable object2)
        {
            ICollidableCircle typeCorrectedObject1 = object1 as ICollidableCircle;
            ICollidableCircle typeCorrectedObject2 = object2 as ICollidableCircle;

            return (typeCorrectedObject1 != null && typeCorrectedObject2 != null);
        }

        public void Collide(TimeSpan frameDuration, ICollidable object1, ICollidable object2)
        {
            Collide(frameDuration, (ICollidableCircle)object1, (ICollidableCircle)object2);
        }

        public void Collide(TimeSpan frameDuration, ICollidableCircle object1, ICollidableCircle object2)
        {
            // Calculate the difference between the two objects.
            Vector2 difference = object1.Position - object2.Position;
            float distanceAtFrameEnd = difference.Length();

            // Calculate the distance that a collision would occur at.
            float collisionDistance = (object1.Diameter / 2f) + (object2.Diameter / 2f);

            // Check of the objects are closer that the collision distance.
            if (distanceAtFrameEnd < collisionDistance)
            {
                // Move both objects back to the exact point of collision.
                float millisecondsAfterCollision = MoveBackToCollisionPoint(frameDuration, object1, object2, distanceAtFrameEnd, collisionDistance);

                // Calculate the normal of the collision plane.
                Vector2 normalPlane = difference;
                normalPlane.Normalize();

                // Calculate the collision plane.
                Vector2 collisionPlane = new Vector2(-normalPlane.Y, normalPlane.X);

                // Calculate prior velocities relative the the collision plane and normal.
                float n_vel1 = Vector2.Dot(normalPlane, object1.Velocity);
                float c_vel1 = Vector2.Dot(collisionPlane, object1.Velocity);
                float n_vel2 = Vector2.Dot(normalPlane, object2.Velocity);
                float c_vel2 = Vector2.Dot(collisionPlane, object2.Velocity);

                // Calculate the scaler velocities of each object after the collision.
                float n_vel1_after = ((n_vel1 * (object1.Mass - object2.Mass)) + (2 * object2.Mass * n_vel2)) / (object2.Mass + object1.Mass);
                float n_vel2_after = ((n_vel2 * (object2.Mass - object1.Mass)) + (2 * object1.Mass * n_vel1)) / (object2.Mass + object1.Mass);
                //float velObject2Tangent_After = c_vel2;
                //float velObject1Tangent_After = c_vel1;

                // Convert the scalers to vectors by multiplying by the normalised plane vectors.
                Vector2 vec_n_vel2_after = n_vel2_after * normalPlane;
                Vector2 vec_c_vel2 = c_vel2 * collisionPlane;
                Vector2 vec_n_vel1_after = n_vel1_after * normalPlane;
                Vector2 vec_c_vel1 = c_vel1 * collisionPlane;

                // Combine the vectors back into a single vector in world space.
                Vector2 vel1_after = vec_n_vel1_after + vec_c_vel1;
                Vector2 vel2_after = vec_n_vel2_after + vec_c_vel2;

                // Reapply the move-back from before the collision (using the post collision velocity)
                Vector2 object1AdjustedPositionAfterCollision = object1.Position + vel1_after * millisecondsAfterCollision;
                Vector2 object2AdjustedPositionAfterCollision = object2.Position + vel2_after * millisecondsAfterCollision;

                // Set the objects new positions and velocities.
                object1.SetState(object1AdjustedPositionAfterCollision, vel1_after);
                object2.SetState(object2AdjustedPositionAfterCollision, vel2_after);
            }
        }

        private static float MoveBackToCollisionPoint(TimeSpan frameDuration, ICollidableCircle object1, ICollidableCircle object2, float distanceAtFrameEnd, float collisionDistance)
        {
            // Calculate the position of each object at the start of the frame.
            float object1PosAtFrameStart_X = (float)(object1.Position.X - object1.Velocity.X * frameDuration.TotalMilliseconds);
            float object1PosAtFrameStart_Y = (float)(object1.Position.Y - object1.Velocity.Y * frameDuration.TotalMilliseconds);
            Vector2 object1PosAtFrameStart = new Vector2(object1PosAtFrameStart_X, object1PosAtFrameStart_Y);

            float object2PosAtFrameStart_X = (float)(object2.Position.X - object2.Velocity.X * frameDuration.TotalMilliseconds);
            float object2PosAtFrameStart_Y = (float)(object2.Position.Y - object2.Velocity.Y * frameDuration.TotalMilliseconds);
            Vector2 object2PosAtFrameStart = new Vector2(object2PosAtFrameStart_X, object2PosAtFrameStart_Y);

            // Calculate the distance between the objects at the start of the frame.
            Vector2 differenceAtFrameStart = object2PosAtFrameStart - object1PosAtFrameStart;
            float distanceAtFrameStart = differenceAtFrameStart.Length();

            // Calculate the total change in distance during the frame, and the required change to reach the collision.
            float distanceTotalDelta = distanceAtFrameEnd - distanceAtFrameStart;
            float distanceDeltaToCollision = collisionDistance - distanceAtFrameStart;

            // Calculate the percentage change to the collision and after the collision.
            float percentageDeltaToCollision = distanceDeltaToCollision / distanceTotalDelta;
            float percentageDeltaAfterCollision = 1 - percentageDeltaToCollision;

            // Calculte the time before and after the collision in the frame.
            double millisecondsToCollision = frameDuration.TotalMilliseconds * percentageDeltaToCollision;
            float millisecondsAfterCollision = (float)(frameDuration.TotalMilliseconds * percentageDeltaAfterCollision);

            // Calculate and move the objects to their positions at the point of collision.
            float object1PosAtCollision_X = (float)(object1PosAtFrameStart_X + object1.Velocity.X * millisecondsToCollision);
            float object1PosAtCollision_Y = (float)(object1PosAtFrameStart_Y + object1.Velocity.Y * millisecondsToCollision);
            Vector2 object1PosAtCollision = new Vector2(object1PosAtCollision_X, object1PosAtCollision_Y);
            object1.SetPosition(object1PosAtCollision);

            float object2PosAtCollision_X = (float)(object2PosAtFrameStart_X + object2.Velocity.X * millisecondsToCollision);
            float object2PosAtCollision_Y = (float)(object2PosAtFrameStart_Y + object2.Velocity.Y * millisecondsToCollision);
            Vector2 object2PosAtCollision = new Vector2(object2PosAtCollision_X, object2PosAtCollision_Y);
            object2.SetPosition(object2PosAtCollision);

            return millisecondsAfterCollision;
        }

        private static float MoveBackToCollisionPoint(TimeSpan frameDuration, ICollidableCircle object1, ICollidableCircle object2)
        {
            float distanceAtFrameEnd = (object1.Position - object2.Position).Length();
            float collisionDistance = (object1.Diameter / 2f) + (object2.Diameter / 2f);

            return MoveBackToCollisionPoint(frameDuration, object1, object2, distanceAtFrameEnd, collisionDistance);
        }
    }
}
