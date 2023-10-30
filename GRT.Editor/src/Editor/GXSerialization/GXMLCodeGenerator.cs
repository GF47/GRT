using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using GRT.Data;
using Modules;
using FairyGUI.Utils;
using System.CodeDom;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine.Pool;

namespace GRT.Editor.GXSerialization
{
    public static class GXMLCodeGenerator
    {
        [MenuItem("Assets/Project/Generate GXML Factories")]
        private static void Generate()
        {
            Debug.Log(Generate(typeof(_temp.Actor)));
        }

        private static string Generate(Type type)
        {
            // var unit = new CodeCompileUnit();

            // var ns = GenNamespace();
            // unit.Namespaces.Add(ns);

            // var cls = GenCodeTypeDeclaration(type);
            // ns.Types.Add(cls);



            var sb = new StringBuilder();
            sb.Append(TEMPLATE_USING);
            sb.Append(TEMPLATE_NAMESPACE_A);
            {
                sb.Append(TEMPLATE_CLASS_A(typeof(_temp.Actor)));
                sb.Append(TEMPLATE_CLASS_B);
            }
            sb.Append(TEMPLATE_NAMESPACE_B);

            return sb.ToString();
        }

        private const string TAB = "    ";
        private const string TEMPLATE_USING = @"// auto generated code, do not change

using GRT.Data;
using System;
using System.Collections.Generic;";

        private const string TEMPLATE_NAMESPACE_A = @"
namespace GXFactories
{";
        private const string TEMPLATE_NAMESPACE_B = @"
}";

        private static string TEMPLATE_CLASS_A(Type type) => $@"
    public class {type.FULL_NAME().Replace('.', '_')}_GXFactory<T> : IGXSerializableFactory<T, {type.FULL_NAME()}>
    {{";
        private const string TEMPLATE_CLASS_B = @"
    }";

        private static string FULL_NAME(this Type type)
        {
            var name = $"{type.Namespace}.{nameof(type)}";
            if (type.IsGenericType)
            {
                name += "<";
                var addComma = false;
                foreach (var arg in type.GetGenericArguments())
                {
                    if (addComma) { name += ", "; } else { addComma = true; }
                    name += arg.FULL_NAME();
                }
                name += ">";
            }
            return name;
        }

        // private static CodeNamespace GenNamespace()
        // {
        //     var ns = new CodeNamespace("GXFactories");
        //     ns.Imports.Add(new CodeNamespaceImport("GRT.Data"));
        //     ns.Imports.Add(new CodeNamespaceImport("System"));
        //     ns.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
        //     return ns;
        // }

        // private static CodeTypeDeclaration GenCodeTypeDeclaration(Type type)
        // {
        //     var cls = new CodeTypeDeclaration($"{type.FullName.Replace('.', '_')}_GXFactory")
        //     {
        //         IsClass = true,
        //     };
        //     cls.BaseTypes.Add($"IGXSerializableFactory<T, {type.FullName}>");
        //     cls.TypeParameters.Add("T");
        //     cls.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
        //     return cls;
        // }

        // private static CodeMemberMethod GenCodeMemberMethod_Serialize()
        // {
        //     var method = new CodeMemberMethod();
        //     method.Attributes = MemberAttributes.Public | MemberAttributes.Final;
        //     method.Name = "Serialize";
        //     method.Parameters.Add(new CodeParameterDeclarationExpression("IGXML<T>", "xml"));
        //     method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(object)), "@object"));
        //     method.Parameters.Add(new CodeParameterDeclarationExpression("T", "parentNode"));
        //     method.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(IGXAttribute)), "refAttribute = default"));
        //     var statement = new CodeMethodInvokeExpression(new CodeMethodReferenceExpression())
        // }
    }
}
