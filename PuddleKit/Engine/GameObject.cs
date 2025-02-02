using PuddleKit.Engine.Colliders;
using PuddleKit.Renderer;
using PuddleKit.Renderer.Meshes;

namespace PuddleKit.Engine
{
    /// <summary>
    /// A gameobject class for reprenting 3D objects withing the game
    /// </summary>
    public class GameObject
    {
        /// <summary>
        /// The Gameobject Transform for representing the location, orientation, and scale
        /// </summary>
        public Transform Transform { get; private set; }

        /// <summary>
        /// Optional collider for checking collisions between gamobjects
        /// </summary>
        public Collider? Collider { get; protected set; }

        /// <summary>
        /// optional 3D Mesh
        /// </summary>
        protected Mesh? Mesh { get; private set; }

        /// <summary>
        /// optional Texture for use with the mesh 
        /// </summary>
        protected Texture? Texture { get; private set; }

        /// <summary>
        /// Initializes a new empty instance of the GameObject class.
        /// </summary>
        public GameObject()
        {
            this.Transform = new Transform();
        }

        /// <summary>
        /// Initializes a new instance of the GameObject class.
        /// With a Mesh and Texture to be drawn
        /// </summary>
        /// <param name="mesh">The mesh to be drawn</param>
        /// <param name="texture">The texture to be applied to the mesh</param>
        public GameObject(Mesh mesh, Texture texture)
        {
            this.Mesh = mesh;
            this.Texture = texture;
            this.Transform = new Transform();
        }

        /// <summary>
        /// Updates the logic of the gameobject should be called once per frame
        /// </summary>
        /// <param name="gameManager">The parent game manager object, should be the calling object</param>
        public virtual void Update(GameManager gameManager) { }

        /// <summary>
        /// Draws the mesh to the screen with the applied texture if availiable
        /// </summary>
        /// <param name="camera">The viewProjection camera</param>
        public virtual void Draw(Camera camera)
        {
            this.Texture?.Bind();
            this.Mesh?.Draw(this.Transform, camera);
        }
    }
}