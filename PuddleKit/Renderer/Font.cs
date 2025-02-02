using SharpFont;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace PuddleKit.Renderer
{
    /// <summary>
    /// A class for loading a font and using it to draw text
    /// </summary>
    public class Font
    {
        /// <summary>
        /// A struct for extracted character information
        /// </summary>
        private struct Character
        {
            public int glTextureHandle;
            public Vector2i size;
            public Vector2i bearing;
            public uint advance;
        }

        private readonly int _glVertexArrayObjectHandle;
        private readonly int _glVertexBufferObjectHandle;
        private readonly Dictionary<char, Character> _characters = new Dictionary<char, Character>();

        /// <summary>
        /// Initializes a new instance of the Font class.
        /// <para></para>Loads a ttf from file and creates gpu buffers
        /// <para></para>Requires Opengl context to be initialized prior to calling
        /// </summary>
        /// <param name="filePath">The file path of the front ttf file starts in "Resources/"</param>
        public Font(string filePath)
        {
            //load character information from file and extract the character textures
            this.GenerateCharacterTextures("Resources/" + filePath);

            //Create vertex array on gpu
            _glVertexArrayObjectHandle = GL.GenVertexArray();
            GL.BindVertexArray(_glVertexArrayObjectHandle);

            //create empty VBO verticies will be copied in on a per character basis
            _glVertexBufferObjectHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _glVertexBufferObjectHandle);
            GL.BufferData(BufferTarget.ArrayBuffer, 6 * 4 * sizeof(float), (nint)null, BufferUsageHint.DynamicDraw);

            //set vertex attribute offsets for vertex positions and texcoods
            //vertex attribute is a single vec4 and will be split on the gpu
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            //unbind
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Loads the font from file then extract the textures and character data for the first 128 ascii characters
        /// </summary>
        /// <param name="filePath">The file path of the front ttf file starts in root directory</param>
        private void GenerateCharacterTextures(string filePath)
        {
            //load font into library
            Library library = new Library();
            Face face = new Face(library, filePath);
            face.SetPixelSizes(0, 48);

            //change texture allignment to suppor single bit width textures
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            //load first 128 characters in font
            for (char character = (char)0; character < 128; character++)
            {
                face.LoadChar(character, LoadFlags.Render, LoadTarget.Normal);

                //create texture on gpu 
                int newGlTextureHandle = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, newGlTextureHandle);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.R8, face.Glyph.Bitmap.Width, face.Glyph.Bitmap.Rows, 0, PixelFormat.Red, PixelType.UnsignedByte, face.Glyph.Bitmap.Buffer);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

                //add character data to character store
                _characters.Add(character, new Character
                {
                    glTextureHandle = newGlTextureHandle,
                    size = new Vector2i(face.Glyph.Bitmap.Width, face.Glyph.Bitmap.Rows),
                    bearing = new Vector2i(face.Glyph.BitmapLeft, face.Glyph.BitmapTop),
                    advance = (uint)face.Glyph.Advance.X.Value
                });
            }

            face.Dispose();
            library.Dispose();
        }

        /// <summary>
        /// Draws text to screen using loaded font
        /// </summary>
        /// <param name="text">The text to draw</param>
        /// <param name="shaderProgram">The text shader to draw with</param>
        /// <param name="position">The screen space position to draw the text</param>
        /// <param name="viewProjection">The orthographic screen space view projection transformation</param>
        /// <param name="scale">A mulpicative scaler (default to 1)</param>
        /// <param name="colour">The colour of the text (defaults to white)</param>
        public void DrawText(string text, ShaderProgram shaderProgram, Vector2 position, Matrix4 viewProjection, float scale = 1, Vector4? colour = null)
        {
            //check for default colour
            Vector4 textColour = colour ?? Vector4.One;

            //bind and update uniforms in shaderprogram
            shaderProgram.Use();
            shaderProgram.SetVector4(textColour, "textColour");
            shaderProgram.SetViewProjectionTransform(viewProjection);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(_glVertexArrayObjectHandle);

            //creates a rectangle for each character we want to draw
            //and draw it with it respective texture
            foreach (char textCharacter in text)
            {
                Character character = _characters[textCharacter];

                //move cursor forward the bearing of the character
                float xPos = position.X + (character.bearing.X * scale);

                //characters that render below the line (g) must be moved down
                float yPos = position.Y - ((character.size.Y - character.bearing.Y) * scale);

                float width = character.size.X * scale;
                float height = character.size.Y * scale;

                //create rectangle that will fit the character
                float[,] verticies = new float[,]
                {
                    {xPos, yPos + height, 0f, 0f},
                    {xPos, yPos, 0f, 1f},
                    {xPos + width, yPos, 1f, 1f},

                    {xPos, yPos + height, 0f, 0f},
                    {xPos + width, yPos, 1f, 1f},
                    {xPos + width, yPos + height, 1f, 0f},
                };

                //upload vertex data and draw character
                GL.BindTexture(TextureTarget.Texture2D, character.glTextureHandle);
                GL.BindBuffer(BufferTarget.ArrayBuffer, _glVertexBufferObjectHandle);
                GL.BufferSubData(BufferTarget.ArrayBuffer, 0, 6 * 4 * sizeof(float), verticies);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

                //move cursor forward the width of the character
                //NOTE: character advance is stored as a fixed point type representing 1/64th of a pixel
                //bitshift >> 6 gives us the value in pixels
                position.X += (character.advance >> 6) * scale;
            }
        }
    }
}