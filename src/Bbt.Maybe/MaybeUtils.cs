namespace Bbt.Maybe
{
    using System;
    using System.Globalization;
    using System.Runtime.Serialization;

    /// <summary>
    /// Provides utility methods for maybe.
    /// </summary>
    internal static class MaybeUtils
    {
        /// <summary>
        /// Asserts that <paramref name="aArg"/> is not null.
        /// In case of null an <see cref="ArgumentNullException"/> is thrown.
        /// </summary>
        /// <typeparam name="T">The argument's type.</typeparam>
        /// <param name="aArg">The argument.</param>
        /// <param name="aArgName">The arguments name.</param>
        /// <returns>The argument.</returns>
        internal static T CheckArgumentNotNull<T>(T aArg, string aArgName)
            where T : class
        {
            if (aArg == null)
            {
                throw new ArgumentNullException(aArgName);
            }

            return aArg;
        }

        /// <summary>
        /// Asserts that <paramref name="aParameter"/> is not null.
        /// In case of null an <see cref="InvalidOperationException"/> is thrown.
        /// </summary>
        /// <typeparam name="T">The parameter's type.</typeparam>
        /// <param name="aParameter">The parameter.</param>
        /// <param name="aParameterName">The parameter's name.</param>
        /// <param name="aAdditionalMessage">An optional additional error message.</param>
        /// <returns>The parameter.</returns>
        internal static T CheckParameterNotNull<T>(
            T aParameter, string aParameterName, string aAdditionalMessage = "")
        {
            if (aParameter == null)
            {
                var lMsg = $"({aParameterName}) may not be null.".ToString(CultureInfo.InvariantCulture);
                if (!string.IsNullOrEmpty(aAdditionalMessage))
                {
                    lMsg = lMsg + " " + aAdditionalMessage;
                }

                throw new InvalidOperationException(lMsg);
            }

            return aParameter;
        }

        /// <summary>
        /// Gets the casted value from <paramref name="aInfo"/>
        /// corresponding to <paramref name="aKey"/>.
        /// </summary>
        /// <typeparam name="T">The value's type.</typeparam>
        /// <param name="aInfo">The serialization info.</param>
        /// <param name="aKey">The key.</param>
        /// <returns>The corresponding value.</returns>
        internal static T GetDeserializedValue<T>(
            SerializationInfo aInfo,
            string aKey)
        {
            return (T)aInfo.GetValue(aKey, typeof(T));
        }
    }
}
