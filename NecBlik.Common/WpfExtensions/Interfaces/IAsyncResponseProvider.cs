using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NecBlik.Common.WpfExtensions.Interfaces
{
    /// <summary>
    /// Interface for user interaction providing.
    /// </summary>
    /// <typeparam name="R">Response type</typeparam>
    /// <typeparam name="Q">Question context type</typeparam>
    public interface IAsyncResponseProvider<R,Q>where Q:class
    {
        /// <summary>
        /// Returns a response based on given context.
        /// </summary>
        /// <param name="context">Context of the question</param>
        /// <returns>Response of R type</returns>
        public Task<R> ProvideResponseAsync(Q context=null);
    }

    /// <summary>
    /// Interface for user interaction providing.
    /// </summary>
    /// <typeparam name="R">Response type</typeparam>
    /// <typeparam name="Q">Question context type</typeparam>
    public interface ISimpleTypeAsyncResponseProvider<R, Q>
    {
        /// <summary>
        /// Returns a response based on given context.
        /// </summary>
        /// <param name="context">Context of the question</param>
        /// <returns>Response of R type</returns>
        public Task<R> ProvideResponse(Q context);
    }

    /// <summary>
    /// Interface for user interaction providing. It assumes that the action is async and interruptible.
    /// </summary>
    /// <typeparam name="R">Response type</typeparam>
    /// <typeparam name="Q">Question context type</typeparam>
    public interface IInterruptibleAsyncResponseProvider<R,Q> where Q:class
    {
        /// <summary>
        /// Returns a response based on given context.
        /// </summary>
        /// <param name="context">Context of the question</param>
        /// <returns>Task with response of R type</returns>
        public Task<R> ProvideResponse(Q context);

        /// <summary>
        /// Interrupts response providing.
        /// </summary>
        /// <returns>Task</returns>
        public Task Interrupt();
    }
}
