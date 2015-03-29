// ---------------------------------------------------------------
// <autor>Pablo González</autor>
// <web>www.odra.com.ar</web>
// <blog>blog.odra.com.ar</blog>
// <email>pablo.gonzalez@odra.com.ar</email>
// <copyrigth>http://www.odra.com.ar/MIT.txt</copyrigth>
// ---------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Unity.InterceptionExtension.Serialization
{
    /// <summary>
    /// Exception for errors in this assembly.
    /// </summary>
    [Serializable]
    public sealed class SerializableInterceptorException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SerializableInterceptorException()
        {
        }

        public SerializableInterceptorException(string message)
            : base(message)
        {
        }

        public SerializableInterceptorException(string format, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, format, args))
        {
        }

        public SerializableInterceptorException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public SerializableInterceptorException(string format, Exception inner, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, format, args), inner)
        {
        }

        private SerializableInterceptorException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
