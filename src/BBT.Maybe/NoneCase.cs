namespace BBT.Maybe
{
    using System;

    /// <summary>
    /// Used in context of <see cref="Maybe{T}"/>. Used to provide <see cref="DoIfNone"/> method.
    /// </summary>
    public struct NoneCase : IEquatable<NoneCase>
    {
        private readonly bool mIsNone;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoneCase"/> struct.
        /// </summary>
        /// <param name="aIsNone">True if representing the none case, false if value is set.</param>
        internal NoneCase(bool aIsNone)
        {
            this.mIsNone = aIsNone;
        }

        /// <summary>
        /// Checks whether the operands are equal.
        /// </summary>
        /// <param name="aA">Maybe to compare.</param>
        /// <param name="aB">Maybe to compare.</param>
        public static bool operator ==(NoneCase aA, NoneCase aB)
        {
            return aA.mIsNone.Equals(aB.mIsNone);
        }

        /// <summary>
        /// Checks whether the operands are unequal.
        /// </summary>
        /// <param name="aA">Maybe to compare.</param>
        /// <param name="aB">Maybe to compare.</param>
        public static bool operator !=(NoneCase aA, NoneCase aB)
        {
            return !(aA == aB);
        }

        /// <summary>
        /// The <paramref name="aDoAction"/> is called in none case.
        /// </summary>
        /// <param name="aDoAction">The action which is performed if maybe is none.</param>
        public void DoIfNone(Action aDoAction)
        {
            if (aDoAction == null)
            {
                throw new ArgumentNullException(nameof(aDoAction));
            }

            if (this.mIsNone)
            {
                aDoAction();
            }
        }

        /// <summary>
        /// See <see cref="object.Equals(object)"/>.
        /// </summary>
        /// <param name="aObj">The object to compare.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public override bool Equals(object aObj)
        {
            if (!(aObj is NoneCase))
            {
                return false;
            }

            return this.Equals((NoneCase)aObj);
        }

        /// <summary>
        /// See <see cref="IEquatable{T}.Equals(T)"/>.
        /// </summary>
        /// <param name="aOther">The maybe to compare.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public bool Equals(NoneCase aOther)
        {
            return this == aOther;
        }

        /// <summary>
        /// See <see cref="object.GetHashCode()"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode() => this.mIsNone.GetHashCode();
    }
}