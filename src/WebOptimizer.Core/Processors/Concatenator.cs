using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebOptimizer;

namespace WebOptimizer
{
    internal class Concatenator : Processor
    {
        public override Task ExecuteAsync(IAssetContext context)
        {
            var content = ByteExtensions.Join(context.Content.Values);
            context.Content = new Dictionary<string, byte[]>
            {
                { Guid.NewGuid().ToString(), content}
            };

            return Task.CompletedTask;
        }
    }

}
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="IAssetPipeline"/>.
    /// </summary>
    public static partial class AssetPipelineExtensions
    {
        /// <summary>
        /// Adds the string content of all source files to the pipeline.
        /// </summary>
        public static IAsset Concatenate(this IAsset asset)
        {
            var reader = new Concatenator();
            asset.Processors.Add(reader);

            return asset;
        }

        /// <summary>
        /// Adds the string content of all source files to the pipeline.
        /// </summary>
        public static IEnumerable<IAsset> Concatenate(this IEnumerable<IAsset> assets)
        {
            return assets.AddProcessor(asset => asset.Concatenate());
        }
    }
}
