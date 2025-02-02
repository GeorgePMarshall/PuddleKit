using PuddleKit.Engine;
using PuddleKit.Renderer;
using PuddleKit.Renderer.Meshes;
using OpenTK.Mathematics;

namespace RobotDodge3D
{
    /// <summary>
    /// A class for the floor object
    /// Rnders a rectangle at 0,0,0 with a tilled texture 
    /// </summary>
    public class Floor : GameObject
    {
        /// <summary>
        /// Initializes a new instance of the Floor class.
        /// </summary>
        /// <param name="shaderProgram">The shader program to rander the floor with</param>
        public Floor(ShaderProgram shaderProgram) : base(new RectangleMesh(shaderProgram, 4f), new Texture("floor.jpg"))
        {
            this.Transform.Rotation += new Vector3(90f, 0f, 0f);
            this.Transform.Scale = new Vector3(20f, 20f, 20f);
        }
    }
}