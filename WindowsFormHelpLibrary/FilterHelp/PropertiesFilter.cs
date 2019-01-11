using System.Collections.Generic;
using System.Linq;

namespace WindowsFormHelpLibrary.FilterHelp
{
    public class PropertiesFilter : Dictionary<int, PropertyValidate>
    {
        public PropertiesFilter()
        {
            
        }
        public PropertiesFilter(IList<PropertyValidate> filters) : base(filters.ToDictionary(f => f.Key.Position, f => f))
        {

        }
        public PropertiesFilter(IEnumerable<PropertyValidate> filters) : base(filters.ToDictionary(f => f.Key.Position, f => f))
        {

        }

        public static string GetFilterExpression(
            params (string propertyName, string pattern)[] values)
        {
            string filter = "1 == 1";
            int i = values.Length - 1;
            foreach (var (propertyName, pattern) in values)
            {
                filter += $" && iif(it.{propertyName} != null, @0(it.{propertyName}.ToString(), \"{pattern}\"), false)";
            }
            return filter;
        }

        public (string propertyName, string pattern)[] GetFilterParameters()
        {
            var values = new List<(string propertyName, string pattern)>(Count);
            foreach (var current in this.Select(kvp => kvp.Value).Where(f => f.Value != null))
            {
                values.Add((current.Key.Name, GetPattern(current.Value.ToString())));
            }
            return values.ToArray();
        }

        public override string ToString() => GetFilterExpression(GetFilterParameters());

        private string GetPattern(string valueValue)
        {
            string pattern = "";
            foreach (char c in valueValue) pattern += $"[{c}]+.*";
            return pattern;
        }

        public void Add(PropertyValidate value)
        {
            base.Add(value.Key.Position, value);
        }
    }
}