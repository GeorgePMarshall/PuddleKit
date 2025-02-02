using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace PuddleKit.Renderer
{
    /// <summary>
    /// A class to load and manipulate a texture on the gpu
    /// </summary>
    public class Texture
    {
        private readonly int _glTextureHandle;

        /// <summary>
        /// Initializes a new instance of the Camera class.
        /// <para></para>Requires Opengl context to be initialized prior to calling
        /// </summary>
        /// <param name="filePath">The file path of the image to load begins in "Resources/"</param>
        public Texture(string filePath)
        {
            //generate texture handle
            _glTextureHandle = GL.GenTexture();
            this.Bind();

            //needed beacuse of Opentks flipped transformations
            StbImage.stbi_set_flip_vertically_on_load(1);

            //load from file
            ImageResult image = ImageResult.FromStream(File.OpenRead("Resources/" + filePath), ColorComponents.RedGreenBlueAlpha);

            //upload to gpu
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            //set parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }

        /// <summary>
        /// Binds the texture to texture unit 0
        /// </summary>
        public void Bind()
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, _glTextureHandle);
        }
    }
}