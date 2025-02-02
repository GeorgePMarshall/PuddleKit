using OpenTK.Graphics.OpenGL4;

namespace PuddleKit.Renderer.Meshes
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelMesh : Mesh
    {
        private readonly int _indicesLength;

        /// <summary>
        /// Initializes a new instance of the ModelMesh class.
        /// <para></para>Requires Opengl context to be initialized prior to calling
        /// <para></para>Loads a mesh from file only supports 1 mesh per file (child mesh wont be loaded)
        /// </summary>
        /// <param name="shader">the shader program to be used by this mesh when drawing</param>
        /// <param name="filePath">The file path of the mesh to load begins in "Resources/"</param>
        public ModelMesh(ShaderProgram shader, string filePath) : base(shader)
        {
            //use assim to load and process mesh into usable format
            Assimp.AssimpContext importer = new Assimp.AssimpContext();
            Assimp.Scene modelScene = importer.ImportFile("Resources/" + filePath, Assimp.PostProcessPreset.TargetRealTimeMaximumQuality);

            //get first mesh in file
            Assimp.Mesh mesh = modelScene.Meshes[0];

            //assimp will process all faces into triangle so 3 verts per face
            _indicesLength = mesh.FaceCount * 3;

            List<float> vertices = new List<float>(mesh.VertexCount);
            List<int> indices = new List<int>(_indicesLength);

            //extract verticies data
            for (int i = 0; i < mesh.VertexCount; i++)
            {
                vertices.Add(mesh.Vertices[i].X);
                vertices.Add(mesh.Vertices[i].Y);
                vertices.Add(mesh.Vertices[i].Z);

                vertices.Add(mesh.Normals[i].X);
                vertices.Add(mesh.Normals[i].Y);
                vertices.Add(mesh.Normals[i].Z);

                vertices.Add(mesh.TextureCoordinateChannels[0][i].X);
                vertices.Add(mesh.TextureCoordinateChannels[0][i].Y);
            }

            //extract indicies data
            foreach (Assimp.Face face in mesh.Faces)
            {
                foreach (int index in face.Indices)
                {
                    indices.Add(index);
                }
            }

            //Create vertex array on gpu
            _glVertexArrayObjectHandle = GL.GenVertexArray();
            GL.BindVertexArray(_glVertexArrayObjectHandle);

            //create VBO and copy in verticies array
            _glVertexBufferObjectHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVertexBufferObjectHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * sizeof(float), vertices.ToArray(), BufferUsageHint.StaticDraw);

            //create EBO and copy in indicies array
            _glElementBufferObjectHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _glElementBufferObjectHandle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(int), indices.ToArray(), BufferUsageHint.StaticDraw);

            //set vertex attribute offsets for vertex positions
            int positionAttributeLocationHandle = shaderProgram.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(positionAttributeLocationHandle);
            GL.VertexAttribPointer(positionAttributeLocationHandle, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            //set vertex attribute offsets for vertex normals
            int normalAttributeLocationHandle = shaderProgram.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalAttributeLocationHandle);
            GL.VertexAttribPointer(normalAttributeLocationHandle, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));

            //set vertex attribute offsets for vertex texture coords
            int texCoordAttributeLocationHandle = shaderProgram.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordAttributeLocationHandle);
            GL.VertexAttribPointer(texCoordAttributeLocationHandle, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            //unbind
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Draws the model with the passed transform
        /// </summary>
        /// <param name="transform">The object transformation for controlling the mesh in world space</param>
        /// <param name="camera">The viewProjection camera for converting world coordinates into clip space</param>
        public override void Draw(Engine.Transform transform, Camera camera)
        {
            base.Draw(transform, camera);

            GL.BindVertexArray(_glVertexArrayObjectHandle);
            GL.DrawElements(PrimitiveType.Triangles, _indicesLength, DrawElementsType.UnsignedInt, 0);
        }
    }
}