using OpenTK.Graphics.OpenGL4;
using PuddleKit.Engine;

namespace PuddleKit.Renderer.Meshes
{
    /// <summary>
    /// A 3D mesh of a Cube
    /// </summary>
    public class CubeMesh : Mesh
    {
        /// <summary>
        /// Initializes a new instance of the CubeMesh class.
        /// <para></para>Requires Opengl context to be initialized prior to calling
        /// </summary>
        /// <param name="shader">the shader program to be used by this mesh when drawing</param>
        public CubeMesh(ShaderProgram shader) : base(shader)
        {
            //an array of verticies and texture coordinates representing a cube
            //verticies were taken from:
            //https://learnopengl.com/code_viewer.php?code=getting-started/cube_vertices
            float[] vertices = [
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,
                0.5f, -0.5f, -0.5f,  1.0f, 0.0f,
                0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 0.0f,

                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                0.5f,  0.5f,  0.5f,  1.0f, 1.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,

                -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                0.5f,  0.5f,  0.5f,  1.0f, 0.0f,

                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,
                0.5f, -0.5f, -0.5f,  1.0f, 1.0f,
                0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                0.5f, -0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f, -0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f, -0.5f, -0.5f,  0.0f, 1.0f,

                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f,
                0.5f,  0.5f, -0.5f,  1.0f, 1.0f,
                0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                0.5f,  0.5f,  0.5f,  1.0f, 0.0f,
                -0.5f,  0.5f,  0.5f,  0.0f, 0.0f,
                -0.5f,  0.5f, -0.5f,  0.0f, 1.0f
            ];

            //Create vertex array on gpu
            _glVertexArrayObjectHandle = GL.GenVertexArray();
            GL.BindVertexArray(_glVertexArrayObjectHandle);

            //create VBO and copy in verticies array
            _glVertexBufferObjectHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVertexBufferObjectHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //set vertex attribute offsets for vertex positions
            int positionAttributeLocationHandle = shaderProgram.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(positionAttributeLocationHandle);
            GL.VertexAttribPointer(positionAttributeLocationHandle, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            //set vertex attribute offsets for vertex texture coords
            int texCoordAttributeLocationHandle = shaderProgram.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordAttributeLocationHandle);
            GL.VertexAttribPointer(texCoordAttributeLocationHandle, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

            //unbind VAO and VBO
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Draws the cube with the passed transform
        /// </summary>
        /// <param name="transform">The object transformation for controlling the mesh in world space</param>
        /// <param name="camera">The viewProjection camera for converting world coordinates into clip space</param>
        public override void Draw(Transform transform, Camera camera)
        {
            base.Draw(transform, camera);

            GL.BindVertexArray(_glVertexArrayObjectHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
    }
}