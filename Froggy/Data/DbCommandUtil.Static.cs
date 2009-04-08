using System;
using System.Data;
using System.Data.Common;

namespace Froggy.Data
{
    public partial class DbCommandUtil
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="daScopeContext"></param>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static DbCommand CreateCommand(DAScopeContext daScopeContext, string commandText, CommandType commandType, int commandTimeout)
        {
            if (daScopeContext == null)
                throw new ArgumentNullException("DAScopeContext");
            DbCommand command = daScopeContext.ProviderFactory.CreateCommand();
            command.Connection = daScopeContext.Connection;
            command.CommandText = commandText;
            command.CommandType = commandType;
            command.CommandTimeout = commandTimeout;
            if (daScopeContext.Transaction != null)
            {
                command.Transaction = daScopeContext.Transaction;
            }
            return command;
        }

    }
}
