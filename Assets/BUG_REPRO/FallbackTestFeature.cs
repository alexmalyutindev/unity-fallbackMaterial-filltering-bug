using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FallbackTestFeature : ScriptableRendererFeature
{
    class FallbackTestPass : ScriptableRenderPass
    {
        private FilteringSettings _filteringSettings;
        private ShaderTagId _fallbackTestId = new ShaderTagId("FallbackTest");
        private Material _fallbackMaterial;

        public FallbackTestPass()
        {
            profilingSampler = new ProfilingSampler(nameof(FallbackTestPass));
            _filteringSettings = FilteringSettings.defaultValue;
            _filteringSettings.renderingLayerMask = 1 << 20;
        }

        public void Setup(Material fallbackMaterial)
        {
            _fallbackMaterial = fallbackMaterial;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData) { }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get();

            using (new ProfilingScope(cmd, profilingSampler))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                var drawingSettings = CreateDrawingSettings(
                    _fallbackTestId, // <- Materials without "FallbackTest" pass don't get into render, till manually moved in scene!
                    ref renderingData,
                    SortingCriteria.CommonOpaque
                );

                drawingSettings.fallbackMaterial = _fallbackMaterial; // Doesn't work, till manually moved in scene!
                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref _filteringSettings);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
        }

        public override void OnCameraCleanup(CommandBuffer cmd) { }
    }


    public Material FallbackMaterial;
    private FallbackTestPass _pass;

    /// <inheritdoc/>
    public override void Create()
    {
        _pass = new FallbackTestPass()
        {
            renderPassEvent = RenderPassEvent.AfterRenderingOpaques
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (FallbackMaterial == null)
            return;

        _pass.Setup(FallbackMaterial);
        renderer.EnqueuePass(_pass);
    }
}