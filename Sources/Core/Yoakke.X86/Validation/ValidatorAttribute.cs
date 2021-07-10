// Copyright (c) 2021 Yoakke.
// Licensed under the Apache License, Version 2.0.
// Source repository: https://github.com/LanguageDev/Yoakke

using System;

namespace Yoakke.X86.Validation
{
    /// <summary>
    /// An attribute to attach a validator to an element requiring semantic validation.
    /// For example this could be an <see cref="IInstructionValidator"/> on an <see cref="IInstruction"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class ValidatorAttribute : Attribute
    {
        /// <summary>
        /// The <see cref="Type"/> of the validator that implements <see cref="IInstructionValidator"/>.
        /// </summary>
        public Type ValidatorType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatorAttribute"/> class.
        /// </summary>
        /// <param name="validatorType">The <see cref="Type"/> of the validator.</param>
        public ValidatorAttribute(Type validatorType)
        {
            this.ValidatorType = validatorType;
        }
    }
}
