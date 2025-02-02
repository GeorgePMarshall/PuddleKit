using OpenTK.Mathematics;
using PuddleKit.Engine;

namespace PuddleKit.Renderer
{
    /// <summary>
    /// A class for representing a view projection transformation to convert world space to clip space
    /// </summary>
    public class Camera
    {
        private Matrix4 _viewTransform;
        private Matrix4 _projectionTransform;
        private Matrix4 _viewProjectionTransform;

        private readonly float _viewWidth;
        private readonly float _viewHeight;

        /// <summary>
        /// The cameras world space transformaion
        /// </summary>
        public Transform Transform { get; private set; }

        /// <summary>
        /// Initializes a new instance of the Camera class.
        /// <para></para>Uses perspective projection
        /// </summary>
        /// <param name="viewWidth">The width of the viewport in pixels</param>
        /// <param name="viewHeight">The height of the view port in pixels</param>
        /// <param name="fov">The Field of view of the viewport in degrees</param>
        public Camera(int viewWidth, int viewHeight, float fov = 60f)
        {
            _viewWidth = viewWidth;
            _viewHeight = viewHeight;
            this.Transform = new Transform();
            _projectionTransform = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(fov), _viewWidth / _viewHeight, 0.1f, 100f);
        }

        /// <summary>
        /// Calculates the view projection transform
        /// </summary>
        /// <returns>Returns the current viewProjection transformation as a matrix4</returns>
        public Matrix4 GetViewProjectionTransform()
        {
            //NOTE: openTK uses ROW MAJOR matricies all transformations must be applied in opposite order
            _viewTransform = this.Transform.TransformMatrix.Inverted();
            _viewProjectionTransform = _viewTransform * _projectionTransform;
            return _viewProjectionTransform;
        }

    }
}