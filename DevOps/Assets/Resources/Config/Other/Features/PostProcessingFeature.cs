using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessingFeature : ScriptableRendererFeature
{
    class CustomRenderPass : ScriptableRenderPass
    {
        public RenderTargetIdentifier Source;

        private readonly List<Material> _materials;
        private readonly RTHandle _temporaryRenderTarget;

        public CustomRenderPass(List<Material> material)
        {
            _materials = material;
            _temporaryRenderTarget = RTHandles.Alloc("_TemporaryColorTexture");
        }
        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("PostProcessFeature");
            foreach (var material in _materials)
            {
                commandBuffer.GetTemporaryRT(Shader.PropertyToID(_temporaryRenderTarget.name), renderingData.cameraData.cameraTargetDescriptor);
                commandBuffer.Blit( Source,_temporaryRenderTarget.nameID, material);
                commandBuffer.Blit( _temporaryRenderTarget.nameID, Source);
                

            }
            
            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
        }
    }
    [System.Serializable]
    public class Settings
    {
        public List<Material> material;
    }

    public Settings settings = new Settings();
    
    CustomRenderPass _mScriptablePass;
    
    /// <inheritdoc/>
    public override void Create()
    {
        _mScriptablePass = new CustomRenderPass(settings.material)
        {
            // Configures where the render pass should be injected.
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing
        };
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        _mScriptablePass.Source = renderer.cameraColorTargetHandle.nameID;
        renderer.EnqueuePass(_mScriptablePass);
    }
}


