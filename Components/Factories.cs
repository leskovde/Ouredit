using System;
using System.Collections.Generic;
using System.Text;
using Components.Models;
using Buffer = Components.Models.Buffer;

namespace Components
{
    [Leskovar]
    internal abstract class BufferFactory
    {
        public abstract Buffer Create(File file);
    }

    [Leskovar]
    internal class ImmediateBufferFactory : BufferFactory
    {
        public override Buffer Create(File file)
        {
            return new ImmediateBuffer(file);
        }
    }

    [Leskovar]
    internal class LazyBufferFactory : BufferFactory
    {
        public override Buffer Create(File file)
        {
            return new LazyBuffer(file);
        }
    }

    /// <summary>
    /// A container holding a table of BufferFactories.
    /// </summary>
    [Leskovar]
    internal static class BufferInstantiator
    {
        private static readonly Dictionary<BufferType, BufferFactory> Factories = new Dictionary<BufferType, BufferFactory>
            {
                {BufferType.Immediate, new ImmediateBufferFactory()},
                {BufferType.Lazy, new LazyBufferFactory()},
            };

        /// <summary>
        /// Returns an instance of a Buffer based on a given buffer type.
        /// </summary>
        /// <param name="bufferType">The type of the instantiated buffer.</param>
        /// <param name="file">The file over which the buffer operates.</param>
        /// <returns></returns>
        public static Buffer GetBuffer(BufferType bufferType, File file)
        {
            return Factories[bufferType].Create(file);
        }
    }
}
