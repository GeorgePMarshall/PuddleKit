namespace RobotDodge3D
{
    /// <summary>
    /// The main class for the program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point of the program
        /// </summary>
        public static void Main()
        {
            RobotDodge3D game = new RobotDodge3D(1600, 900, "Robot Dodge 3D");
            game.Run();
        }
    }
}
