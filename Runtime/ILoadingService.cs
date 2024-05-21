using RealityCollective.ServiceFramework.Interfaces;
using System.Threading.Tasks;

namespace Dinomite.Loading
{
    public delegate void OnLoadingStarted(bool isIndeterminate);
    public delegate void OnLoadingProgressChanged(float progress);
    public delegate void OnLoadingHintChanged(string loadingHint);
    public delegate void OnLoadingEnded();

    /// <summary>
    /// A service responsible for putting the application into loading state and displaying
    /// a loading screen to the player, while other processes and services are performing loading operationrs.
    /// This service itself is not responsible for managing loading processes.
    /// </summary>
    public interface ILoadingService : IService
    {
        /// <summary>
        /// Is there an active loading operation?
        /// </summary>
        bool IsLoadingActive { get; }

        /// <summary>
        /// The loading hint is s player facing message that describes what is going on.
        /// </summary>
        string LoadingHint { get; set; }

        /// <summary>
        /// Is the duration of the current loading operation indeterminate?
        /// </summary>
        bool IsIndeterminate { get; }

        /// <summary>
        /// The progress of the current loading session in range [0, 1], inclusive.
        /// </summary>
        float Progress { get; set; }

        /// <summary>
        /// A loading operation has started.
        /// </summary>
        event OnLoadingStarted LoadingStarted;

        /// <summary>
        /// The overall progress has changed. This event will
        /// not fire for <see cref="IsIndeterminate"/> loading.
        /// </summary>
        event OnLoadingProgressChanged LoadingProgressChanged;

        /// <summary>
        /// The <see cref="LoadingHint"/> has changed.
        /// </summary>
        event OnLoadingHintChanged LoadingHintChanged;

        /// <summary>
        /// The loading operation has ended.
        /// </summary>
        event OnLoadingEnded LoadingEnded;

        /// <summary>
        /// Enters the loading state.
        /// </summary>
        /// <param name="fadeCamera">
        /// If set, the player camera will fade out for the loading operation.
        /// Use this e.g. for a smooth transition in between scenes.
        /// </param>
        /// <param name="isIndeterminate">Is the duration of the loading operation indeterminate?</param>
        Task StartLoadingAsync(bool fadeCamera, bool isIndeterminate);

        /// <summary>
        /// Ends the loading state.
        /// </summary>
        Task EndLoadingAsync();
    }
}