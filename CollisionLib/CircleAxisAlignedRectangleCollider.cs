using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace CollisionLib
{
    public class CircleAxisAlignedRectangleCollider : BaseCollider
    {
        private Game _game;

        public CircleAxisAlignedRectangleCollider(Game game)
        {
            _game = game;
        }

        public override bool CanCollide(ICollidable object1, ICollidable object2)
        {
            ICollidableCircle typeCorrectedObject1 = object1 as ICollidableCircle;
            IColliadableAxisAlignedRectangle typeCorrectedObject2 = object2 as IColliadableAxisAlignedRectangle;
            if (typeCorrectedObject1 != null && typeCorrectedObject2 != null)
            {
                return true;
            }
            else
            {
                IColliadableAxisAlignedRectangle typeCorrectedObject1_reverse = object1 as IColliadableAxisAlignedRectangle;
                ICollidableCircle typeCorrectedObject2_reverse = object2 as ICollidableCircle;

                if (typeCorrectedObject1_reverse != null && typeCorrectedObject2_reverse != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public override void Collide(TimeSpan frameDuration, ICollidable object1, ICollidable object2)
        {
            ICollidableCircle typeCorrectedObject1 = object1 as ICollidableCircle;
            IColliadableAxisAlignedRectangle typeCorrectedObject2 = object2 as IColliadableAxisAlignedRectangle;
            if (typeCorrectedObject1 != null && typeCorrectedObject2 != null)
            {
                Collide(frameDuration, typeCorrectedObject2, typeCorrectedObject1);
            }
            else
            {
                IColliadableAxisAlignedRectangle typeCorrectedObject1_reverse = object1 as IColliadableAxisAlignedRectangle;
                ICollidableCircle typeCorrectedObject2_reverse = object2 as ICollidableCircle;

                if (typeCorrectedObject1_reverse != null && typeCorrectedObject2_reverse != null)
                {
                    Collide(frameDuration, typeCorrectedObject1_reverse, typeCorrectedObject2_reverse);
                }
            }
        }

        public void Collide(TimeSpan frameDuration, IColliadableAxisAlignedRectangle object1, ICollidableCircle object2)
        {
            // Calculate the 4 corners of the rectangle.
            Vector2 rectVertex_v1 = new Vector2(object1.Position.X - (object1.Width / 2), object1.Position.Y - (object1.Height / 2));
            Vector2 rectVertex_v2 = new Vector2(object1.Position.X - (object1.Width / 2), object1.Position.Y + (object1.Height / 2));
            Vector2 rectVertex_v3 = new Vector2(object1.Position.X + (object1.Width / 2), object1.Position.Y + (object1.Height / 2));
            Vector2 rectVertex_v4 = new Vector2(object1.Position.X + (object1.Width / 2), object1.Position.Y - (object1.Height / 2));

            // Calculate the 4 corners of the outer collision zone (based on the size of the circle).
            Vector2 rectOuterVertex_v1 = new Vector2(object1.Position.X - (object1.Width / 2) - object2.Radius, object1.Position.Y - (object1.Height / 2) - object2.Radius);
            Vector2 rectOuterVertex_v2 = new Vector2(object1.Position.X - (object1.Width / 2) - object2.Radius, object1.Position.Y + (object1.Height / 2) + object2.Radius);
            Vector2 rectOuterVertex_v3 = new Vector2(object1.Position.X + (object1.Width / 2) + object2.Radius, object1.Position.Y + (object1.Height / 2) + object2.Radius);
            Vector2 rectOuterVertex_v4 = new Vector2(object1.Position.X + (object1.Width / 2) + object2.Radius, object1.Position.Y - (object1.Height / 2) - object2.Radius);

            // Calculate the 4 outer zone sides.
            Vector2 rectOuterSide1 = rectOuterVertex_v2 - rectOuterVertex_v1;
            Vector2 rectOuterSide2 = rectOuterVertex_v3 - rectOuterVertex_v2;
            Vector2 rectOuterSide3 = rectOuterVertex_v4 - rectOuterVertex_v3;
            Vector2 rectOuterSide4 = rectOuterVertex_v1 - rectOuterVertex_v4;

            // Calcualte the vectors & distances to each of the outer corners.
            Vector2 vecToOuterV1 = object2.Position - rectOuterVertex_v1;
            Vector2 vecToOuterV2 = object2.Position - rectOuterVertex_v2;
            Vector2 vecToOuterV3 = object2.Position - rectOuterVertex_v3;
            Vector2 vecToOuterV4 = object2.Position - rectOuterVertex_v4;

            float distToOuterSide1 = ((rectOuterSide1 * vecToOuterV1).Length() / rectOuterSide1.Length());
            float distToOuterSide2 = ((rectOuterSide2 * vecToOuterV2).Length() / rectOuterSide2.Length());
            float distToOuterSide3 = ((rectOuterSide3 * vecToOuterV3).Length() / rectOuterSide3.Length());
            float distToOuterSide4 = ((rectOuterSide4 * vecToOuterV4).Length() / rectOuterSide4.Length());

            // Calcualte the vectors & distances to each of the actual corners.
            Vector2 vecToVertex1 = object2.Position - rectVertex_v1;
            Vector2 vecToVertex2 = object2.Position - rectVertex_v2;
            Vector2 vecToVertex3 = object2.Position - rectVertex_v3;
            Vector2 vecToVertex4 = object2.Position - rectVertex_v4;

            float distToVetex1 = vecToVertex1.Length();
            float distToVetex2 = vecToVertex2.Length();
            float distToVetex3 = vecToVertex3.Length();
            float distToVetex4 = vecToVertex4.Length();

            bool isPastSide1 = Vector2.Dot(vecToOuterV1, rectOuterSide1) > 0;
            bool isPastSide2 = Vector2.Dot(vecToOuterV2, rectOuterSide2) > 0;
            bool isPastSide3 = Vector2.Dot(vecToOuterV3, rectOuterSide3) > 0;
            bool isPastSide4 = Vector2.Dot(vecToOuterV4, rectOuterSide4) > 0;

            // Code for calculating the angle between. Not needed any more.
            //float c = v / (vecToV1.Length() * rect_side1.Length());
            //var angleBetween = MathHelper.ToDegrees((float)Math.Acos(c));

            /* There are 8 collision zones:
             *    _____________
             *    : 1 __8__ 7 :
             *    :   |   |   :
             *    : 2 |   | 6 :
             *    :   |___|   :
             *    :_3___4___5_:
             *    
             * Zones 2, 4, 6 & 8 are collisions with a flat surface of the AA rectangle.
             * Zones 1, 3, 5 & 7 are *potential* collisions with a corner. 
             * In these zones we must test again that the circle has actually hit the corner of the AA rect.
             */

            bool collision = false;
            Vector2 collisionPlane = Vector2.Zero;
            Vector2 normalPlane = Vector2.Zero;
            if (isPastSide1 && isPastSide2 && isPastSide3 && isPastSide4)
            {
                // The centre of the cirle is somewhere within the outer collision zone.
                // Now figure out which zone it's in and find the collision & normal planes.
                if (distToOuterSide1 < object2.Radius)
                {
                    // Zones 1, 8, 7
                    if (distToOuterSide2 < object2.Radius)
                    {
                        // Zone 1.
                        collision = distToVetex1 < object2.Radius;
                        if (collision)
                        {
                            normalPlane = vecToVertex1;
                            normalPlane.Normalize();
                            collisionPlane = new Vector2(-normalPlane.Y, normalPlane.X);
                            collisionPlane.Normalize();
                        }
                    }
                    else if (distToOuterSide3 < object2.Radius)
                    {
                        // Zone 7.
                        collision = distToVetex2 < object2.Radius;
                        if (collision)
                        {
                            normalPlane = vecToVertex4;
                            normalPlane.Normalize();
                            collisionPlane = new Vector2(-normalPlane.Y, normalPlane.X);
                            collisionPlane.Normalize();
                        }
                    }
                    else
                    {
                        // Zone 8.
                        collision = true;
                        collisionPlane = rectOuterSide4;
                        collisionPlane.Normalize();
                        normalPlane = new Vector2(-1 * collisionPlane.Y, collisionPlane.X);
                        normalPlane.Normalize();
                    }
                }
                else if (distToOuterSide3 < object2.Radius)
                {
                    // Zones 3, 4, 5.
                    if (distToOuterSide2 < object2.Radius)
                    {
                        // Zone 3.
                        collision = distToVetex3 < object2.Radius;
                        if (collision)
                        {
                            normalPlane = vecToVertex2;
                            normalPlane.Normalize();
                            collisionPlane = new Vector2(-normalPlane.Y, normalPlane.X);
                            collisionPlane.Normalize();
                        }
                    }
                    else if (distToOuterSide3 < object2.Radius)
                    {
                        // Zone 5.
                        collision = distToVetex4 < object2.Radius;
                        if (collision)
                        {
                            normalPlane = vecToVertex3;
                            normalPlane.Normalize();
                            collisionPlane = new Vector2(-normalPlane.Y, normalPlane.X);
                            collisionPlane.Normalize();
                        }
                    }
                    else
                    {
                        // Zone 4.
                        collision = true;
                        collisionPlane = rectOuterSide2;
                        collisionPlane.Normalize();
                        normalPlane = new Vector2(-1 * collisionPlane.Y, collisionPlane.X);
                        normalPlane.Normalize();
                    }
                }
                else if (distToOuterSide2 < 50)
                {
                    // Zone 2.
                    collision = true;
                    collisionPlane = rectOuterSide1;
                    collisionPlane.Normalize();
                    normalPlane = new Vector2(-1 * collisionPlane.Y, collisionPlane.X);
                    normalPlane.Normalize();
                }
                else if (distToOuterSide4 < 50)
                {
                    // Zone 6.
                    collision = true;
                    collisionPlane = rectOuterSide3;
                    collisionPlane.Normalize();
                    normalPlane = new Vector2(-1 * collisionPlane.Y, collisionPlane.X);
                    normalPlane.Normalize();
                }

                if (collision)
                {
                    // Once we know there was a collision, we can apply it to the objects.
                    Vector2 vel1_after;
                    Vector2 vel2_after;
                    CalculatePostCollisionVelocityVectors(object1, object2, normalPlane, collisionPlane, out vel1_after, out vel2_after);

                    // Set the objects new positions and velocities.
                    object1.SetVelocity(vel1_after);
                    object2.SetVelocity(vel2_after);
                }
            }
        }
    }
}
