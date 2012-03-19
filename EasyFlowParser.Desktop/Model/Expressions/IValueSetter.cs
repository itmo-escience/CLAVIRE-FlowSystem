using System;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Interface for object supporting setting values.
    /// </summary>
    [Obsolete]
    public interface IValueSetter
    {
        ///<summary>
        /// Sets value accorfing to the passed identifier. 
        ///</summary>
        ///<param name="identifier">Identifier.</param>
        ///<param name="value">Value to be set.</param>
        void SetValue(CompoundVarIdentifier identifier, ValueBase value);
    }
}