using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PuddleKit.Renderer.Meshes
{
    /// <summary>
    /// A 2D Mesh of a rectangle for use in 3D space
    /// </summary>
    public class RectangleMesh : Mesh
    {
        private readonly int _indicesLength;

        /// <summary>
        /// Initializes a new instance of the RectangleMesh class.
        /// <para></para>Requires Opengl context to be initialized prior to calling
        /// </summary>
        /// <param name="shader">The shader program to be used by this mesh when drawing</param>
        /// <param name="textureCoordModifier">A mulplicative multiplier for the texture coords used for tileing the texture</param>
        public RectangleMesh(ShaderProgram shader, float textureCoordModifier = 1.0f) : base(shader)
        {
            //an array of verticies and texture coordinates representing a rectangle
            float[] vertices = [
                //Position          Texture coordinates
                0.5f,  0.5f, 0.0f, 1.0f * textureCoordModifier, 1.0f * textureCoordModifier, // top right
                0.5f, -0.5f, 0.0f, 1.0f * textureCoordModifier, 0.0f * textureCoordModifier, // bottom right
               -0.5f, -0.5f, 0.0f, 0.0f * textureCoordModifier, 0.0f * textureCoordModifier, // bottom left
               -0.5f,  0.5f, 0.0f, 0.0f * textureCoordModifier, 1.0f * textureCoordModifier // top left
            ];

            //indicies representing the order to draw the verticies in order to triangulate teh rectangle
            uint[] indices = [
                0, 1, 3,   // first triangle
                1, 2, 3    // second triangle
            ];

            _indicesLength = indices.Length;

            //Create vertex array on gpu
            _glVertexArrayObjectHandle = GL.GenVertexArray();
            GL.BindVertexArray(_glVertexArrayObjectHandle);

            //create VBO and copy in verticies array
            _glVertexBufferObjectHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVertexBufferObjectHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            //create EBO and copy in indicies array
            _glElementBufferObjectHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _glElementBufferObjectHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

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
        /// Draws the rectangle with the passed transform
        /// </summary>
        /// <param name="transform">The object transformation for controlling the mesh in world space</param>
        /// <param name="camera">The viewProjection camera for converting world coordinates into clip space</param>
        public override void Draw(Engine.Transform transform, Camera camera)
        {
            base.Draw(transform, camera);

            GL.BindVertexArray(_glVertexArrayObjectHandle);
            GL.DrawElements(PrimitiveType.Triangles, _indicesLength, DrawElementsType.UnsignedInt, 0);
        }

        /// <summary>
        /// Draws the rectangle with the passed transform
        /// </summary>
        /// <param name="transform">The object transformation for controlling the mesh in world space</param>
        /// <param name="viewProjectionTransform">The viewProjection matrix for converting world coordinates into clip space</param>
        public override void Draw(Engine.Transform transform, Matrix4 viewProjectionTransform)
        {
            base.Draw(transform, viewProjectionTransform);

            GL.BindVertexArray(_glVertexArrayObjectHandle);
            GL.DrawElements(PrimitiveType.Triangles, _indicesLength, DrawElementsType.UnsignedInt, 0);
        }
    }
}