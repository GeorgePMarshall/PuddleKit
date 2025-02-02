using PuddleKit;
using PuddleKit.Renderer;
using PuddleKit.Renderer.Meshes;
using OpenTK.Mathematics;
using PuddleKit.Engine;
using PuddleKit.Engine.Utilities;

namespace RobotDodge3D
{
    /// <summary>
    /// The main game manager of Robot Dodge 3D
    /// Responsible for main game logic and drawing
    /// </summary>
    public class RobotDodge3D : GameManager
    {
        //NOTE: due to the execution order any object that require the opengl context at creation time cannot be init using a field initializer and must be init in the constructor
        //this is because the base constructor must be called to initialize the opengl context.
        //Field Initializer -> Base Constructor -> local constructor
        private readonly ShaderProgram _defaultShader;
        private readonly ShaderProgram _diffuseShader;
        private readonly Camera _camera;
        private readonly Player _player;
        private readonly GUI _gui;
        private readonly Floor _floor;
        private readonly Rectangle _playAreaBounds = new Rectangle(-7f, 6f, -7f, 1f);

        //mesh and textures for objects that are repeatedly created are loaded in the game manager
        //and shared references passsed to the objects to avoid repeated loading
        private readonly ModelMesh _robot1Mesh;
        private readonly ModelMesh _robot2Mesh;
        private readonly ModelMesh _robot3Mesh;
        private readonly ModelMesh _bulletMesh;
        private readonly Texture _robot1Texture;
        private readonly Texture _robot2Texture;
        private readonly Texture _robot3Texture;
        private readonly Texture _bulletTexture;

        //lists for agregating all robots and bullets that need to be updated and drawn
        private readonly List<Robot> _robots = new List<Robot>();
        private readonly List<Bullet> _bullets = new List<Bullet>();

        /// <summary>
        /// Bool representing the current gamover state
        /// </summary>
        public bool GameOver { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the RobotDodge3D class.
        /// Base condtructor initializes opengl context
        /// </summary>
        /// <param name="width">The width of the window</param>
        /// <param name="height">The height of the game window</param>
        /// <param name="title">The title of the game window</param>
        public RobotDodge3D(int width, int height, string title) : base(width, height, title)
        {
            _defaultShader = new ShaderProgram("default.vert", "default.frag");
            _diffuseShader = new ShaderProgram("diffuse.vert", "diffuse.frag");

            _camera = new Camera(width, height);
            _camera.Transform.Position = new Vector3(0f, 5f, 0f);
            _camera.Transform.Rotation -= new Vector3(65f, 0f, 0f);

            //load robot mesh and texture to avid re loading 
            _robot1Mesh = new ModelMesh(_diffuseShader, "Robot1/Robot1.obj");
            _robot1Texture = new Texture("Robot1/Robot1.png");
            _robot2Mesh = new ModelMesh(_diffuseShader, "Robot2/Robot2.obj");
            _robot2Texture = new Texture("Robot2/Robot2.png");
            _robot3Mesh = new ModelMesh(_diffuseShader, "Robot3/Robot3.obj");
            _robot3Texture = new Texture("Robot3/Robot3.png");
            _bulletMesh = new ModelMesh(_diffuseShader, "Bullet/Bullet.obj");
            _bulletTexture = new Texture("Bullet/Bullet.png");

            //player init
            _player = new Player(_diffuseShader);
            _player.Transform.Scale = new Vector3(0.4f);

            _gui = new GUI(_player, width, height);

            //floor uses default shader because rectangle normals are not calculated for diffuse 
            _floor = new Floor(_defaultShader);
        }

        /// <summary>
        /// Updates the game logic
        /// Called every frame
        /// </summary>
        public override void Update()
        {
            //stop updating when gameover
            if (this.GameOver)
            {
                return;
            }

            //spawn random robot at random intervals
            if (Random.Shared.Next(1000) == 0)
            {
                switch (Random.Shared.Next(3))
                {
                    case 0:
                        _robots.Add(new Robot(_robot1Mesh, _robot1Texture, _player, _playAreaBounds));
                        break;
                    case 1:
                        _robots.Add(new Robot(_robot2Mesh, _robot2Texture, _player, _playAreaBounds));
                        break;
                    case 2:
                        _robots.Add(new Robot(_robot3Mesh, _robot3Texture, _player, _playAreaBounds));
                        break;
                    default:
                        break;
                }
            }

            _player.HandleInput(this);
            _player.UpdateScore(this.DeltaTime);

            foreach (Robot robot in _robots.ToList())
            {
                robot.Update(this);
            }

            foreach (Bullet bullet in _bullets)
            {
                bullet.Update(this);
            }

            this.ResolveCollisions();
        }

        /// <summary>
        /// Draws all game objects
        /// </summary>
        public override void Draw()
        {
            _player.Draw(_camera);
            _floor.Draw(_camera);

            foreach (Robot robot in _robots)
            {
                robot.Draw(_camera);
            }

            foreach (Bullet bullet in _bullets)
            {
                bullet.Draw(_camera);
            }

            //gui shoudl be drawn last for proper transparency calculation
            _gui.Draw(this);
        }

        /// <summary>
        /// Creates a new bullet and adds it to be updateed and drawn
        /// </summary>
        /// <param name="originPosition">The starting position of the bullet</param>
        /// <param name="direction">The direction the bullet should travel</param>
        public void CreateBullet(Vector3 originPosition, Vector3 direction)
        {
            _bullets.Add(new Bullet(_bulletMesh, _bulletTexture, originPosition, direction));
        }

        /// <summary>
        /// Checks for any overlaping collisions by objects being managed by the game manager, then destroyes them
        /// </summary>
        private void ResolveCollisions()
        {
            foreach (Robot robot in _robots.ToList())
            {
                //player and robot collisions
                if (_player.Collider != null && robot.Collider != null && _player.Collider.CollidesWith(robot.Collider))
                {
                    _player.TakeDamage(this);
                    _robots.Remove(robot);
                }
                else if (!this.GameObjectWithinPlayArea(robot))
                {
                    _robots.Remove(robot);
                }

                //bullet and robot collisions
                foreach (Bullet bullet in _bullets.ToList())
                {
                    if (robot.Collider != null && bullet.Collider != null && bullet.Collider.CollidesWith(robot.Collider))
                    {
                        _robots.Remove(robot);
                        _bullets.Remove(bullet);
                    }
                    else if (!this.GameObjectWithinPlayArea(bullet))
                    {
                        _bullets.Remove(bullet);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a gameobject is within the defined play area
        /// </summary>
        /// <param name="gameObject">The gameobject to check</param>
        /// <returns>Returns true if gameobject is within the play area otherwise false</returns>
        public bool GameObjectWithinPlayArea(GameObject gameObject)
        {
            return !(gameObject.Transform.Position.X < _playAreaBounds.left || gameObject.Transform.Position.Z < _playAreaBounds.top || gameObject.Transform.Position.X > _playAreaBounds.right || gameObject.Transform.Position.Z > _playAreaBounds.bottom);
        }
    }
}