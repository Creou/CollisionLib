using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;

namespace CollisionLib
{   
    public class CollisionManager
    {
        IEnumerable<CollilisionPair> _collisionPairs;
        Collection<ICollider> _colliders;

        public CollisionManager(Game game, IEnumerable<ICollidable> collideableObjects)
        {
            // Create the colliders.
            _colliders = new Collection<ICollider>();
            _colliders.Add(new CircleToCircleCollider(game));
            _colliders.Add(new CircleAxisAlignedRectangleCollider(game));

            // Search for all the pairs of collidable objects.
            var allCollisionPairs = new Collection<CollilisionPair>();
            foreach (var object1 in collideableObjects)
            {
                foreach (var object2 in collideableObjects.Where(o => o != object1))
                {
                    allCollisionPairs.Add(new CollilisionPair(object1, object2));
                }
            }

            // Remove duplicates.
            _collisionPairs = allCollisionPairs.Distinct(new DistinctPairDetector()).ToArray();

            // Locate the collider for each pair.
            foreach (var pair in _collisionPairs)
            {
                ICollider collider = ResolveCollisionCalculation(pair);
                pair.BindCollider(collider);
            }
        }

        private ICollider ResolveCollisionCalculation(CollilisionPair pair)
        {
            return _colliders.SingleOrDefault(c => c.CanCollide(pair.Object1, pair.Object2));
        }

        public void ApplyCollisions(TimeSpan frameDuration)
        {
            foreach (var pair in _collisionPairs)
            {
                pair.TriggerCollisionCalculation(frameDuration);
            }
        }      
    }
}
