namespace BBT.MaybePattern.Tests.TestData
{
    /// <summary>
    /// Used for test purposes.
    /// </summary>
    public struct ReferencedStruct
    {
        /// <summary>
        /// Gets or sets the owner of the 1-n relationship.
        /// </summary>
        public BaseStruct ReferencingStruct { get; set; }
    }
}
