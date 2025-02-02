using OpenTK.Mathematics;
using PuddleKit.Engine;

namespace PuddleKit.Renderer.Meshes
{
    /// <summary>
    /// A class representing a 3D mesh comprised of verticies stored on the gpu
    /// </summary>
    public abstract class Mesh
    {
        /// <summary>
        /// Handle for the opengl VAO from the graphics driver
        /// </summary>
        protected int _glVertexArrayObjectHandle;

        /// <summary>
        /// Handle for the opengl VBO from the graphics driver
        /// </summary>
        protected int _glVertexBufferObjectHandle;

        /// <summary>
        /// Handle for the opengl EBO from the graphics driver
        /// </summary>
        protected int _glElementBufferObjectHandle;

        /// <summary>
        /// A reference to the shader program to be used by this mesh when drawing
        /// </summary>
        protected ShaderProgram shaderProgram;

        /// <summary>
        /// Initializes a new instance of the Mesh class.
        /// </summary>
        /// <param name="shader">the shader program to be used by this mesh when drawing</param>
        public Mesh(ShaderProgram shader)
        {
            shaderProgram = shader;
        }

        /// <summary>
        /// Writes transform information to shaderprogram to prepare to draw mesh
        /// </summary>
        /// <param name="transform">The object transformation for controlling the mesh in world space</param>
        /// <param name="camera">The viewProjection camera for converting world coordinates into clip space</param>
        public virtual void Draw(Transform transform, Camera camera)
        {
            shaderProgram.Use();
            shaderProgram.SetObjectTransform(transform.TransformMatrix);
            shaderProgram.SetCameraTransform(camera);
        }

        /// <summary>
        /// Writes transform information to shaderprogram to prepare to draw mesh
        /// </summary>
        /// <param name="transform">The object transformation for controlling the mesh in world space</param>
        /// <param name="viewProjectionTransform">The viewProjection transform for converting world coordinates into clip space</param>
        public virtual void Draw(Transform transform, Matrix4 viewProjectionTransform)
        {
            shaderProgram.Use();
            shaderProgram.SetObjectTransform(transform.TransformMatrix);
            shaderProgram.SetViewProjectionTransform(viewProjectionTransform);
        }
    }
}