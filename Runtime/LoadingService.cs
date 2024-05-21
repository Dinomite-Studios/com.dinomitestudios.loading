using Dinomite.Loading.Fade;
using RealityCollective.ServiceFramework.Services;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dinomite.Loading
{
    /// <summary>
    /// Default implementation for <see cref="ILoadingService"/>.
    /// </summary>
    [System.Runtime.InteropServices.Guid("858cdaae-6cdb-4249-9017-1ed76ffb68c5")]
    public class LoadingService : BaseServiceWithConstructor, ILoadingService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name assigned to this service instance.</param>
        /// <param name="priority">The priority assigned to this service instance.</param>
        /// <param name="profile">The configuration profile for this service instance.</param>
        public LoadingService(string name, uint priority, LoadingServiceProfile profile)
            : base(name, priority)
        {
            loadingSceneName = profile.LoadingSceneName;
            loadingClearFlags = profile.LoadingClearFlags;
            loadingBackgroundColor = profile.LoadingBackgroundColor;
            loadingCullingMask = profile.LoadingCullingMask;
        }

        private readonly CameraClearFlags loadingClearFlags;
        private readonly Color loadingBackgroundColor;
        private readonly LayerMask loadingCullingMask;
        private readonly string loadingSceneName;
        private bool didFadeCamera;
        private CameraClearFlags previousCameraClearFlags;
        private Color previousCameraBackgroundColor;
        private LayerMask previousCullingMask;

        /// <inheritdoc/>
        public bool IsLoadingActive { get; private set; }

        private string loadingHint;
        /// <inheritdoc/>
        public string LoadingHint
        {
            get => loadingHint;
            set
            {
                if (string.Equals(loadingHint, value))
                {
                    return;
                }

                loadingHint = value;
                LoadingHintChanged?.Invoke(loadingHint);
            }
        }

        /// <inheritdoc/>
        public bool IsIndeterminate { get; private set; }

        private float progress;
        /// <inheritdoc/>
        public float Progress
        {
            get => progress;
            set
            {
                if (Mathf.Approximately(value, progress))
                {
                    return;
                }

                progress = value;
                LoadingProgressChanged?.Invoke(progress);
            }
        }

        /// <inheritdoc/>
        public event OnLoadingStarted LoadingStarted;

        /// <inheritdoc/>
        public event OnLoadingProgressChanged LoadingProgressChanged;

        /// <inheritdoc/>
        public event OnLoadingHintChanged LoadingHintChanged;

        /// <inheritdoc/>
        public event OnLoadingEnded LoadingEnded;

        /// <inheritdoc/>
        public async Task StartLoadingAsync(bool fadeCamera, bool isIndeterminate)
        {
            if (IsLoadingActive)
            {
                return;
            }

            didFadeCamera = fadeCamera;
            var cameraFadeProvider = ServiceManager.Instance.GetService<ICameraFadeModule>();

            if (fadeCamera)
            {
                await cameraFadeProvider.FadeOutAsync();
            }

            previousCullingMask = Camera.main.cullingMask;
            previousCameraBackgroundColor = Camera.main.backgroundColor;
            previousCameraClearFlags = Camera.main.clearFlags;

            Camera.main.cullingMask = loadingCullingMask;
            Camera.main.clearFlags = loadingClearFlags;
            Camera.main.backgroundColor = loadingBackgroundColor;
            await SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);

            IsIndeterminate = isIndeterminate;
            IsLoadingActive = true;
            LoadingStarted?.Invoke(IsIndeterminate);

            if (fadeCamera)
            {
                await cameraFadeProvider.FadeInAsync();
            }
        }

        /// <inheritdoc/>
        public async Task EndLoadingAsync()
        {
            if (!IsLoadingActive)
            {
                return;
            }

            var cameraFadeProvider = ServiceManager.Instance.GetService<ICameraFadeModule>();

            if (didFadeCamera)
            {
                await cameraFadeProvider.FadeOutAsync();
            }

            await SceneManager.UnloadSceneAsync(loadingSceneName);
            Camera.main.cullingMask = previousCullingMask;
            Camera.main.clearFlags = previousCameraClearFlags;
            Camera.main.backgroundColor = previousCameraBackgroundColor;

            IsLoadingActive = false;
            LoadingEnded?.Invoke();

            if (didFadeCamera)
            {
                await cameraFadeProvider.FadeInAsync();
            }
        }
    }
}
