using System.Collections.Generic;
using System.Linq;

namespace Workflow.BusinessCore.BusinessLayer.Helpers
{
    /// <summary>
    /// ConditionOperatorEnum enum.
    /// </summary>
    public enum ConditionOperatorEnum
    {
        EqualsTo,
        NotEqualsTo,
        LowerThan,
        LowerOrEquals,
        GreaterThan,
        GreaterOrEquals,
        StartWith,
        EndWith,
        Contains,
        NotContains
    }

    /// <summary>
    /// SortingEnum enum.
    /// </summary>
    public enum SortingEnum
    {
        Asc,
        Desc
    }

    /// <summary>
    /// FieldFormatEnum enum.
    /// </summary>
    public enum FieldFormatEnum
    {
        normal,
        Count,
        Sum,
        Avg,
        Min,
        Max
    }

    /// <summary>
    /// SqlRawBuilder class.
    /// Class describes all method for building a SQL raw.
    /// </summary>
    public class SqlRawBuilder
    {
        public SelectBuilder Select { get; set; }
        public CommandBuilder Command { get; set; }
        public string GetSQL { get; set; }
        public bool HasSelect { get; set; }
        public bool HasCondition { get; set; }
        public bool HasGroupBySelect { get; set; }
        public bool HasTable { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public SqlRawBuilder()
        {
            Select = new SelectBuilder(this);
            Command = new CommandBuilder(this);
        }
    }

    /// <summary>
    /// CommandBuilder class.
    /// </summary>
    public class CommandBuilder
    {
        private SqlRawBuilder sqlRawBuilder;

        /// <summary>
        /// Class constructor
        /// </summary>
        public CommandBuilder()
        {
        }

        public CommandBuilder(SqlRawBuilder sqlRawBuilder)
        {
            this.sqlRawBuilder = sqlRawBuilder;
        }

        public InsertBuilder Insert(string table, IDictionary<string, string> values)
        {
            return new InsertBuilder(sqlRawBuilder, table, values);
        }

        public DeleteBuilder Delete(string table)
        {
            return new DeleteBuilder(sqlRawBuilder, table);
        }

        public UpdateBuilder Update(string table, IDictionary<string, string> updates)
        {
            return new UpdateBuilder(sqlRawBuilder, table, updates);
        }
    }

    /// <summary>
    /// Delete class.
    /// </summary>
    public class DeleteBuilder
    {
        private SqlRawBuilder sqlRawBuilder;
        private string table;
        private IDictionary<string, string> values;

        /// <summary>
        /// Class constructor
        /// </summary>
        public DeleteBuilder()
        {
        }

        public DeleteBuilder(SqlRawBuilder sqlRawBuilder, string table)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.table = table;
        }

        public CommandWhere AddWhere()
        {
            return new CommandWhere(sqlRawBuilder);
        }
    }

    /// <summary>
    /// Insert class.
    /// </summary>
    public class InsertBuilder
    {
        private SqlRawBuilder sqlRawBuilder;
        private string table;
        private IDictionary<string, string> values;

        /// <summary>
        /// Class constructor
        /// </summary>
        public InsertBuilder()
        {
        }

        public InsertBuilder(SqlRawBuilder sqlRawBuilder, string table, IDictionary<string, string> values)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.table = table;
            this.values = values;
        }
    }

    /// <summary>
    /// Update class.
    /// </summary>
    public class UpdateBuilder
    {
        private SqlRawBuilder sqlRawBuilder;
        private string table;
        private IDictionary<string, string> updates;

        /// <summary>
        /// Class constructor
        /// </summary>
        public UpdateBuilder()
        {
        }

        public UpdateBuilder(SqlRawBuilder sqlRawBuilder, string table, IDictionary<string, string> updates)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.table = table;
            this.updates = updates;

            string updatesRaw = string.Empty;
            foreach (KeyValuePair<string, string> item in updates)
            {
                if (!string.IsNullOrEmpty(updatesRaw)) { updatesRaw += ", "; }
                updatesRaw += item.Key + "=" + item.Value;
            }

            this.sqlRawBuilder.GetSQL += "UPDATE " + table + updatesRaw;
        }

        public CommandWhere AddWhere()
        {
            return new CommandWhere(sqlRawBuilder);
        }
    }

    /// <summary>
    /// CommandWhere class.
    /// </summary>
    public class CommandWhere
    {
        private SqlRawBuilder sqlRawBuilder;

        /// <summary>
        /// Class constructor
        /// </summary>
        public CommandWhere()
        {
        }

        public CommandWhere(SqlRawBuilder sqlRawBuilder)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.sqlRawBuilder.GetSQL += " WHERE ";
        }

        public OrderBy AddOrder(string order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public OrCommand AddOr(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new OrCommand(sqlRawBuilder, field, conditionOperator, value);
        }

        public NotCommand AddNot(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new NotCommand(sqlRawBuilder, field, conditionOperator, value);
        }

        public AndCommand AddAnd(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new AndCommand(sqlRawBuilder, field, conditionOperator, value);
        }
    }

    /// <summary>
    /// AndCommand class.
    /// </summary>
    public class AndCommand
    {
        private SqlRawBuilder sqlRawBuilder;
        private string field;
        private ConditionOperatorEnum conditionOperator;
        private string value;

        /// <summary>
        /// Class constructor
        /// </summary>
        public AndCommand()
        {
        }

        public AndCommand(SqlRawBuilder sqlRawBuilder, string field, ConditionOperatorEnum conditionOperator, string value)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.field = field;
            this.conditionOperator = conditionOperator;
            this.value = value;

            if (this.sqlRawBuilder.HasCondition)
            {
                this.sqlRawBuilder.GetSQL += " AND ";
            }
            this.sqlRawBuilder.GetSQL += new FormatCondition(field, conditionOperator, value).Raw;
            this.sqlRawBuilder.HasCondition = true;
        }

        public OrderBy AddOrder(string order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public OrCommand AddOr(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new OrCommand(sqlRawBuilder, field, conditionOperator, value);
        }

        public AndCommand AddAnd(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new AndCommand(sqlRawBuilder, field, conditionOperator, value);
        }
    }

    /// <summary>
    /// NotCommand class.
    /// </summary>
    public class NotCommand
    {
        private SqlRawBuilder sqlRawBuilder;
        private string field;
        private ConditionOperatorEnum conditionOperator;
        private string value;

        /// <summary>
        /// Class constructor
        /// </summary>
        public NotCommand()
        {
        }

        public NotCommand(SqlRawBuilder sqlRawBuilder, string field, ConditionOperatorEnum conditionOperator, string value)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.field = field;
            this.conditionOperator = conditionOperator;
            this.value = value;

            if (this.sqlRawBuilder.HasCondition)
            {
                this.sqlRawBuilder.GetSQL += " NOT ";
            }
            this.sqlRawBuilder.GetSQL += new FormatCondition(field, conditionOperator, value).Raw;
            this.sqlRawBuilder.HasCondition = true;
        }

        public OrderBy AddOrder(string order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public OrCommand AddOr(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new OrCommand(sqlRawBuilder, field, conditionOperator, value);
        }

        public AndCommand AddAnd(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new AndCommand(sqlRawBuilder, field, conditionOperator, value);
        }
    }

    /// <summary>
    /// OrCommand class.
    /// </summary>
    public class OrCommand
    {
        private SqlRawBuilder sqlRawBuilder;
        private string field;
        private ConditionOperatorEnum conditionOperator;
        private string value;

        /// <summary>
        /// Class constructor
        /// </summary>
        public OrCommand()
        {
        }

        public OrCommand(SqlRawBuilder sqlRawBuilder, string field, ConditionOperatorEnum conditionOperator, string value)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.field = field;
            this.conditionOperator = conditionOperator;
            this.value = value;

            if (this.sqlRawBuilder.HasCondition)
            {
                this.sqlRawBuilder.GetSQL += " OR ";
            }
            this.sqlRawBuilder.GetSQL += new FormatCondition(field, conditionOperator, value).Raw;
            this.sqlRawBuilder.HasCondition = true;
        }

        public OrderBy AddOrder(string order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public OrCommand AddOr(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new OrCommand(sqlRawBuilder, field, conditionOperator, value);
        }

        public AndCommand AddAnd(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new AndCommand(sqlRawBuilder, field, conditionOperator, value);
        }
    }

    /// <summary>
    /// SelectBuilder class.
    /// </summary>
    public class SelectBuilder
    {
        private SqlRawBuilder sqlRawBuilder;

        /// <summary>
        /// Class constructor
        /// </summary>
        public SelectBuilder()
        {
        }

        public SelectBuilder(SqlRawBuilder sqlRawBuilder)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.sqlRawBuilder.GetSQL = "SELECT ";
        }

        public Field AddField(string field, FieldFormatEnum fieldFormatEnum = FieldFormatEnum.normal)
        {
            return new Field(sqlRawBuilder, field, fieldFormatEnum);
        }

        public Field AddFieldGroup(string[] fields, FieldFormatEnum fieldFormatEnum = FieldFormatEnum.normal)
        {
            return new Field(sqlRawBuilder, fields, fieldFormatEnum);
        }
    }

    /// <summary>
    /// Field class.
    /// </summary>
    public class Field
    {
        private SqlRawBuilder sqlRawBuilder;
        private string field;
        private string[] fields;

        /// <summary>
        /// Class constructor
        /// </summary>
        public Field()
        {
        }

        public Field(SqlRawBuilder sqlRawBuilder, string field, FieldFormatEnum fieldFormatEnum)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.field = field;

            string fieldFormat =
                (fieldFormatEnum == FieldFormatEnum.Count) ? string.Format("COUNT({0}) AS {0}", field)
                : (fieldFormatEnum == FieldFormatEnum.Sum) ? string.Format("SUM({0}) AS {0}", field)
                : (fieldFormatEnum == FieldFormatEnum.Avg) ? string.Format("AVG({0}) AS {0}", field)
                : (fieldFormatEnum == FieldFormatEnum.Min) ? string.Format("MIN({0}) AS {0}", field)
                : (fieldFormatEnum == FieldFormatEnum.Max) ? string.Format("MAX({0}) AS {0}", field)
                : field;

            this.sqlRawBuilder.GetSQL += (this.sqlRawBuilder.HasSelect ? ", " + fieldFormat : fieldFormat) + " ";
            this.sqlRawBuilder.HasSelect = true;
        }

        public Field(SqlRawBuilder sqlRawBuilder, string[] fields, FieldFormatEnum fieldFormatEnum)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.fields = fields;

            string fieldGroupFormat = string.Join(", ", fields.Select(s =>
            (fieldFormatEnum == FieldFormatEnum.Count) ? string.Format("COUNT({0}) AS {0}", s)
            : (fieldFormatEnum == FieldFormatEnum.Sum) ? string.Format("SUM({0}) AS {0}", s)
            : (fieldFormatEnum == FieldFormatEnum.Avg) ? string.Format("AVG({0}) AS {0}", s)
            : s));

            this.sqlRawBuilder.GetSQL += (this.sqlRawBuilder.HasSelect ? ", " + fieldGroupFormat : fieldGroupFormat) + " ";
            this.sqlRawBuilder.HasSelect = true;
        }

        public Field AddField(string field, FieldFormatEnum fieldFormatEnum = FieldFormatEnum.normal)
        {
            return new Field(sqlRawBuilder, field, fieldFormatEnum);
        }

        public Table AddTable(string table)
        {
            return new Table(sqlRawBuilder, table);
        }

        public InnerJoin AddInnerJoin(string table1, string tableId1, string table2, string tableId2)
        {
            return new InnerJoin(sqlRawBuilder, table1, tableId1, table2, tableId2);
        }

        public FullJoin AddFullJoin(string table1, string tableId1, string table2, string tableId2)
        {
            return new FullJoin(sqlRawBuilder, table1, tableId1, table2, tableId2);
        }

        public RightJoin AddRightJoin(string table1, string tableId1, string table2, string tableId2)
        {
            return new RightJoin(sqlRawBuilder, table1, tableId1, table2, tableId2);
        }

        public LeftJoin AddLeftJoin(string table1, string tableId1, string table2, string tableId2)
        {
            return new LeftJoin(sqlRawBuilder, table1, tableId1, table2, tableId2);
        }
    }

    /// <summary>
    /// InnerJoin class.
    /// Select records that have matching values in both tables.
    /// </summary>
    public class InnerJoin
    {
        private SqlRawBuilder sqlRawBuilder;
        private string table1;
        private string tableId1;
        private string table2;
        private string tableId2;

        /// <summary>
        /// Class constructor
        /// </summary>
        public InnerJoin()
        {
        }

        public InnerJoin(SqlRawBuilder sqlRawBuilder, string table1, string tableId1, string table2, string tableId2)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.table1 = table1;
            this.tableId1 = tableId1;
            this.table2 = table2;
            this.tableId2 = tableId2;
            this.sqlRawBuilder.GetSQL += (!sqlRawBuilder.HasTable ? string.Format("FROM {0} ", table1) : "") + " INNER JOIN " + table2 + " ON " + table1 + "." + tableId1 + " = " + table2 + "." + tableId2 + " ";

            sqlRawBuilder.HasTable = true;
        }

        public InnerJoin AddInnerJoin(string table1, string tableId1, string table2, string tableId2)
        {
            return new InnerJoin(sqlRawBuilder, table1, tableId1, table2, tableId2);
        }

        public OrderBy AddOrder(string[] order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public Where AddWhere()
        {
            return new Where(sqlRawBuilder);
        }

        public GroupBy AddGroupBy()
        {
            return new GroupBy(sqlRawBuilder);
        }
    }

    /// <summary>
    /// FullJoin class.
    /// Selects all records that match either left or right table records.
    /// </summary>
    public class FullJoin
    {
        private SqlRawBuilder sqlRawBuilder;
        private string table1;
        private string tableId1;
        private string table2;
        private string tableId2;

        /// <summary>
        /// Class constructor
        /// </summary>
        public FullJoin()
        {
        }

        public FullJoin(SqlRawBuilder sqlRawBuilder, string table1, string tableId1, string table2, string tableId2)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.table1 = table1;
            this.tableId1 = tableId1;
            this.table2 = table2;
            this.tableId2 = tableId2;
            this.sqlRawBuilder.GetSQL += (!sqlRawBuilder.HasTable ? string.Format("FROM {0} ", table1) : "") + " FULL JOIN " + table2 + "  ON " + table1 + "." + tableId1 + " = " + table2 + "." + tableId2 + " ";

            sqlRawBuilder.HasTable = true;
        }

        public FullJoin AddInnerJoin(string table1, string tableId1, string table2, string tableId2)
        {
            return new FullJoin(sqlRawBuilder, table1, tableId1, table2, tableId2);
        }

        public OrderBy AddOrder(string[] order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public Where AddWhere()
        {
            return new Where(sqlRawBuilder);
        }

        public GroupBy AddGroupBy()
        {
            return new GroupBy(sqlRawBuilder);
        }
    }

    /// <summary>
    /// RightJoin class.
    /// Select records from the second (right-most) table with matching left table records.
    /// </summary>
    public class RightJoin
    {
        private SqlRawBuilder sqlRawBuilder;
        private string table1;
        private string tableId1;
        private string table2;
        private string tableId2;

        /// <summary>
        /// Class constructor
        /// </summary>
        public RightJoin()
        {
        }

        public RightJoin(SqlRawBuilder sqlRawBuilder, string table1, string tableId1, string table2, string tableId2)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.table1 = table1;
            this.tableId1 = tableId1;
            this.table2 = table2;
            this.tableId2 = tableId2;
            this.sqlRawBuilder.GetSQL += (!sqlRawBuilder.HasTable ? string.Format("FROM {0} ", table1) : "") + " RIGHT JOIN " + table2 + " ON " + table1 + "." + tableId1 + " = " + table2 + "." + tableId2 + " ";

            sqlRawBuilder.HasTable = true;
        }

        public RightJoin AddRightJoin(string table1, string tableId1, string table2, string tableId2)
        {
            return new RightJoin(sqlRawBuilder, table1, tableId1, table2, tableId2);
        }

        public OrderBy AddOrder(string[] order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public Where AddWhere()
        {
            return new Where(sqlRawBuilder);
        }

        public GroupBy AddGroupBy()
        {
            return new GroupBy(sqlRawBuilder);
        }
    }

    /// <summary>
    /// LeftJoin class.
    /// Select records from the first (left-most) table with matching right table records.
    /// </summary>
    public class LeftJoin
    {
        private SqlRawBuilder sqlRawBuilder;
        private string table1;
        private string tableId1;
        private string table2;
        private string tableId2;

        /// <summary>
        /// Class constructor
        /// </summary>
        public LeftJoin()
        {
        }

        public LeftJoin(SqlRawBuilder sqlRawBuilder, string table1, string tableId1, string table2, string tableId2)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.table1 = table1;
            this.tableId1 = tableId1;
            this.table2 = table2;
            this.tableId2 = tableId2;
            this.sqlRawBuilder.GetSQL += (!sqlRawBuilder.HasTable ? string.Format("FROM {0} ", table1) : "") + "LEFT JOIN " + table2 + " ON " + table1 + "." + tableId1 + " = " + table2 + "." + tableId2 + " ";

            sqlRawBuilder.HasTable = true;
        }

        public LeftJoin AddLeftJoin(string table1, string tableId1, string table2, string tableId2)
        {
            return new LeftJoin(sqlRawBuilder, table1, tableId1, table2, tableId2);
        }

        public OrderBy AddOrder(string[] order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public Where AddWhere()
        {
            return new Where(sqlRawBuilder);
        }

        public GroupBy AddGroupBy()
        {
            return new GroupBy(sqlRawBuilder);
        }
    }

    /// <summary>
    /// Table class.
    /// </summary>
    public class Table
    {
        private SqlRawBuilder sqlRawBuilder;
        private string table;

        /// <summary>
        /// Class constructor
        /// </summary>
        public Table()
        {
        }

        public Table(SqlRawBuilder sqlRawBuilder, string table)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.table = table;
            this.sqlRawBuilder.GetSQL += " FROM " + table;
        }

        public OrderBy AddOrder(string order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public OrderBy AddOrder(string[] orders, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, orders, sortingEnum);
        }

        public Where AddWhere()
        {
            return new Where(sqlRawBuilder);
        }

        public GroupBy AddGroupBy()
        {
            return new GroupBy(sqlRawBuilder);
        }
    }

    /// <summary>
    /// GroupBy class.
    /// </summary>
    public class GroupBy
    {
        private SqlRawBuilder sqlRawBuilder;

        /// <summary>
        /// Class constructor
        /// </summary>
        public GroupBy()
        {
        }

        public GroupBy(SqlRawBuilder sqlRawBuilder)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.sqlRawBuilder.GetSQL += " GROUP BY ";
        }

        public GroupField AddField(string field)
        {
            return new GroupField(sqlRawBuilder, field);
        }

        public GroupField AddFieldGroup(string[] fields)
        {
            return new GroupField(sqlRawBuilder, fields);
        }
    }

    /// <summary>
    /// GroupField class.
    /// </summary>
    public class GroupField
    {
        private SqlRawBuilder sqlRawBuilder;
        private string[] fields;
        private string field;

        /// <summary>
        /// Class constructor
        /// </summary>
        public GroupField()
        {
        }

        public GroupField(SqlRawBuilder sqlRawBuilder, string field)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.field = field;

            this.sqlRawBuilder.GetSQL += (this.sqlRawBuilder.HasGroupBySelect ? ", " + field : field);
            this.sqlRawBuilder.HasGroupBySelect = true;
        }

        public GroupField(SqlRawBuilder sqlRawBuilder, string[] fields)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.fields = fields;

            string fieldGroupFormat = string.Join(", ", fields.Select(s => s));
            this.sqlRawBuilder.GetSQL += (this.sqlRawBuilder.HasGroupBySelect ? ", " + fieldGroupFormat : fieldGroupFormat);
            this.sqlRawBuilder.HasGroupBySelect = true;
        }

        public Having AddHaving()
        {
            return new Having(sqlRawBuilder);
        }
    }

    /// <summary>
    /// Having class.
    /// </summary>
    public class Having
    {
        private SqlRawBuilder sqlRawBuilder;

        /// <summary>
        /// Class constructor
        /// </summary>
        public Having()
        {
        }

        public Having(SqlRawBuilder sqlRawBuilder)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.sqlRawBuilder.GetSQL += " HAVING ";
        }

        public OrderBy AddOrder(string order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public Or AddOr(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new Or(sqlRawBuilder, field, conditionOperator, value);
        }

        public Not AddNot(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new Not(sqlRawBuilder, field, conditionOperator, value);
        }

        public And AddAnd(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new And(sqlRawBuilder, field, conditionOperator, value);
        }
    }

    /// <summary>
    /// Where class
    /// </summary>
    public class Where
    {
        private SqlRawBuilder sqlRawBuilder;

        /// <summary>
        /// Class constructor
        /// </summary>
        public Where()
        {
        }

        public Where(SqlRawBuilder sqlRawBuilder)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.sqlRawBuilder.GetSQL += " WHERE ";
        }

        public Or AddOr(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new Or(sqlRawBuilder, field, conditionOperator, value);
        }

        public Not AddNot(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new Not(sqlRawBuilder, field, conditionOperator, value);
        }

        public And AddAnd(string field, ConditionOperatorEnum conditionOperator, string value)
        {
            return new And(sqlRawBuilder, field, conditionOperator, value);
        }
    }

    /// <summary>
    /// Or class.
    /// </summary>
    public class Or
    {
        private SqlRawBuilder sqlRawBuilder;
        private string field;

        /// <summary>
        /// Class constructor
        /// </summary>
        public Or()
        {
        }

        public Or(SqlRawBuilder sqlRawBuilder, string field, ConditionOperatorEnum operatorEnum, string value)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.field = field;

            if (this.sqlRawBuilder.HasCondition)
            {
                this.sqlRawBuilder.GetSQL += " OR ";
            }
            this.sqlRawBuilder.GetSQL += new FormatCondition(field, operatorEnum, value).Raw;
            this.sqlRawBuilder.HasCondition = true;
        }

        public OrderBy AddOrder(string order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public And AddAnd(string field, ConditionOperatorEnum operatorEnum, string value)
        {
            return new And(sqlRawBuilder, field, operatorEnum, value);
        }

        public Or AddOr(string field, ConditionOperatorEnum operatorEnum, string value)
        {
            return new Or(sqlRawBuilder, field, operatorEnum, value);
        }
    }

    /// <summary>
    /// Not class.
    /// </summary>
    public class Not
    {
        private SqlRawBuilder sqlRawBuilder;
        private string field;

        /// <summary>
        /// Class constructor
        /// </summary>
        public Not()
        {
        }

        public Not(SqlRawBuilder sqlRawBuilder, string field, ConditionOperatorEnum operatorEnum, string value)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.field = field;

            if (this.sqlRawBuilder.HasCondition)
            {
                this.sqlRawBuilder.GetSQL += " NOT ";
            }
            this.sqlRawBuilder.GetSQL += (new FormatCondition(field, operatorEnum, value)).Raw;
            this.sqlRawBuilder.HasCondition = true;
        }

        public OrderBy AddOrder(string order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public Or AddOr(string field, ConditionOperatorEnum operatorEnum, string value)
        {
            return new Or(sqlRawBuilder, field, operatorEnum, value);
        }

        public And AddAnd(string field, ConditionOperatorEnum operatorEnum, string value)
        {
            return new And(sqlRawBuilder, field, operatorEnum, value);
        }
    }

    public class And
    {
        private SqlRawBuilder sqlRawBuilder;
        private string field;

        /// <summary>
        /// Class constructor
        /// </summary>
        public And()
        {
        }

        public And(SqlRawBuilder sqlRawBuilder, string field, ConditionOperatorEnum operatorEnum, string value)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.field = field;

            if (this.sqlRawBuilder.HasCondition)
            {
                this.sqlRawBuilder.GetSQL += " AND ";
            }
            this.sqlRawBuilder.GetSQL += (new FormatCondition(field, operatorEnum, value)).Raw;
            this.sqlRawBuilder.HasCondition = true;
        }

        public OrderBy AddOrder(string order, SortingEnum sortingEnum)
        {
            return new OrderBy(sqlRawBuilder, order, sortingEnum);
        }

        public Or AddOr(string field, ConditionOperatorEnum operatorEnum, string value)
        {
            return new Or(sqlRawBuilder, field, operatorEnum, value);
        }

        public And AddAnd(string field, ConditionOperatorEnum operatorEnum, string value)
        {
            return new And(sqlRawBuilder, field, operatorEnum, value);
        }
    }

    public class OrderBy
    {
        private SqlRawBuilder sqlRawBuilder;
        private string order;
        private string[] orders;

        /// <summary>
        /// Class constructor
        /// </summary>
        public OrderBy()
        {
        }

        public OrderBy(SqlRawBuilder sqlRawBuilder, string order, SortingEnum sortingEnum = SortingEnum.Asc)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.order = order;
            this.sqlRawBuilder.GetSQL += " ORDER BY " + order + (sortingEnum == SortingEnum.Desc ? " DESC" : "");
        }

        public OrderBy(SqlRawBuilder sqlRawBuilder, string[] orders, SortingEnum sortingEnum = SortingEnum.Asc)
        {
            this.sqlRawBuilder = sqlRawBuilder;
            this.orders = orders;
            this.sqlRawBuilder.GetSQL += " ORDER BY " + string.Join(", ", orders) + (sortingEnum == SortingEnum.Desc ? " DESC" : "");
        }
    }

    /// <summary>
    /// FormatCondition class.
    /// </summary>
    public class FormatCondition
    {
        public string Raw { get; set; }
        private string field;
        private ConditionOperatorEnum operatorEnum;
        private string value;

        /// <summary>
        /// Class constructor
        /// </summary>
        public FormatCondition()
        {
        }

        public FormatCondition(string field, ConditionOperatorEnum operatorEnum, string value)
        {
            this.field = field;
            this.operatorEnum = operatorEnum;
            this.value = value;

            switch (operatorEnum)
            {
                case ConditionOperatorEnum.EqualsTo:
                    Raw = field + " = '" + value + "'"
                    ; break;
                case ConditionOperatorEnum.GreaterThan:
                    Raw = field + " > '" + value + "'"
                    ; break;
                case ConditionOperatorEnum.GreaterOrEquals:
                    Raw = field + " >= '" + value + "'"
                    ; break;
                case ConditionOperatorEnum.LowerThan:
                    Raw = field + " < '" + value + "'"
                    ; break;
                case ConditionOperatorEnum.LowerOrEquals:
                    Raw = field + " <= '" + value + "'"
                    ; break;
                case ConditionOperatorEnum.NotEqualsTo:
                    Raw = field + " <> '" + value + "'"
                    ; break;
                case ConditionOperatorEnum.StartWith:
                    Raw = field + " LIKE '" + value + "%'"
                    ; break;
                case ConditionOperatorEnum.EndWith:
                    Raw = field + " LIKE '%" + value + "'"
                    ; break;
                case ConditionOperatorEnum.Contains:
                    Raw = field + " LIKE '%" + value + "%'"
                    ; break;
                case ConditionOperatorEnum.NotContains:
                    Raw = field + " NOT LIKE '%" + value + "%'"
                    ; break;
            }
        }
    }
}
