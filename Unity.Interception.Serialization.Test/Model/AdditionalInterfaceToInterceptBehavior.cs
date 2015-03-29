using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Unity.InterceptionExtension.Serialization.Test.Model
{
    [Serializable]
    public class AdditionalInterfaceToInterceptBehavior : IInterceptionBehavior
    {
        private string _additionalstate;
        private string _proxystate;

        #region IInterceptionBehavior Members

        /// <summary>
        /// Implement this method to execute your behavior processing.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <param name="getNext">Delegate to execute to get the next delegate in the behavior chain.</param>
        /// <returns>Return value from the target.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if (input.MethodBase.Name == "IsIntercept")
            {
                return input.CreateMethodReturn(true);
            }

            if (input.MethodBase.IsSpecialName && input.MethodBase.Name == "get_PropertyBase")
            {
                return input.CreateMethodReturn(_proxystate);
            }

            if (input.MethodBase.IsSpecialName && input.MethodBase.Name == "set_PropertyBase")
            {
                _proxystate = input.Inputs[0].ToString();

                return input.CreateMethodReturn(null);
            }

            if (input.MethodBase.IsSpecialName && input.MethodBase.Name == "get_AdditionalProperty")
            {
                return input.CreateMethodReturn(_additionalstate);
            }

            if (input.MethodBase.IsSpecialName && input.MethodBase.Name == "set_AdditionalProperty")
            {
                _additionalstate = input.Inputs[0].ToString();
                return input.CreateMethodReturn(null);
            }

            if (input.MethodBase.Name == "CanInterceptThis")
            {
                return input.CreateMethodReturn(true);
            }

            return getNext().Invoke(input, getNext);
        }

        /// <summary>
        /// Returns the interfaces required by the behavior for the objects it intercepts.
        /// </summary>
        /// <returns>The required interfaces.</returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return new[] { typeof(IAdditionalInterfaceToIntercept) };
        }

        /// <summary>
        /// Returns a flag indicating if this behavior will actually do anything when invoked.
        /// </summary>
        /// <remarks>This is used to optimize interception. If the behaviors won't actually
        /// do anything (for example, PIAB where no policies match) then the interception
        /// mechanism can be skipped completely.</remarks>
        public bool WillExecute
        {
            get { return true; }
        }

        #endregion
    }
}