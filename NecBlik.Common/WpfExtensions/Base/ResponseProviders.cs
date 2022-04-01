using NecBlik.Common.WpfExtensions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Common.WpfExtensions.Base
{
    /// <summary>
    /// Generic implementation of an IResponseProvider
    /// </summary>
    /// <typeparam name="R">Response type</typeparam>
    /// <typeparam name="Q">Question context type</typeparam>
    public class GenericResponseProvider<R, Q> : IResponseProvider<R, Q> where Q : class
    {
        private R response;

        public Func<Q, R> ProviderFunction = null;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="response">Response that will be returned on any question</param>
        public GenericResponseProvider(R response)
        {
            this.response = response;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="providerFunction">Function that gets the response when provider is asked for one.</param>
        public GenericResponseProvider(Func<Q, R> providerFunction=null)
        {
            this.ProviderFunction = providerFunction;
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <param name="questionContext">Question context</param>
        /// <returns>Response as a given one or taken from the function if defaultresponse==null</returns>
        public R ProvideResponse(Q questionContext = null)
        {
            if (this.ProviderFunction != null)
            {
                return this.ProviderFunction.Invoke(questionContext);
            }
            return response;
        }
    }

    /// <summary>
    /// Generic implementation of an IInterruptibleResponseProvider
    /// </summary>
    /// <typeparam name="R">Response type</typeparam>
    /// <typeparam name="Q">Question context type</typeparam>
    public class GenericInterruptibleResponseProvider<R, Q> : IInterruptibleResponseProvider<R, Q> where Q : class
    {
        private R response;

        public Func<Q, R> ProviderFunction = null;

        public Action InterruptAction = null;

        public Task<R> task = null;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="response">Response that will be returned on any question</param>
        public GenericInterruptibleResponseProvider(R response)
        {
            this.response = response;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="providerFunction">Function that gets the response when provider is asked for one.</param>
        /// <param name="interruptAction">Action to be invoked if the responseproviding process is interrupted.</param>
        public GenericInterruptibleResponseProvider(Func<Q, R> providerFunction,Action interruptAction)
        {
            this.InterruptAction = interruptAction;
            this.ProviderFunction = providerFunction;
        }

        /// <summary>
        /// Async function that provides a response. The previously given function is called as a separate tassk that is then awaited. 
        /// </summary>
        /// <param name="questionContext">Question context</param>
        /// <returns>Response from user</returns>
        public async Task<R> ProvideResponse(Q questionContext = null)
        {
             task =  Task<R>.Run( async ()=> {
                if (this.ProviderFunction != null)
                {
                    return this.ProviderFunction.Invoke(questionContext);
                }
                return response;
            });
            return await task;
        }

        /// <summary>
        /// Function that calls the interrupting callback.
        /// </summary>
        /// <returns>Task</returns>
        public async Task Interrupt()
        {
            this.InterruptAction?.Invoke();
        }
    }

    /// <summary>
    /// Generic implementation of an ISimpleTypeResponseProvider
    /// </summary>
    /// <typeparam name="R">Response type</typeparam>
    /// <typeparam name="Q">Question context type</typeparam>
    public class SimpleTypeResponseProvider<R, Q> : ISimpleTypeResponseProvider<R, Q>
    {
        private R response;

        public Func<Q, R> ProviderFunction = null;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="response">Response that will be returned on any question</param>
        public SimpleTypeResponseProvider(R Response)
        {
            this.response = Response;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="providerFunction">Function that gets the response when provider is asked for one.</param>
        public SimpleTypeResponseProvider(Func<Q, R> ProviderFunction)
        {
            this.ProviderFunction = ProviderFunction;
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        /// <param name="question">Question context</param>
        /// <returns>Response as a given one or taken from the function if defaultresponse==null</returns>
        public R ProvideResponse(Q question)
        {
            if (this.ProviderFunction != null)
            {
                return this.ProviderFunction.Invoke(question);
            }
            return this.response;
        }
    }

    /// <summary>
    /// Simple IResponeProvider that returns always true
    /// </summary>
    public class YesStringResponseProvider : IResponseProvider<bool,string>
    {
        public YesStringResponseProvider()
        {

        }

        public bool ProvideResponse(string question = null)
        {
            return true;
        }
    }

    /// <summary>
    /// Simple IResponeProvider that returns always false
    /// </summary>
    public class NoStringResponseProvider : IResponseProvider<bool,string>
    {
        public NoStringResponseProvider()
        {

        }

        public bool ProvideResponse(string question = null)
        {
            return false;
        }
    }
}
