using System;
using System.Diagnostics;
using System.Reflection;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace IMS.NCS.CourseSearchService.Common.Unity
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that runs any exceptions returned from the
    /// target through the Exception Handling Application Block.
    /// </summary>
    /// <remarks>This fixes the problem with the Ent Lib version of the class.
    /// </remarks>
    [ConfigurationElementType(typeof(ExceptionCallHandlerData))]
    public class ExceptionCallHandler : ICallHandler
    {
        private string _exceptionPolicy;
        private int _order = 0;
        private ExceptionManager _exManager;

        /// <summary>
        /// Creates a new <see cref="ExceptionCallHandler"/> that processses exceptions
        /// using the given exception policy.
        /// </summary>
        /// <param name="exceptionPolicy">Exception policy.</param>
        public ExceptionCallHandler(string exceptionPolicy)
        {
            _exceptionPolicy = exceptionPolicy;
            _exManager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
        }

        /// <summary>
        /// Creates a new <see cref="ExceptionCallHandler"/> that processses exceptions
        /// using the given exception policy.
        /// </summary>
        /// <param name="exceptionPolicy">Exception policy.</param>
        /// <param name="order">Order in which the handler will be executed.</param>
        public ExceptionCallHandler(string exceptionPolicy, int order)
        : this(exceptionPolicy)
        {
            _order = order;
        }

        /// <summary>
        /// Gets the exception policy used by this handler.
        /// </summary>
        /// <value>Exception policy.</value>
        public string ExceptionPolicy
        {
            get
            {
                return _exceptionPolicy;
            }
        }

        #region ICallHandler Members
        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
            }
        }

        /// <summary>
        /// Processes the method call.
        /// </summary>
        /// <remarks>This handler does nothing before the call. If there is an exception
        /// returned, it runs the exception through the Exception Handling Application Block.</remarks>
        /// <param name="input"><see cref="IMethodInvocation"/> with information about the call.</param>
        /// <param name="getNext">delegate to call to get the next handler in the pipeline.</param>
        /// <returns>Return value from the target, or the (possibly changed) exceptions.</returns>
		[DebuggerHidden]		
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (input == null) throw new ArgumentNullException("input");
            if (getNext == null) throw new ArgumentNullException("getNext");

            IMethodReturn result = getNext()(input, getNext);
            if (result.Exception != null)
            {
                try
                {
                    bool rethrow = _exManager.HandleException(result.Exception, _exceptionPolicy);
                    if (!rethrow)
                    {
                        // Exception is being swallowed
                        result.ReturnValue = null;
                        result.Exception = null;

                        if (input.MethodBase.MemberType == MemberTypes.Method)
                        {
                            MethodInfo method = (MethodInfo)input.MethodBase;
                            if (method.ReturnType != typeof(void))
                            {
                                result.Exception =
                                new InvalidOperationException(
                                    Resources.CantSwallowNonVoidReturnMessage);
                            }
                        }
                    }
                    // Otherwise the original exception will be returned to the previous handler
                }
                catch (Exception ex)
                {
                    // New exception was returned
                    result.Exception = ex;
                }
            }
            return result;
        }
        #endregion
    }
}