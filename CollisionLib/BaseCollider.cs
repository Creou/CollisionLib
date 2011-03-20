using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollisionLib
{
    public abstract class BaseCollider : ICollider
    {
        public abstract bool CanCollide(ICollidable object1, ICollidable object2);
        public abstract void Collide(TimeSpan frameDuration, ICollidable Object1, ICollidable Object2);

        protected static void CalculatePostCollisionVelocityVectors(ICollidable object1, ICollidable object2, Vector2 normalPlane, Vector2 collisionPlane, out Vector2 vel1_after, out Vector2 vel2_after)
        {
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
            vel1_after = vec_n_vel1_after + vec_c_vel1;
            vel2_after = vec_n_vel2_after + vec_c_vel2;
        }
    }
}
