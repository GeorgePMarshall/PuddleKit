using OpenTK.Graphics.OpenGL4;

namespace PuddleKit.Renderer.Meshes
{
    /// <summary>
    /// A 2D Mesh of a triangle for use in 3D space
    /// </summary>
    public class TriangleMesh : Mesh
    {
        /// <summary>
        /// Initializes a new instance of the TriangleMesh class.
        /// <para></para>Requires Opengl context to be initialized prior to calling
        /// </summary>
        /// <param name="shader">the shader program to be used by this mesh when drawing</param>
        public TriangleMesh(ShaderProgram shader) : base(shader)
        {
            //an array of verticies representing a single triangle
            float[] vertices =
            [
                -0.5f, -0.5f, 0.0f, //bottom left
                0.5f, -0.5f, 0.0f, //bottom right
                0.0f,  0.5f, 0.0f  //top
            ];

            //Create vertex array on gpu
            _glVertexArrayObjectHandle = GL.GenVertexArray();
            GL.BindVertexArray(_glVertexArrayObjectHandle);

            //create VBO and copy in verticies array
            _glVertexBufferObjectHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVertexBufferObjectHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //set vertex attribute offsets for vertex positions
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            //unbind VAO and VBO
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Draws the triangle with the passed transform
        /// </summary>
        /// <param name="transform">The object transformation for controlling the mesh in world space</param>
        /// <param name="camera">The viewProjection camera for converting world coordinates into clip space</param>
        public override void Draw(Engine.Transform transform, Camera camera)
        {
            base.Draw(transform, camera);

            GL.BindVertexArray(_glVertexArrayObjectHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        }
    }
}