// DO NOT MODIFY THIS FILE.
// Polyfill for the .NET attribute that is not available in .NET Standard 2.0.
// It must exactly match the original definition, and the compiler recognizes it only by its full name.

// ReSharper disable All

#pragma warning disable IDE0130
#pragma warning disable IDE0290

namespace System.Diagnostics.CodeAnalysis
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.ReturnValue, AllowMultiple = true, Inherited = false)]
    internal sealed class NotNullIfNotNullAttribute : Attribute
    {
        #region Constructors

        public NotNullIfNotNullAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        #endregion

        #region Properties

        public string ParameterName { get; }

        #endregion
    }
}
