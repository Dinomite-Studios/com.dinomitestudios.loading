using System.Threading.Tasks;

namespace Dinomite.Loading.Fade
{
    /// <summary>
    /// Interface for a <see cref="ISceneService"/> provider implementing scene
    /// transition animation.
    /// </summary>
    public interface ICameraFadeModule : ILoadingServiceModule
    {
        /// <summary>
        /// Is the camera currently faded out?
        /// </summary>
        bool IsFadedOut { get; }

        /// <summary>
        /// Fades the camera out.
        /// </summary>
        Task FadeOutAsync();

        /// <summary>
        /// Fades the camera in.
        /// </summary>
        Task FadeInAsync();
    }
}