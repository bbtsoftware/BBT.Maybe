namespace BBT.MaybePattern
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
        private readonly T value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T}"/> struct.
        /// </summary>
        /// <param name="value">The potentially nullable value.</param>
        internal Maybe(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T}"/> struct.
        /// Constructor used for deserialization.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        internal Maybe(SerializationInfo info, StreamingContext context)
        {
            this.value = MaybeUtils.GetDeserializedValue<T>(info, nameof(this.value));
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Maybe"/> has a value (true)
        /// or is representing the null case (false).
        /// </summary>
        public bool HasValue => this.value != null;

        /// <summary>
        /// Checks whether the operands are equal.
        /// </summary>
        /// <param name="a">Maybe to compare.</param>
        /// <param name="b">Maybe to compare.</param>
        public static bool operator ==(Maybe<T> a, Maybe<T> b)
        {
            return object.Equals(a.value, b.value);
        }

        /// <summary>
        /// Checks whether the operands are unequal.
        /// </summary>
        /// <param name="a">Maybe to compare.</param>
        /// <param name="b">Maybe to compare.</param>
        public static bool operator !=(Maybe<T> a, Maybe<T> b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Executes <paramref name="doAction"/> if value is not null.
        /// </summary>
        /// <param name="doAction">The action which is performed if maybe is not none.</param>
        /// <returns>The none case.</returns>
        public NoneCase Do(Action<T> doAction)
        {
            MaybeUtils.CheckArgumentNotNull(doAction, nameof(doAction));

            if (this.value != null)
            {
                doAction(this.value);
                return new NoneCase(false);
            }

            return new NoneCase(true);
        }

        /// <summary>
        /// Gets the <see cref="Maybe{TResult}"/> of the projection function <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="func">The projection function.</param>
        /// <returns>The projected maybe.</returns>
        public Maybe<TResult> Some<TResult>(Func<T, TResult> func)
            where TResult : class
        {
            MaybeUtils.CheckArgumentNotNull(func, nameof(func));

            var maybe = Maybe.None<TResult>();
            this.Do(x => maybe = Maybe.Some(func(x)));

            return maybe;
        }

        /// <summary>
        /// Gets the <see cref="Maybe{TResult}"/> of the projection function <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="func">The projection function.</param>
        /// <returns>The projected maybe.</returns>
        public Maybe<TResult> Some<TResult>(Func<T, Maybe<TResult>> func)
            where TResult : class
        {
            MaybeUtils.CheckArgumentNotNull(func, nameof(func));

            var maybe = Maybe.None<TResult>();
            this.Do(x => maybe = func(x));

            return maybe;
        }

        /// <summary>
        /// Gets the <see cref="MaybeStruct{TResult}"/> of the projection function <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="func">The projection function.</param>
        /// <returns>The projected maybe.</returns>
        public MaybeStruct<TResult> SomeStruct<TResult>(Func<T, TResult> func)
            where TResult : struct
        {
            MaybeUtils.CheckArgumentNotNull(func, nameof(func));

            var maybe = Maybe.NoneStruct<TResult>();
            this.Do(x => maybe = Maybe.SomeStruct<TResult>(func(x)));

            return maybe;
        }

        /// <summary>
        /// Gets the <see cref="MaybeStruct{TResult}"/> of the projection function <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the projected result.</typeparam>
        /// <param name="func">The projection function.</param>
        /// <returns>The projected maybe.</returns>
        public MaybeStruct<TResult> SomeStruct<TResult>(Func<T, MaybeStruct<TResult>> func)
            where TResult : struct
        {
            MaybeUtils.CheckArgumentNotNull(func, nameof(func));

            var maybe = Maybe.NoneStruct<TResult>();
            this.Do(x => maybe = func(x));

            return maybe;
        }

        /// <summary>
        /// Returns the value in case it is initialized.
        /// Otherwise throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="maybeParameterName">The maybe reference used in error message.</param>
        /// <param name="additionalMessage">Additional error message.</param>
        /// <returns>The value.</returns>
        public T ThrowExceptionIfNone(
            string maybeParameterName,
            string additionalMessage = "")
        {
            return MaybeUtils.CheckParameterNotNull(this.value, maybeParameterName, additionalMessage);
        }

        /// <summary>
        /// See <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Maybe<T>))
            {
                return false;
            }

            return this.Equals((Maybe<T>)obj);
        }

        /// <summary>
        /// See <see cref="IEquatable{T}.Equals(T)"/>.
        /// </summary>
        /// <param name="other">The maybe to compare.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public bool Equals(Maybe<T> other)
        {
            return this == other;
        }

        /// <summary>
        /// See <see cref="object.GetHashCode()"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            if (this.HasValue)
            {
                return this.value.GetHashCode();
            }

            return base.GetHashCode();
        }

        /// <summary>
        /// See <see cref="ISerializable.GetObjectData"/>.
        /// </summary>
        /// <param name="info">The serialization info.</param>
        /// <param name="context">The streaming context.</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            MaybeUtils.SetData(info, nameof(this.value), this.value);
        }

        /// <summary>
        /// See <see cref="object.ToString"/>.
        /// </summary>
        /// <returns>The string representation.</returns>
        public override string ToString()
        {
            if (this.HasValue)
            {
                return this.value.ToString();
            }

            return string.Empty;
        }
    }
}
