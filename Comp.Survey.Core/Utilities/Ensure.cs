using JetBrains.Annotations;
using System;
using System.Diagnostics;

namespace Comp.Survey.Core.Utilities
{
    public static class Ensure
    {
        /// <summary>
        /// Ensures that the specified parameter is not null.
        /// </summary>
        /// <param name="argument">The parameter.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        [DebuggerStepThrough]
        [ContractAnnotation("halt <= argument:null")]
        public static void ArgumentNotNull(object argument, [InvokerParameterName] string parameterName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }

        /// <summary>
        /// Ensures that the specified Guid is not empty.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="parameterName">Name of the id parameter.</param>
        public static void GuidNotEmpty(Guid id, [InvokerParameterName] string parameterName)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
