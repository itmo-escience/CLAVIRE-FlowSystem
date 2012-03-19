using System.Collections.Generic;
using System.Linq;

namespace Easis.Common
{
    /// <summary>
    /// Hash code generator class.
    /// </summary>
    public static class HashCode
    {
        /// <summary>
        /// Returns combined hash code for the list objects.
        /// </summary>
        /// <typeparam name="T">Type of objects.</typeparam>
        /// <param name="objects">List of objects to get combined hash.</param>
        /// <returns>Combined hash code.</returns>
        public static int Generate<T>(IEnumerable<T> objects)
        {            
            return Combine(objects.Select(obj => ReferenceEquals(obj, null) ? 0 : obj.GetHashCode()));
        }

        /// <summary>
        /// Returns combined hash code for the list of hashes.
        /// </summary>
        /// <param name="hashes">List of hashes.</param>
        /// <returns>Combined hash code.</returns>
        public static int Combine(IEnumerable<int> hashes)
        {
            unchecked
            {
                return hashes.Aggregate(0, (current, hash) => (current * 397) ^ hash);
            }
        }

        /// <summary>
        /// Returns combined hash for the list of hashes.
        /// </summary>
        /// <param name="hashes">List of hashes.</param>
        /// <returns>Combined hash code.</returns>
        public static int Combine(params int [] hashes)
        {
            return Combine(hashes.AsEnumerable());
        }

        /// <summary>
        /// Returns combined hash code for the list objects.
        /// </summary>
        /// <param name="objects">List of objects.</param>
        /// <returns>Combined hash code.</returns>
        public static int Generate(params object [] objects)
        {
            return Generate(objects.AsEnumerable());
        }
    }
}