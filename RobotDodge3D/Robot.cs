using OpenTK.Mathematics;
using PuddleKit;
using PuddleKit.Engine;
using PuddleKit.Engine.Colliders;
using PuddleKit.Engine.Utilities;
using PuddleKit.Renderer;
using PuddleKit.Renderer.Meshes;

namespace RobotDodge3D
{
    /// <summary>
    /// A robot class representing the enemies in the game
    /// </summary>
    public class Robot : GameObject
    {
        private Vector3 _velocity;

        /// <summary>
        /// The speed of the robot in units/s
        /// </summary>
        public float Speed { get; private set; } = 2;

        /// <summary>
        /// Initializes a new instance of the Robot class.
        /// <para></para>Robot position will be spanned at a random location on the perimeter of the play area
        /// </summary>
        /// <param name="mesh">The mesh the robot will render</param>
        /// <param name="texture">The texture to be applied to the mesh</param>
        /// <param name="targetPlayer">The player the robot will run towards</param>
        /// <param name="playArea">The play area that robot will spawn outside of</param>
        public Robot(Mesh mesh, Texture texture, Player targetPlayer, Rectangle playArea) : base(mesh, texture)
        {
            Vector3 newPosition = new Vector3();

            // set poition to random point on perimeter of play area
            if (Random.Shared.Next(2) == 0)
            {
                newPosition.Z = Random.Shared.Next(2) == 1 ? playArea.top : playArea.bottom + 1;
                newPosition.X = Random.Shared.Next((int)playArea.left, (int)playArea.right + 1);
            }
            else
            {
                newPosition.X = Random.Shared.Next(2) == 1 ? playArea.left : playArea.right + 1;
                newPosition.Z = Random.Shared.Next((int)playArea.top, (int)playArea.bottom + 1);
            }

            this.Transform.Position = newPosition;
            this.Transform.Scale = new Vector3(0.3f);

            //calculate direction and rotation
            Vector3 direction = targetPlayer.Transform.Position - this.Transform.Position;
            direction.Y = 0;
            direction.Normalize();
            _velocity = direction * this.Speed;
            float rot = (float)MathHelper.Atan2(direction.X, direction.Z);
            this.Transform.Rotation = new Vector3(0f, MathHelper.RadiansToDegrees(rot), 0f);

            //add collider
            this.Collider = new SphereCollider(0.3f, this);
        }

        /// <summary>
        /// Updates the robot position based on its velocity
        /// </summary>
        /// <param name="gameManager">The owning game manager</param>
        public override void Update(GameManager gameManager)
        {
            base.Update(gameManager);

            this.Transform.Position += _velocity * gameManager.DeltaTime;
        }
    }
}