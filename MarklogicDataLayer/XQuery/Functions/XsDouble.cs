namespace MarklogicDataLayer.XQuery.Functions
{
    public class XsDouble : Function
    {
        readonly string _value;

        public XsDouble(string value)
        {
            _value = value;
        }

        public override string Query
        {
            get
            {
                return FunctionToQuery(
                    "xs:double",
                    _value);
            }
        }
    }
}
