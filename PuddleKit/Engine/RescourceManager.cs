using PuddleKit.Renderer.Meshes;
using PuddleKit.Renderer;


namespace PuddleKit.Engine
{
    /// <summary>
    /// A class for managing all object loaded from file
    /// </summary>
    public class ResourceManager
    {
        private readonly Dictionary<string, ModelMesh> _loadedModelMeshes = new Dictionary<string, ModelMesh>();

        /// <summary>
        /// Returns a previously loaded mesh. If mesh has not been previously loaded it will be loaded from file
        /// </summary>
        /// <param name="modelMeshShaderprogram">The shader program to render the mesh with</param>
        /// <param name="path">The path of the file</param>
        /// <returns>Returns a mesh object created from the specified path</returns>
        public ModelMesh LoadOrGetModelMesh(ShaderProgram modelMeshShaderprogram, string path)
        {
            if (_loadedModelMeshes.TryGetValue(path, out ModelMesh? mesh))
            {
                return mesh;
            }
            else
            {
                mesh = new ModelMesh(modelMeshShaderprogram, path);
                _loadedModelMeshes.Add(path, mesh);
                return mesh;
            }
        }
    }
}