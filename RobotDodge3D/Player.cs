using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using PuddleKit.Engine;
using PuddleKit.Renderer;
using PuddleKit.Renderer.Meshes;
using PuddleKit.Engine.Colliders;

namespace RobotDodge3D
{
    /// <summary>
    /// A class for the player object that will handle its own movement and fire projectiles
    /// </summary>
    public class Player : GameObject
    {
        /// <summary>
        /// The maximum lives the player can have
        /// </summary>
        public int MaxLives { get; private set; }

        /// <summary>
        /// The current lives the players has
        /// </summary>
        public int Lives { get; private set; }

        /// <summary>
        /// The current score of the player
        /// </summary>
        public float Score { get; private set; }

        /// <summary>
        /// The movment speed of the player in units/s
        /// </summary>
        public float Speed { get; set; } = 5;

        /// <summary>
        /// Initializes a new instance of the Player class.
        /// </summary>
        /// <param name="shaderProgram">The shader program to draw the player mesh</param>
        /// <param name="startingLives">The amount of live the player should start with (default = 5)</param>
        public Player(ShaderProgram shaderProgram, int startingLives = 5) : base(new ModelMesh(shaderProgram, "Robot/Robot.obj"), new Texture("Robot/Robot.png"))
        {
            this.Collider = new SphereCollider(0.5f, this);
            this.MaxLives = startingLives;
            this.Lives = startingLives;
        }

        /// <summary>
        /// updates the player based on keyboard input
        /// </summary>
        /// <param name="gameManager">The owning game manager</param>
        public void HandleInput(RobotDodge3D gameManager)
        {
            //get the keyboard state for the current frame
            KeyboardState keyboardState = gameManager.KeyboardState;

            //fire rocket on space pressed
            if (keyboardState.IsKeyPressed(Keys.Space))
            {
                gameManager.CreateBullet(this.Transform.Position, this.Transform.Forward);
            }

            //dont update facing direnction when no input
            if (!(keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.S)))
            {
                return;
            }

            //calculate input diection based on keys pressed
            Vector3 direction = new Vector3();

            if (keyboardState.IsKeyDown(Keys.A))
            {
                direction.X -= 1;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                direction.X += 1;
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                direction.Z -= 1;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                direction.Z += 1;
            }

            //move and rotate based on input
            this.Transform.Position += direction * this.Speed * gameManager.DeltaTime;
            float rot = (float)MathHelper.Atan2(direction.X, direction.Z);
            this.Transform.Rotation = new Vector3(0f, MathHelper.RadiansToDegrees(rot), 0f);
        }

        /// <summary>
        /// Update the player score to the total elapsed game time
        /// </summary>
        /// <param name="deltaTime">The time between frames</param>
        public void UpdateScore(float deltaTime)
        {
            this.Score += deltaTime;
        }

        /// <summary>
        /// Reduces the player lives and sets gameover if less than or equal to zero
        /// </summary>
        /// <param name="gameManager">the owning gameManager</param>
        /// <param name="damage">The amount of lives to reduce (default = 1)</param>
        public void TakeDamage(RobotDodge3D gameManager, int damage = 1)
        {
            this.Lives -= damage;

            if (this.Lives <= 0)
            {
                gameManager.GameOver = true;
            }
        }
    }
}