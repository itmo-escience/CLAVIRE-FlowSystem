using System;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Interface for objects supporting getting values.
    /// </summary>
    [Obsolete]
    public interface IValueGetter
    {
        /// <summary>
        /// Gets value according to the passed identifier.
        /// </summary>
        /// <param name="identifier">Identifier.</param>
        /// <returns>Return value.</returns>        
        ValueBase GetValue(CompoundVarIdentifier identifier);
    }
}