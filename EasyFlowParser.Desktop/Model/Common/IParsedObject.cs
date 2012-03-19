using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easis.Wfs.EasyFlow.Model
{
    /// <summary>
    /// Interface for parsed object.
    /// </summary>
    public interface IParsedObject
    {
        /// <summary>
        /// Gets or sets script text behind this object.
        /// </summary>
        string TextBehind { get; set; }

        /// <summary>
        /// Gets or sets text range in script for this object.
        /// </summary>
        TextRange TextRange { get; set; }

        /// <summary>
        /// Gets or sets whether there were errors during parsing this object.
        /// </summary>
        bool IsInvalid { get; set; }

        /// <summary>
        /// Gets or sets whether.
        /// </summary>
        bool IsComplete { get; set; }        

        /// <summary>
        /// Область, которой принадлежит объект.
        /// </summary>
        Scope Scope { get; set; }

        /// <summary>
        /// Сообщение об ошибке в случае её наличия.
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
