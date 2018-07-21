using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MarklogicDataLayer.XQuery
{
    public class Flwor : Expression
    {
        readonly List<string> _queryList;

        public Flwor(params Expression[] queries)
        {
            var stringQueries = queries.Select(q => q.Query);
            _queryList = new List<string>(stringQueries);
        }

        public string DefaultNamespace
        {
            get; set;
        }


        public override string Query
        {
            get
            {
                var qb = new StringBuilder();

                foreach (var queryLine in _queryList)
                {
                    qb.AppendLine(queryLine);
                }

                return qb.ToString();
            }
        }
        public Flwor For(VariableName itemName, Expression expression)
        {
            var clause = new ForClause(itemName, expression);
            _queryList.Add(clause.Query);

            return this;
        }

        public Flwor Let(string variableName, Expression exp)
        {
            return Let(new VariableName(variableName), exp);
        }

        public Flwor Let(VariableName variableName, Expression exp)
        {
            var clause = new LetClause(variableName, exp);
            _queryList.Add(clause.Query);
            return this;
        }

        public Flwor Return(VariableName variableName)
        {
            var clause = new ReturnClause(variableName);
            _queryList.Add(clause.Query);
            return this;
        }

        public Flwor Return(Expression expression)
        {
            var clause = new ReturnClause(expression);
            _queryList.Add(clause.Query);
            return this;
        }

        public Flwor Return(XElement xmlStructure)
        {
            var clause = new ReturnClause(xmlStructure);
            _queryList.Add(clause.Query);
            return this;
        }

        public Flwor Where(string expression)
        {
            var clause = new WhereClause(expression);
            _queryList.Add(clause.Query);

            return this;
        }
    }
}
