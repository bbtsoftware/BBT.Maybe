namespace BBT.Maybe.Tests.TestData
{
    /// <summary>
    /// Used for test purposes.
    /// </summary>
    public struct BaseStruct
    {
        public BaseStruct(string value = "")
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}
