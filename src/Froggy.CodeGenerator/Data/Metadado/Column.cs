using System;
using System.Collections.Generic;
using System.Data;

namespace Froggy.CodeGenerator.Data.Metadado
{
    public class Column
    {
        private static readonly Dictionary<Type, DbType> _TypeToDbType;

        public static DbType GetDbTypeByType(Type type)
        {
            if (_TypeToDbType.ContainsKey(type))
            {
                return _TypeToDbType[type];
            }
            return DbType.Object;
        }

        static Column()
        {
            _TypeToDbType = new Dictionary<Type, DbType>
                                {
                                    {typeof (Boolean), DbType.Boolean},
                                    {typeof (Byte), DbType.Byte},
                                    {typeof (Byte[]), DbType.Binary},
                                    {typeof (DateTime), DbType.DateTime},
                                    {typeof (Decimal), DbType.Decimal},
                                    {typeof (Double), DbType.Double},
                                    {typeof (Guid), DbType.Guid},
                                    {typeof (Int16), DbType.Int16},
                                    {typeof (Int32), DbType.Int32},
                                    {typeof (Int64), DbType.Int64},
                                    {typeof (Object), DbType.Object},
                                    {typeof (String), DbType.AnsiString},
                                    {typeof (Char), DbType.AnsiString}
                                };
        }

        private Type dataType;

        public string Name { get; set; }
        public bool AllowNull { get; set; }
        public bool ReadOnly { get; set; }
        public DbType DbType { get; private set; }

        public Type DataType
        {
            get { return dataType; }
            set 
            { 
                dataType = value;
                DbType = GetDbTypeByType(dataType);
            }
        }

        public int MaxLength { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}