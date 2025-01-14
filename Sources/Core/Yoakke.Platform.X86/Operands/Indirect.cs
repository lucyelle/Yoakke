// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

namespace Yoakke.Platform.X86.Operands
{
    /// <summary>
    /// An indirect memory access.
    /// </summary>
    public readonly struct Indirect : IOperand
    {
        /// <inheritdoc/>
        public bool IsMemory => true;

        /// <inheritdoc/>
        public DataWidth? GetSize() => this.Size;

        /// <inheritdoc/>
        public DataWidth GetSize(AssemblyContext context) => this.Size;

        /// <summary>
        /// The width of the accessed data.
        /// </summary>
        public readonly DataWidth Size;

        /// <summary>
        /// The <see cref="Operands.Address"/> to read from.
        /// </summary>
        public readonly Address Address;

        /// <summary>
        /// Initializes a new instance of the <see cref="Indirect"/> struct.
        /// </summary>
        /// <param name="size">The width - or size - of the accessed data.</param>
        /// <param name="address">The <see cref="Operands.Address"/> to read from.</param>
        public Indirect(DataWidth size, Address address)
        {
            this.Size = size;
            this.Address = address;
        }
    }
}
