using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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

        }
    }
}
                