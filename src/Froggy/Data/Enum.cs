using System.Data.Common;

namespace Froggy.Data
{
    /// <summary>
    /// Options for transacions in <see cref="DaScopeContext"/>
    /// </summary>
    public enum TransactionOption
    {
        /// <summary>
        /// Use a existing transacion, if one exists, or not use trasaction at all, if not yet exists. This is the Default
        /// </summary>
        Automatic,
        /// <summary>
        /// Use a existing transacion, if one exists, or create a new tramsaction, if not yet exists
        /// </summary>
        Required
    }

    /// <summary>
    /// Config <see cref="DbCommandUtil"/> to command type of <see cref="DataAdapter"/>
    /// </summary>
    public enum DataAdapterCommand
    {
        /// <summary>
        /// Any change in <see cref="DbCommandUtil"/> affect <see cref="DataAdapterCommand.Select"/> of the <see cref="DataAdapter"/> in <see cref="DbCommandUtil"/>
        /// </summary>
        Select,
        /// <summary>
        /// Any change in <see cref="DbCommandUtil"/> affect <see cref="DataAdapterCommand.Insert"/> of the <see cref="DataAdapter"/> in <see cref="DbCommandUtil"/>
        /// </summary>
        Insert,
        /// <summary>
        /// Any change in <see cref="DbCommandUtil"/> affect <see cref="DataAdapterCommand.Update"/> of the <see cref="DataAdapter"/> in <see cref="DbCommandUtil"/>
        /// </summary>
        Update,
        /// <summary>
        /// Any change in <see cref="DbCommandUtil"/> affect <see cref="DataAdapterCommand.Delete"/> of the <see cref="DataAdapter"/> in <see cref="DbCommandUtil"/>
        /// </summary>
        Delete
    }
}