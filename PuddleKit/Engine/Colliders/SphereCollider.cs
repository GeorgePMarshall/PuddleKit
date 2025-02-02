using OpenTK.Mathematics;

namespace PuddleKit.Engine.Colliders
{
    /// <summary>
    /// class to represent a sphereical collision volume with a position and radius
    /// </summary>
    public class SphereCollider : Collider
    {
        /// <summary>
        /// The radius of the sphere
        /// </summary>
        public float Radius { get; private set; }

        /// <summary>
        /// The parent gameobject to use the position from
        /// </summary>
        public GameObject Parent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the SphereCollider class.
        /// </summary>
        /// <param name="radius">The radius of the sphere collision volume</param>
        /// <param name="parent">The parent gameobject to get the position from</param>
        public SphereCollider(float radius, GameObject parent)
        {
            this.Radius = radius;
            this.Parent = parent;
        }

        /// <summary>
        /// Checks if the Collision volume intersects with another.
        /// </summary>
        /// <param name="other">The other collision volume to check against</param>
        /// <returns>Returns true if the collision volumes intersect, otherwise false</returns>
        public override bool CollidesWith(Collider other)
        {
            if (other.GetType() == typeof(SphereCollider))
            {
                return this.SphereSphereCollision((SphereCollider)other);
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check for collsion between two spheres
        /// </summary>
        /// <param name="other">The other collision volume to check against</param>
        /// <returns>Returns true if the spheres intersect, otherwise false</returns>
        private bool SphereSphereCollision(SphereCollider other)
        {
            return Vector3.Distance(this.Parent.Transform.Position, other.Parent.Transform.Position) <= this.Radius + other.Radius;
        }
    }
}