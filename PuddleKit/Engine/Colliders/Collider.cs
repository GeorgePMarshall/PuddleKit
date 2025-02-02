namespace PuddleKit.Engine.Colliders
{
    /// <summary>
    /// Class to represent a collision volume
    /// </summary>
    public abstract class Collider
    {
        /// <summary>
        /// Checks if the Collision volume intersects with another.
        /// </summary>
        /// <param name="other">The other collision volume to check against</param>
        /// <returns>Returns true if the collision volumes intersect, otherwise false</returns>
        public abstract bool CollidesWith(Collider other);
    }
}