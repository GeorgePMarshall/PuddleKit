namespace PuddleKit.Engine.Utilities
{
    /// <summary>
    /// A struct for representing a rectangle with 4 sides
    /// </summary>
    public struct Rectangle
    {
        /// <summary>
        /// left side of rectangle
        /// </summary>
        public float left;

        /// <summary>
        /// right side of rectangle
        /// </summary>
        public float right;

        /// <summary>
        /// top side of rectangle
        /// </summary>
        public float top;

        /// <summary>
        /// bottom side of rectangle
        /// </summary>
        public float bottom;

        /// <summary>
        /// Initializes a new instance of the Rectangle struct.
        /// </summary>
        /// <param name="left">left side of rectangle</param>
        /// <param name="right">right side of rectangle</param>
        /// <param name="top">top side of rectangle</param>
        /// <param name="bottom">bottom side of rectangle</param>
        public Rectangle(float left, float right, float top, float bottom)
        {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }
    }
}