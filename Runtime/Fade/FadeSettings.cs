using RealityCollective.Utilities.Extensions;
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Dinomite.Loading.Fade
{
    [Serializable]
    public class FadeSettings
    {
        public bool IsEnabled = true;
        public string ProfilerTag = "Camera Fade";
        public RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
        public Material Material;

        public Material RuntimeMaterial { get; set; }

        public bool AreValid => RuntimeMaterial.IsNotNull() && IsEnabled;
    }
}
