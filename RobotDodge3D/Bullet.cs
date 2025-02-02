using OpenTK.Mathematics;
using PuddleKit.Engine;
using PuddleKit.Renderer;
using PuddleKit.Renderer.Meshes;
using PuddleKit.Engine.Colliders;
using PuddleKit;

namespace RobotDodge3D
{
    /// <summary>
    /// A class representing a bullet projectile to be fired by the player
    /// </summary>
    public class Bullet : GameObject
    {
        private Vector3 _velocity;

        /// <summary>
        /// The speed of the bullet in units/s
        /// </summary>
        public float Speed { get; private set; } = 10;

        /// <summary>
        /// Initializes a new instance of the Bullet class.
        /// </summary>
        /// <param name="bulletMesh"> The mesh the bullet will render</param>
        /// <param name="bulletTexture"> The texture to apply to the bulletMesh</param>
        /// <param name="originPosition"> The starting position of the bullet</param>
        /// <param name="direction">The direction the bullet should travel</param>
        public Bullet(Mesh bulletMesh, Texture bulletTexture, Vector3 originPosition, Vector3 direction) : base(bulletMesh, bulletTexture)
        {
            this.Transform.Position = originPosition;
            this.Transform.Scale = new Vector3(0.3f);

            //set velocity and rotation
            _velocity = direction * this.Speed;
            float rot = (float)MathHelper.Atan2(direction.X, direction.Z);
            this.Transform.Rotation = new Vector3(0f, MathHelper.RadiansToDegrees(rot), 0f);

            //add collider
            this.Collider = new SphereCollider(0.3f, this);
        }

        /// <summary>
        /// Updates the bullet position based on its velocity
        /// </summary>
        /// <param name="gameManager">The owning game manager</param>
        public override void Update(GameManager gameManager)
        {
            base.Update(gameManager);

            this.Transform.Position += _velocity * gameManager.DeltaTime;
        }
    }
}