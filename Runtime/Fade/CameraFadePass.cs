using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Dinomite.Loading.Fade
{
    public class CameraFadePass : ScriptableRenderPass
    {
        public CameraFadePass(FadeSettings settings)
        {
            this.settings = settings;
            renderPassEvent = settings.RenderPassEvent;
            FadePropertyId = Shader.PropertyToID("_Alpha");
        }

        private readonly FadeSettings settings;

        public static int FadePropertyId { get; private set; }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var command = CommandBufferPool.Get(settings.ProfilerTag);
            var source = BuiltinRenderTextureType.CameraTarget;
            var destination = BuiltinRenderTextureType.CurrentActive;

            command.Blit(source, destination, settings.RuntimeMaterial);
            context.ExecuteCommandBuffer(command);

            CommandBufferPool.Release(command);
        }
    }
}
