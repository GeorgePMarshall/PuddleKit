using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace PuddleKit.Renderer
{
    /// <summary>
    /// A class for compilling and manipulating shader programs
    /// </summary>
    public class ShaderProgram : IDisposable
    {
        private readonly int _glShaderHandle;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the ShaderProgram class.
        /// <para></para>Loads and compiles a shaderprogram with vertex and fragment shaders
        /// <para></para>Requires Opengl context to be initialized prior to calling
        /// </summary>
        /// <param name="vertexSourcePath">The path to the vertex shader starts in "Renderer/Shaders/"</param>
        /// <param name="fragmentSourcePath">The path to the fragment shader starts in "Renderer/Shaders/"</param>
        public ShaderProgram(string vertexSourcePath, string fragmentSourcePath)
        {
            //read shader source code from files
            string vertexShaderSource = File.ReadAllText("Renderer/Shaders/" + vertexSourcePath);
            string fragmentShaderSource = File.ReadAllText("Renderer/Shaders/" + fragmentSourcePath);

            //Create and complile vertex shader
            int vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderSource);
            GL.CompileShader(vertexShaderHandle);
            GL.GetShader(vertexShaderHandle, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                Console.WriteLine(GL.GetShaderInfoLog(vertexShaderHandle));
            }

            //Create and complile fragment shader
            int fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderSource);
            GL.CompileShader(fragmentShaderHandle);
            GL.GetShader(fragmentShaderHandle, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                Console.WriteLine(GL.GetShaderInfoLog(fragmentShaderHandle));
            }

            //create and link shader program
            _glShaderHandle = GL.CreateProgram();
            GL.AttachShader(_glShaderHandle, vertexShaderHandle);
            GL.AttachShader(_glShaderHandle, fragmentShaderHandle);
            GL.LinkProgram(_glShaderHandle);
            GL.GetProgram(_glShaderHandle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                Console.WriteLine(GL.GetProgramInfoLog(_glShaderHandle));
            }

            //cleanup fragment and vertex now that they are not needed 
            GL.DetachShader(_glShaderHandle, vertexShaderHandle);
            GL.DetachShader(_glShaderHandle, fragmentShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);
            GL.DeleteShader(vertexShaderHandle);
        }

        /// <summary>
        /// Checks if shader program has been destroyed properly before destroing owning class
        /// </summary>
        ~ShaderProgram()
        {
            if (!_isDisposed)
            {
                Console.WriteLine("ERROR: Shader not properly disposed, GPU RESCOURCE LEAK");
            }
        }

        /// <summary>
        /// unloads the shader prodram from the gpu
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                GL.DeleteProgram(_glShaderHandle);
                _isDisposed = true;
            }
        }

        /// <summary>
        /// binds the shader program to be sued for subsequent draw calls
        /// </summary>
        public void Use()
        {
            if (_isDisposed)
            {
                Console.WriteLine("ERROR: Trying to use disposed shader");
                return;
            }
            GL.UseProgram(_glShaderHandle);
        }

        /// <summary>
        /// Gets the location offset of a named attribute in the shader program
        /// </summary>
        /// <param name="attributeName">The name of the attribute</param>
        /// <returns>Returns a Attribute location handle</returns>
        public int GetAttribLocation(string attributeName)
        {
            this.Use();
            return GL.GetAttribLocation(_glShaderHandle, attributeName);
        }

        /// <summary>
        /// Sets a uniform Matrix4 inside the shader program
        /// </summary>
        /// <param name="matrix">The matrix to set</param>
        /// <param name="name">The name of the Uniform</param>
        public void SetMatrix4(Matrix4 matrix, string name)
        {
            this.Use();
            int uniformLocationHandle = GL.GetUniformLocation(_glShaderHandle, name);
            GL.UniformMatrix4(uniformLocationHandle, true, ref matrix);
        }

        /// <summary>
        /// Sets a uniform Vector4 inside the shader program
        /// </summary>
        /// <param name="vector4">The vector4 to set</param>
        /// <param name="name">The name of the Uniform</param>
        public void SetVector4(Vector4 vector4, string name)
        {
            this.Use();
            int uniformLocationHandle = GL.GetUniformLocation(_glShaderHandle, name);
            GL.Uniform4(uniformLocationHandle, vector4.X, vector4.Y, vector4.Z, vector4.W);
        }

        /// <summary>
        /// Sets the object transform matrix4 inside the shader program
        /// </summary>
        /// <param name="matrix">The matrix4 to set</param>
        public void SetObjectTransform(Matrix4 matrix)
        {
            this.SetMatrix4(matrix, "objectTransform");
        }

        /// <summary>
        /// Sets the viewProjection transform matrix4 inside the shader program
        /// </summary>
        /// <param name="viewProjectionTransform">The viewProjectionTransform to set</param>
        public void SetViewProjectionTransform(Matrix4 viewProjectionTransform)
        {
            this.SetMatrix4(viewProjectionTransform, "viewProjectionTransform");
        }

        /// <summary>
        /// Sets the viewProjection transform matrix4 inside the shader program
        /// </summary>
        /// <param name="camera">The camer with the viewProjectionTransform to set</param>
        public void SetCameraTransform(Camera camera)
        {
            this.SetViewProjectionTransform(camera.GetViewProjectionTransform());
        }
    }
}