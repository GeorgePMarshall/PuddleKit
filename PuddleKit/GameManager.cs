using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace PuddleKit
{
    /// <summary>
    /// A parent class for managing all game logic
    /// <para></para>This class should be inherited from in the game application
    /// <para></para>The following functions can be overided to implement game logic
    /// <para></para>public virtual void Start() { }
    /// <para></para>public virtual void Update() { }
    /// <para></para>public virtual void Draw() { }
    /// <para></para>public virtual void Shutdown() { }
    /// </summary>
    public abstract class GameManager : GameWindow
    {
        /// <summary>
        /// The time betwwen frames in secconds
        /// </summary>
        public float DeltaTime { get; private set; }

        /// <summary>
        /// Initializes a new instance of the GameManager class.
        /// Sets up the game window and opengl context
        /// </summary>
        /// <param name="width">The width of the game window</param>
        /// <param name="height">The height of the game window</param>
        /// <param name="title">The window title</param>
        public GameManager(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { ClientSize = (width, height), Title = title }) { }

        /// <summary>
        /// runs at the start of the program
        /// </summary>
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            this.Start();
        }

        /// <summary>
        /// uns when the window is ready to update
        /// </summary>
        /// <param name="args">The frame event args used for event timer</param>
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            this.DeltaTime = (float)args.Time;
            this.Update();
        }

        /// <summary>
        /// Runs when the window is ready to draw
        /// </summary>
        /// <param name="args">The frame event args used for event timer</param>
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            //clear window
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //draw user program
            this.Draw();

            //diplay buffer to screen
            this.SwapBuffers();
        }

        /// <summary>
        /// Called when window size changes, used to updatea viewport size
        /// </summary>
        /// <param name="e">The event arguments</param>
        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
        {
            base.OnFramebufferResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }

        /// <summary>
        /// runs before the window closes
        /// </summary>
        protected override void OnUnload()
        {
            base.OnUnload();
            this.Shutdown();
        }

        /// <summary>
        /// runs at the start of the program
        /// </summary>
        public virtual void Start() { }

        /// <summary>
        /// For updataing game logic
        /// Runs once every frame
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// For drawing to the window
        /// Runs once every frame
        /// </summary>
        public virtual void Draw() { }

        /// <summary>
        /// Runs at the end of the program
        /// </summary>
        public virtual void Shutdown() { }
    }
}