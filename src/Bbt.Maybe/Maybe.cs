namespace Bbt.Maybe
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents a reference type which may be null or not null.
    /// </summary>
    /// <remarks>
    /// Implementation of the "maybe monad" pattern / Optional-pattern.
    /// https://smellegantcode.wordpress.com/2008/12/11/the-maybe-monad-in-c/.
    /// </remarks>
    /// <typeparam name="T">The reference type.</typeparam>
    [Serializable]
    public struct Maybe<T> : ISerializable, IEquatable<Maybe<T>>
        where T : class
    {
        private readonly T mValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T}"/> struct.
        /// </summary>
        /// <param name="aValueNullable">The potentially nullable value.</param>
        internal Maybe(T aValueNullable)
        {
            this.mValue = aValueNullable;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T}"/> struct.
        /// Constructor used for deserialization.
        /// </summary>
        private Maybe(SerializationInfo aInfo, StreamingContext aContext)
        {
            MaybeUtils.CheckArgumentNotNull(aInfo, nameof(aInfo));
            this.mValue = MaybeUtils.GetDeserializedValue<T>(aInfo, nameof(this.mValue));
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Maybe"/> has a value (true)
        /// or is representing the null case (false).
        /// </summary>
        public bool HasValue => this.mValue != null;

        /// <summary>
        /// Checks whether the operands are equal.
        /// </summary>
        /// <param name="aA">Maybe to compare.</param>
        /// <param name="aB">Maybe to compare.</param>
        public static bool operator ==(Maybe<T> aA, Maybe<T> aB)
        {
            return object.Equals(aA.mValue, aB.mValue);
        }

        /// <summary>
        /// Checks whether the operands are unequal.
        /// </summary>
        /// <param name="aA">Maybe to compare.</param>
        /// <param name="aB">Maybe to compare.</param>
        public static bool operator !=(Maybe<T> aA, Maybe<T> aB)
        {
            return !(aA == aB);
        }

        /// <summary>
        /// Executes <paramref name="aDoAction"/> if value is not null.
        /// </summary>
        /// <param name="aDoAction">The action which is performed if maybe is not none.</param>
        /// <returns>The none case.</returns>
        public NoneCase Do(Action<T> aDoAction)
        {
            MaybeUtils.CheckArgumentNotNull(aDoAction, nameof(aDoAction));

            if (this.mValue != null)
            {
                aDoAction(this.mValue);
                return new NoneCase(false);
            }

            return new NoneCase(true);
        }

        /// <summary>
        /// Gets the <see cref="Maybe{TResult}"/> of the projection function <paramref name="aFunc"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="aFunc">The projection function.</param>
        /// <returns>The projected maybe.</returns>
        public Maybe<TResult> Some<TResult>(Func<T, TResult> aFunc)
            where TResult : class
        {
            MaybeUtils.CheckArgumentNotNull(aFunc, nameof(aFunc));

            var lMaybe = Maybe.None<TResult>();
            this.Do(aX => lMaybe = Maybe.Some(aFunc(aX)));

            return lMaybe;
        }

        /// <summary>
        /// Returns the value in case it is initialized.
        /// Otherwise throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="aMaybeParameterName">The maybe reference used in error message.</param>
        /// <param name="aAdditionalMessage">Additional error message.</param>
        /// <returns>The value.</returns>
        public T ThrowExceptionIfNone(
            string aMaybeParameterName,
            string aAdditionalMessage = "")
        {
            return MaybeUtils.CheckParameterNotNull(this.mValue, aMaybeParameterName, aAdditionalMessage);
        }

        /// <summary>
        /// See <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <param name="aObj">The object to compare.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public override bool Equals(object aObj)
        {
            if (!(aObj is Maybe<T>))
            {
                return false;
            }

            return this.Equals((Maybe<T>)aObj);
        }

        /// <summary>
        /// See <see cref="IEquatable{T}.Equals(T)"/>.
        /// </summary>
        /// <param name="aOther">The maybe to compare.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public bool Equals(Maybe<T> aOther)
        {
            return this == aOther;
        }

        /// <summary>
        /// See <see cref="object.GetHashCode()"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            if (this.HasValue)
            {
                this.mValue.GetHashCode();
            }

            return base.GetHashCode();
        }

        /// <summary>
        /// See <see cref="ISerializable.GetObjectData"/>.
        /// </summary>
        /// <param name="aInfo">The serialization info.</param>
        /// <param name="aContext">The streaming context.</param>
        public void GetObjectData(SerializationInfo aInfo, StreamingContext aContext)
        {
            if (aInfo == null)
            {
                throw new ArgumentNullException(nameof(aInfo));
            }

            aInfo.AddValue(nameof(this.mValue), this.mValue, typeof(T));
        }

        /// <summary>
        /// See <see cref="object.ToString"/>.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            if (this.HasValue)
            {
                return this.mValue.ToString();
            }

            return string.Empty;
        }
    }
}
