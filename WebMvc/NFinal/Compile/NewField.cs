﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;

namespace NFinal.Compile
{
    public struct StructField
    {
        public string typeName;
        public string varName;
        public int index;
        public int length;
        public string csharpCode;

        public bool isNumber;
        public bool isSimple;
        public bool isList;
    }
    public class NewField
    {
        public string varName;
        public NewField(string varName)
        {
            this.varName = varName;
        }
        public bool IsNumber(string typeName)
        {
            bool isNumber = true;
            if(typeName.LastIndexOf('.')>-1)
            {
                typeName =typeName.Substring(typeName.LastIndexOf('.')+1);            
            }
            switch (typeName)
            {

                case "short": isNumber = true; break;
                case "ushort": isNumber = true; break;
                case "byte": isNumber = true; break;
                case "sbyte": isNumber = true; break;
                case "int": isNumber = true; break;
                case "uint": isNumber = true; break;
                case "long": isNumber = true; break;
                case "ulong": isNumber = true; break;
                case "float": isNumber = true; break;
                case "double": isNumber = true; break;
                case "decimal": isNumber = true; break;
                case "Int16": isNumber = true; break;
                case "Int32": isNumber = true; break;
                case "Int64": isNumber = true; break;
                case "UInt16": isNumber = true; break;
                case "UInt32": isNumber = true; break;
                case "UInt64": isNumber = true; break;
                case "Byte": isNumber = true; break;
                case "SByte": isNumber = true; break;
                case "Single": isNumber = true; break;
                case "Double": isNumber = true; break;
                case "Decimal": isNumber = true; break;
                default: isNumber = false; break;
            }
            return isNumber;
        }
        public List<StructField> GetFields(ref string csharpCode,string MethodName)
        {
            
            string partern = varName + @"\s*.\s*AddNewField\s*\(""([-_0-9a-zA-Z]+)""(?:\s*,\s*typeof\(\s*([^()]+)\))?\s*\)\s*;";
            Regex addNewFieldReg = new Regex(partern);
            MatchCollection addNewFieldMats = addNewFieldReg.Matches(csharpCode);
            StructField field;
            List<StructField> structFields = new List<StructField>();
            string note="";
            if (addNewFieldMats.Count>0)
            {
                partern =@"List\s*<\s*dynamic\s*>";
                Regex ListdynamicTypeReg = new Regex(partern);
                Match ListdynamicTypeMat;
                for (int i = 0; i < addNewFieldMats.Count; i++)
                {
                    field = new StructField();
                    field.index = addNewFieldMats[i].Index;
                    field.length = addNewFieldMats[i].Length;
                    field.varName = addNewFieldMats[i].Groups[1].Value;
                    
                    //原位置替换成注释语句
                    csharpCode = csharpCode.Remove(field.index,field.length);
                    note= string.Format("/*field:{0}.{1}*/",varName,field.varName);
                    csharpCode =csharpCode.Insert(field.index,note.PadRight(field.length));

                    ListdynamicTypeMat = ListdynamicTypeReg.Match(field.typeName);
                    field.isSimple = true;
                    field.isList = false;
                    if (addNewFieldMats[i].Groups[2].Success)
                    {
                            if (ListdynamicTypeMat.Success)
                            {
                                field.isSimple = false;
                                field.isList = true;
                                field.typeName = "NFinal.DB.NList<__" + MethodName + "_" + field.varName + "__>";
                            }
                            else
                            {
                                field.isSimple = true;
                                field.typeName = addNewFieldMats[i].Groups[2].Value;
                                field.isNumber = IsNumber(field.typeName);
                            }
                    }
                    else
                    {
                        field.isSimple = false;
                        field.isList = false;
                        field.typeName = "__" + MethodName + "_" + field.varName + "__";
                    }
                    structFields.Add(field);
                }
            }
            return structFields;
        }
    }
}