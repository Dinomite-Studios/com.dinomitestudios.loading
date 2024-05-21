using RealityCollective.ServiceFramework.Definitions;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dinomite.Loading.Fade
{
    /// <summary>
    /// Configuration profile for <see cref="CameraFadeModule"/>.
    /// </summary>
    public class CameraFadeModuleProfile : BaseProfile
    {
        [field: SerializeField, Tooltip("URP renderer definition.")]
        public UniversalRendererData UniversalRendererData { get; private set; } = null;

        [field: SerializeField, Tooltip("If set, the camera will fade in right on start.")]
        public bool FadeOnStart { get; private set; } = true;

        [field: SerializeField, Tooltip("Total duration to fade in / out.")]
        public float FadeDuration { get; private set; } = 2f;
    }
}