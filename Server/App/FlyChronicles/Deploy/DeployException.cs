using System;
using System.Runtime.Serialization;

namespace FlyCronicles.DeployerService
{
    /// <summary>
    /// wrapper for deploy exceptions
    /// </summary>
    [Serializable]
    public class DeployException : Exception
    {
        /// <summary>
        /// default ctor
        /// </summary>
        public DeployException()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        public DeployException(string message) : base(message)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DeployException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DeployException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
