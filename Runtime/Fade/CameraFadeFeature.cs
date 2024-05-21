using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dinomite.Loading.Fade
{
    public class CameraFadeFeature : ScriptableRendererFeature
    {
        [SerializeField]
        private FadeSettings settings;

        private CameraFadePass fadeRenderPass;

        public FadeSettings Settings => settings;

        public override void Create()
        {
            fadeRenderPass = new CameraFadePass(settings);
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (!settings.AreValid)
            {
                return;
            }

            renderer.EnqueuePass(fadeRenderPass);
        }
    }
}
