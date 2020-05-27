// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace Azure.Management.Resources.Models
{
    /// <summary> The environment variable to pass to the script in the container instance. </summary>
    public partial class EnvironmentVariable
    {
        /// <summary> Initializes a new instance of EnvironmentVariable. </summary>
        /// <param name="name"> The name of the environment variable. </param>
        public EnvironmentVariable(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        /// <summary> Initializes a new instance of EnvironmentVariable. </summary>
        /// <param name="name"> The name of the environment variable. </param>
        /// <param name="value"> The value of the environment variable. </param>
        /// <param name="secureValue"> The value of the secure environment variable. </param>
        internal EnvironmentVariable(string name, string value, string secureValue)
        {
            Name = name;
            Value = value;
            SecureValue = secureValue;
        }

        /// <summary> The name of the environment variable. </summary>
        public string Name { get; set; }
        /// <summary> The value of the environment variable. </summary>
        public string Value { get; set; }
        /// <summary> The value of the secure environment variable. </summary>
        public string SecureValue { get; set; }
    }
}