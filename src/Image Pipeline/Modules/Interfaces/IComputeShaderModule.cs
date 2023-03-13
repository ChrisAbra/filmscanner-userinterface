using System;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

namespace Scanner.ImagePipeline {
    public interface IComputeShaderModule : IPipelineModule<PipelineImage,PipelineImage> {}
}
