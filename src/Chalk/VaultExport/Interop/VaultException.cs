using System;
using System.Runtime.Serialization;

namespace Chalk.VaultExport.Interop
{
    [Serializable]
    public class VaultException : Exception
    { 
        public VaultException(string message) : base(message)
        {
        }

        // ReSharper disable once UnusedMember.Global
        public VaultException(string message, Exception inner) : base(message, inner)
        {
        }

        protected VaultException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
