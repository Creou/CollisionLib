using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace CollisionLib
{
    public class CircleAxisAlignedRectangleCollider : ICollider
    {
        private Game _game;

        public CircleAxisAlignedRectangleCollider(Game game)
        {
            _game = game;
        }

        public bool CanCollide(ICollidable object1, ICollidable object2)
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

        public void Collide(TimeSpan frameDuration, ICollidable object1, ICollidable object2)
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
            Vector2 rect_v1 = new Vector2(object1.Position.X - (object1.Width / 2), object1.Position.Y - (object1.Height / 2));
            Vector2 rect_v2 = new Vector2(object1.Position.X - (object1.Width / 2), object1.Position.Y + (object1.Height / 2));
            Vector2 rect_v3 = new Vector2(object1.Position.X + (object1.Width / 2), object1.Position.Y + (object1.Height / 2));
            Vector2 rect_v4 = new Vector2(object1.Position.X + (object1.Width / 2), object1.Position.Y - (object1.Height / 2));

            Vector2 rect_side1 = rect_v2 - rect_v1;
            Vector2 rect_side2 = rect_v3 - rect_v2;
            Vector2 rect_side3 = rect_v4 - rect_v3;
            Vector2 rect_side4 = rect_v1 - rect_v4;
            Vector2 rect_side1_n = rect_side1;
            Vector2 rect_side2_n =  rect_side2;
            Vector2 rect_side3_n =  rect_side3;
            Vector2 rect_side4_n =  rect_side4;
            rect_side1_n.Normalize();
            rect_side2_n.Normalize();
            rect_side3_n.Normalize();
            rect_side4_n.Normalize();

            Vector2 w = object2.Position - rect_v1;
            //w.Normalize();
            float d = ((rect_side1 * w).Length() / rect_side1.Length());

            float v = Vector2.Dot(w, rect_side1);
            var angleBetween = MathHelper.ToDegrees((float)Math.Acos(v / (w.Length() * rect_side1.Length())));


            Debug.WriteLine("D: {0} {1}", angleBetween, d);



            //http://softsurfer.com/Archive/algorithm_0102/algorithm_0102.htm
            //http://softsurfer.com/Archive/algorithm_0104/algorithm_0104.htm


            //Vector2 rect_side1_normal = new Vector2(-1 * rect_side1.Y, rect_side1.X);
        }
    }
}
