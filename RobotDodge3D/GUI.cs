using PuddleKit.Renderer;
using OpenTK.Mathematics;
using PuddleKit.Engine;
using PuddleKit.Renderer.Meshes;

namespace RobotDodge3D
{
    /// <summary>
    /// A class resposible for renering UI elements
    /// uiTransform is set such that 0,0 is in the botton left
    /// </summary>
    public class GUI
    {
        private readonly ShaderProgram _uiShader = new ShaderProgram("ScreenSpace.vert", "ScreenSpace.frag");
        private readonly ShaderProgram _textShader = new ShaderProgram("Text.vert", "Text.frag");
        private readonly Matrix4 _uiProjection;
        private readonly Texture _heartTexture = new Texture("redHeart.png");
        private readonly Texture _greyHeartTexture = new Texture("greyHeart.png");
        private readonly RectangleMesh _heartMesh;
        private readonly Transform _heartTransform = new Transform();
        private readonly Font _font = new Font("arial.ttf");
        private readonly Player _player;

        /// <summary>
        /// Initializes a new instance of the GUI class.\
        /// </summary>
        /// <param name="player">A refernce to the for for drawing health and score</param>
        /// <param name="width">The width of the window</param>
        /// <param name="height">The height of the window</param>
        public GUI(Player player, float width, float height)
        {
            //fixed view orthographic projection is used so ui is always drawn ontop and irrespective of z depth 
            _uiProjection = Matrix4.CreateOrthographicOffCenter(0, width, 0, height, 0.1f, 10f) * Matrix4.CreateTranslation(0f, 0f, 1f);
            _heartMesh = new RectangleMesh(_uiShader);
            _heartTransform.Scale = new Vector3(50f);
            _player = player;
        }

        /// <summary>
        /// Draws the UI to the screen
        /// Should be drawn last to properly draw transparent elements 
        /// </summary>
        /// <param name="gameManager">the parent gameManager</param>
        public void Draw(RobotDodge3D gameManager)
        {
            this.DrawLives(_player);
            this.DrawScore(_player);

            if (gameManager.GameOver)
            {
                _font.DrawText($"- Game Over -", _textShader, new Vector2(660, 400), _uiProjection, 1, new Vector4(0f, 0f, 0f, 1f));
            }
        }

        /// <summary>
        /// Draws the players lives as red heart images
        /// </summary>
        /// <param name="player">The players whose lives is to be drawn</param>
        private void DrawLives(Player player)
        {
            //for every possible life
            for (int i = 0; i < player.MaxLives; i++)
            {
                //bind red texture if life remains
                //Bind grey texture if life is lost
                if (i < player.Lives)
                {
                    _heartTexture.Bind();
                }
                else
                {
                    _greyHeartTexture.Bind();
                }
                _heartTransform.Position = new Vector3((_heartTransform.Scale.X * i * 1.1f) + 100, 850f, 0f);
                _heartMesh.Draw(_heartTransform, _uiProjection);
            }
        }

        /// <summary>
        /// Draw player score to the screeen
        /// </summary>
        /// <param name="player">The players whose score is to be drawn</param>
        private void DrawScore(Player player)
        {
            _font.DrawText($"Score {(int)player.Score}", _textShader, new Vector2(1300f, 820f), _uiProjection);
        }
    }

}