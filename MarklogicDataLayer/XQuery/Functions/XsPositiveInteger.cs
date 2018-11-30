namespace MarklogicDataLayer.XQuery.Functions
{
    public class XsInteger : Function
    {
        readonly string _value;

        public XsInteger(string value)
        {
            _value = value;
        }

        public override string Query
        {
            get
            {
                return FunctionToQuery(
                    "xs:integer",
                    _value);
            }
        }
    }
}
