using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Data;
using System.IO;
using Froggy.CodeGenerator.Data.Metadado;
using Froggy.Data;
using Microsoft.CSharp;
using TypedDataSetGenerator=System.Data.Design.TypedDataSetGenerator;

namespace Froggy.CodeGenerator.Data
{
    public class DataAccess
    {
        public DataAccess(string dataSetName, params string[] tablesFullNames)
        {
            var tables = Array.ConvertAll(tablesFullNames, fullName => new Table(fullName));
            var dataSet = new DataSet(dataSetName);
            foreach (var table in tables)
            {
                dataSet.Merge(table.DataTableSchema);
            }

            var sw = new StringWriter();
            dataSet.WriteXmlSchema(sw);
            sw.Flush();
            var codeUnit = new CodeCompileUnit();
            var codeNamespace = new CodeNamespace("Test");
            var codeProvider = new CSharpCodeProvider();
            codeUnit.Namespaces.Add(codeNamespace);
            using (var dasc = new DAScopeContext())
            {
                TypedDataSetGenerator.Generate(sw.ToString(),
                                               codeUnit,
                                               codeNamespace,
                                               codeProvider,
                                               dasc.ProviderFactory
                    );
            }
            sw = new StringWriter();
            codeProvider.GenerateCodeFromCompileUnit(codeUnit, sw, new CodeGeneratorOptions());
        }
    }
}