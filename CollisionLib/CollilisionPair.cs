using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CollisionLib
{
    public class CollilisionPair
    {
        public ICollidable Object1 { get; private set; }
        public ICollidable Object2 { get; private set; }

        private ICollider _collider;

        public CollilisionPair(ICollidable object1, ICollidable object2)
        {
            if (object1 == object2) { throw new InvalidOperationException("Cannot collide an object with itself."); }

            this.Object1 = object1;
            this.Object2 = object2;
        }

        public void BindCollider(ICollider collider)
        {
            _collider = collider;
        }

        internal void TriggerCollisionCalculation(TimeSpan frameDuration)
        {
            Debug.Assert(_collider != null, "No collider could be found for colliding types");
            if (_collider != null)
            {
                _collider.Collide(frameDuration, Object1, Object2);
            }
        }
    }
}
