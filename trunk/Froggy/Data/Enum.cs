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
        /// Any change in <see cref="DbCommandUtil"/> affect <see cref="DataAdapterCommand.SelectCommand"/> of the <see cref="DataAdapter"/> in <see cref="DbCommandUtil"/>
        /// </summary>
        SelectCommand,
        /// <summary>
        /// Any change in <see cref="DbCommandUtil"/> affect <see cref="DataAdapterCommand.InsertCommand"/> of the <see cref="DataAdapter"/> in <see cref="DbCommandUtil"/>
        /// </summary>
        InsertCommand,
        /// <summary>
        /// Any change in <see cref="DbCommandUtil"/> affect <see cref="DataAdapterCommand.UpdateCommand"/> of the <see cref="DataAdapter"/> in <see cref="DbCommandUtil"/>
        /// </summary>
        UpdateCommand,
        /// <summary>
        /// Any change in <see cref="DbCommandUtil"/> affect <see cref="DataAdapterCommand.DeleteCommand"/> of the <see cref="DataAdapter"/> in <see cref="DbCommandUtil"/>
        /// </summary>
        DeleteCommand
    }
}