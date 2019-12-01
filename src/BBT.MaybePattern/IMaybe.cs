namespace BBT.MaybePattern
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Interface for any maybe-implementation which supports
    /// covariance.
    /// </summary>
    /// <remarks>
    /// Deliberately implements <see cref="ISerializable"/>, but not <see cref="IEquatable{T}"/>
    /// as <see cref="IEquatable{T}"/> is not covariant.
    /// See also https://stackoverflow.com/questions/3289440/why-was-iequatable-t-not-made-contravariant-in-t-for-c-sharp-4-0.
    /// </remarks>
    /// <typeparam name="T">Type which is encapsulated by the <see cref="IMaybe{T}"/>.</typeparam>
    public interface IMaybe<out T> : ISerializable
    {
        /// <summary>
        /// Gets a value indicating whether <see cref="Maybe"/> has a value (true)
        /// or is representing the null case (false).
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        /// Executes <paramref name="doAction"/> if value is not null.
        /// </summary>
        /// <param name="doAction">The action which is performed if maybe is not none.</param>
        /// <returns>The none case.</returns>
        NoneCase Do(Action<T> doAction);

        /// <summary>
        /// Returns the value in case it is initialized.
        /// Otherwise throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="maybeParameterName">The maybe reference used in error message.</param>
        /// <param name="additionalMessage">Additional error message.</param>
        /// <returns>The value.</returns>
        T ThrowExceptionIfNone(string maybeParameterName, string additionalMessage = "");
    }
}
