using RealityCollective.ServiceFramework.Definitions;
using UnityEngine;

namespace Dinomite.Loading
{
    /// <summary>
    /// Configuration profile for the <see cref="ILoadingService"/>.
    /// </summary>
    public class LoadingServiceProfile : BaseServiceProfile<ILoadingServiceModule>
    {
        /// <summary>
        /// A general use loading scene used whenever a loading task needs to be completed
        /// before the player can continue.
        /// </summary>
        [field: SerializeField, Tooltip("The generic loading scene.")]
        public string LoadingSceneName { get; private set; }

        /// <summary>
        /// The loading <see cref="CameraClearFlags"/> applied to the camera while loading.
        /// </summary>
        [field: SerializeField, Tooltip("The loading clear flags applied to the camera while loading.")]
        public CameraClearFlags LoadingClearFlags { get; private set; } = CameraClearFlags.SolidColor;

        /// <summary>
        /// The loading background color applied to the camera while loading.
        /// </summary>
        [field: SerializeField, Tooltip("The loading background color applied to the camera while loading.")]
        public Color LoadingBackgroundColor { get; private set; } = Color.black;

        /// <summary>
        /// The loading culling mask to apply to the main camera during the loading scene appearance.
        /// </summary>
        [field: SerializeField, Tooltip("The loading culling mask to apply to the main camera.")]
        public LayerMask LoadingCullingMask { get; private set; } = -1;
    }
}
