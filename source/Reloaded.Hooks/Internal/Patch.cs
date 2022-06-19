﻿using System;
using Reloaded.Memory.Sources;
using static Reloaded.Memory.Sources.Memory;

namespace Reloaded.Hooks.Internal
{
    /// <summary>
    /// A simple structure that defines an address and the bytes that should be written to the address.
    /// </summary>
    public struct Patch
    {
        private nuint _address;
        private byte[] _bytes;

        public Patch(nuint address, byte[] bytes)
        {
            _address = address;
            _bytes = bytes;
        }

        /// <summary>
        /// Applies the patch. (Writes bytes to address).
        /// </summary>
        public void Apply()
        {
            CurrentProcess.SafeWriteRaw(_address, _bytes);
        }

        /// <summary>
        /// Applies the patch without changing permissions.
        /// </summary>
        public unsafe void ApplyUnsafe()
        {
            var addressSpan = new Span<byte>((void*) _address, _bytes.Length);
            var bytesSpan   = _bytes.AsSpan();
            bytesSpan.CopyTo(addressSpan);
        }
    }
}
