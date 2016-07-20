using System;
using System.Collections.Generic;
using System.Linq;

namespace K2Facade
{
    public class SqlFilter : IDisposable
    {
        public string Field { get; set; }
        public string VariableName { get; set; }
        public List<object> Values { get; set; }
        public FilterOperators Operator { get; set; }
        public int Group { get; set; }

        public SqlFilter()
        {
            Values = new List<object>();
        }

        public SqlFilter(string field, object value)
        {
            Field = field;
            VariableName = "@" + field;
            Values = new List<object>();
            Values.Add(value);
            Operator = FilterOperators.Equal;
            Group = 0;
        }

        public SqlFilter(string field, object value, FilterOperators opr)
        {
            Field = field;
            VariableName = "@" + field;
            Values = new List<object>();
            Values.Add(value);
            Operator = opr;
            Group = 0;
        }

        public SqlFilter(string field, object value, FilterOperators opr, string variableName)
        {
            Field = field;
            VariableName = "@" + variableName;
            Values = new List<object>();
            Values.Add(value);
            Operator = opr;
            Group = 0;
        }

        public SqlFilter(string field, object value, FilterOperators opr, string variableName, int group)
        {
            Field = field;
            VariableName = "@" + variableName;
            Values = new List<object>();
            Values.Add(value);
            Operator = opr;
            Group = group;
        }

        public SqlFilter(string field, object[] values)
        {
            Field = field;
            VariableName = "@" + field;
            Values = new List<object>();
            if (values != null && values.Count() > 0) Values.AddRange(values);
            Operator = FilterOperators.Equal;
            Group = 0;
        }

        public SqlFilter(string field, object[] values, FilterOperators opr)
        {
            Field = field;
            VariableName = "@" + field;
            Values = new List<object>();
            if (values != null && values.Count() > 0) Values.AddRange(values);
            Operator = opr;
            Group = 0;
        }

        public SqlFilter(string field, object[] values, FilterOperators opr, string variableName)
        {
            Field = field;
            VariableName = "@" + variableName;
            Values = new List<object>();
            if (values != null && values.Count() > 0) Values.AddRange(values);
            Operator = opr;
            Group = 0;
        }

        public SqlFilter(string field, object[] values, FilterOperators opr, string variableName, int group)
        {
            Field = field;
            VariableName = "@" + variableName;
            Values = new List<object>();
            if (values != null && values.Count() > 0) Values.AddRange(values);
            Operator = opr;
            Group = group;
        }

        public string GetOperator()
        {
            switch (Operator)
            {
                case FilterOperators.Equal: return " = " + VariableName;
                case FilterOperators.NotEqual: return " <> " + VariableName;
                case FilterOperators.LessThan: return " < " + VariableName;
                case FilterOperators.LessThanOrEqual: return " <= " + VariableName;
                case FilterOperators.GreaterThan: return " > " + VariableName;
                case FilterOperators.GreaterThanOrEqual: return " >= " + VariableName;
                case FilterOperators.Like: return " like '%' + " + VariableName + " + '%'";
                case FilterOperators.NotLike: return " not like '%' + " + VariableName + " + '%'";
                case FilterOperators.In:
                    var flt = " in (";
                    if (Values.Count == 1)
                        flt += VariableName;
                    else
                        for (int i = 0; i < Values.Count; i++)
                        {
                            flt += VariableName + "_" + i;
                            if (i + 1 < Values.Count) flt += ", ";
                        }
                    flt += ")";
                    return flt;
                case FilterOperators.NotIn:
                    var flt2 = " not in (";
                    if (Values.Count == 1)
                        flt2 += VariableName;
                    else
                    for (int i = 0; i < Values.Count; i++)
                    {
                        flt2 += VariableName + "_" + i;
                        if (i + 1 < Values.Count) flt2 += ", ";
                    }
                    flt2 += ")";
                    return flt2;
                case FilterOperators.Between:
                    return " between " + VariableName + "_1 and " + VariableName + "_2";
                case FilterOperators.NotBetween:
                    return " between " + VariableName + "_1 and " + VariableName + "_2";
                case FilterOperators.IsNull: return " is null";
                case FilterOperators.IsNotNull: return " is not null";
                default: return " = ";
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
