using OpenTK.Mathematics;


namespace PuddleKit.Engine
{
    /// <summary>
    /// A class for represnting 3D transformations
    /// </summary>
    public class Transform
    {
        private Matrix4 _cachedTransformMatrix;
        private bool _transformNeedsRecalcuation = true;

        /// <summary>
        /// The 3D position of the Transform
        /// </summary>
        public Vector3 Position { get; set { field = value; _transformNeedsRecalcuation = true; } }

        /// <summary>
        /// The rotation of the object in Euler andgles.
        /// Rotation will be applied in the following order X -> Y -> Z
        /// </summary>
        public Vector3 Rotation { get; set { field = value; _transformNeedsRecalcuation = true; } }

        /// <summary>
        /// The mulplicative scale along each axis
        /// </summary>
        public Vector3 Scale { get; set { field = value; _transformNeedsRecalcuation = true; } } = Vector3.One;

        /// <summary>
        /// A unit vector representing the forward direction of the transform
        /// </summary>
        public Vector3 Forward => this.TransformMatrix.Transposed().Column2.Normalized().Xyz;

        /// <summary>
        /// A unit vector representing the rightwards direction of the transform
        /// </summary>
        public Vector3 Right => -this.TransformMatrix.Transposed().Column0.Normalized().Xyz;

        /// <summary>
        /// A unit vector representing the Upwards direction of the transform
        /// </summary>
        public Vector3 Up => this.TransformMatrix.Transposed().Column1.Normalized().Xyz;

        /// <summary>
        /// A matrix represnting all transformations
        /// <para>Cached copy will be returned if tranform has mot been modifed</para>
        /// otherwise it will be recalculated
        /// </summary>
        public Matrix4 TransformMatrix
        {
            get
            {
                if (_transformNeedsRecalcuation)
                {
                    Matrix4 translationMatrix = Matrix4.CreateTranslation(this.Position);
                    Vector3 radianRotation = new Vector3(MathHelper.DegreesToRadians(this.Rotation.X), MathHelper.DegreesToRadians(this.Rotation.Y), MathHelper.DegreesToRadians(this.Rotation.Z));
                    Matrix4 rotationMatrix = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(radianRotation));
                    Matrix4 scaleMatrix = Matrix4.CreateScale(this.Scale);
                    _cachedTransformMatrix = scaleMatrix * rotationMatrix * translationMatrix;
                    _transformNeedsRecalcuation = false;
                }
                return _cachedTransformMatrix;
            }
        }
    }
}