using DG.Tweening;
using RealityCollective.ServiceFramework.Modules;
using RealityCollective.Utilities.Extensions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dinomite.Loading.Fade
{
    /// <summary>
    /// Default implementation for <see cref="ICameraFadeModule"/>.
    /// </summary>
    [System.Runtime.InteropServices.Guid("d46b788d-ed6c-4f68-9b15-500c4e8304c7")]
    public class CameraFadeModule : BaseServiceModule, ICameraFadeModule
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name assigned to this service instance.</param>
        /// <param name="priority">The priority assigned to this service instance.</param>
        /// <param name="profile">The configuration profile for this service instance.</param>
        /// <param name="parentService">The parent service instance for this service intance.</param>
        public CameraFadeModule(string name, uint priority, CameraFadeModuleProfile profile, ILoadingService parentService)
                : base(name, priority, profile, parentService)
        {
            fadeOnStart = profile.FadeOnStart;
            universalRendererData = profile.UniversalRendererData;
            fadeDuration = profile.FadeDuration;
        }

        private readonly bool fadeOnStart;
        private readonly float fadeDuration;
        private readonly UniversalRendererData universalRendererData;
        private Material fadeMaterial;

        /// <inheritdoc/>
        public bool IsFadedOut { get; private set; }

        /// <inheritdoc/>
        public override void Initialize()
        {
            base.Initialize();

            if (!Application.isPlaying)
            {
                return;
            }

            var feature = universalRendererData.rendererFeatures.Find(feature => feature is CameraFadeFeature);
            if (feature.IsNull() || feature is not CameraFadeFeature cameraFadeFeature)
            {
                Debug.LogError($"{nameof(CameraFadeFeature)} not configured on the active {nameof(UniversalRendererData)} asset.");
                return;
            }

            fadeMaterial = Object.Instantiate(cameraFadeFeature.Settings.Material);
            cameraFadeFeature.Settings.RuntimeMaterial = fadeMaterial;
        }

        /// <inheritdoc/>
        public override void Start()
        {
            if (fadeOnStart)
            {
                _ = FadeInAsync();
            }
        }

        /// <inheritdoc/>
        public async Task FadeOutAsync()
        {
            IsFadedOut = true;

            var done = false;
            var sequence = DOTween.Sequence();

            var material = fadeMaterial;
            sequence.Append(material.DOFloat(1f, CameraFadePass.FadePropertyId, fadeDuration));
            sequence.AppendCallback(() =>
            {
                done = true;
            });

            while (!done)
            {
                await Task.Yield();
            }
        }

        /// <inheritdoc/>
        public async Task FadeInAsync()
        {
            var done = false;
            var sequence = DOTween.Sequence();

            var material = fadeMaterial;
            sequence.Append(material.DOFloat(0f, CameraFadePass.FadePropertyId, fadeDuration));
            sequence.AppendCallback(() =>
            {
                done = true;
            });

            while (!done)
            {
                await Task.Yield();
            }

            IsFadedOut = false;
        }
    }
}