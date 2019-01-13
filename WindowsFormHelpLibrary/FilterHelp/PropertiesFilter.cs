using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;
using DynamicExpression =  System.Linq.Dynamic.DynamicExpression;
using TypeFilter = WindowsFormHelpLibrary.SortableBindingList.TypeFilter;

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

        public PropertiesFilter(Type source)
        {
            var properties = TypeDescriptor.GetProperties(source);
            int i = 0;
            foreach (PropertyDescriptor propertyDescriptor in properties)
            {
                Add(new PropertyValidate(propertyDescriptor.PropertyType, (i++, propertyDescriptor.Name)));
            }
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

        private static MethodInfo ValidatorExpressionInfo { get; } = typeof(PropertiesFilter).GetMethod(nameof(ValidatorExpression), BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo TypeValidatorExpressionInfo { get; } = typeof(PropertiesFilter).GetMethod(nameof(TypeValidatorExpression), BindingFlags.NonPublic | BindingFlags.Static);
        private static MethodInfo CompileInfo { get; } = typeof(LambdaExpression).GetMethod(nameof(LambdaExpression.Compile), new Type[0]);
        private static LambdaExpression ValidatorExpression(Type srcType, string filter, params object[] args)
        {
            return DynamicExpression.ParseLambda(srcType, typeof(bool), filter, args);
        }

        //todo ввести визитёра
        public static Func<T, bool> CreateFilter<T>(TypeFilter filter)
        {
            string tempFilter = "1 == 1";
            int i = 0;
            var list = new List<object>(2);
            LambdaExpression propertyValidator;
            if (!String.IsNullOrEmpty(filter.Filter))
            {
                tempFilter += $" && @{i++}(it)";
                propertyValidator = ValidatorExpression(typeof(T), filter.Filter, FilterExpression);
                list.Add(propertyValidator);
            }
            if (filter.PropertyFilters != null && filter.PropertyFilters.Count != 0)
            {
                tempFilter += $" && @{i}(it)";
                propertyValidator = TypeValidatorExpression(typeof(T), filter.PropertyFilters);
                list.Add(propertyValidator);
            }
            return DynamicExpression.ParseLambda<T, bool>(tempFilter, list.ToArray()).Compile();
        }
        private static LambdaExpression TypeValidatorExpression(Type srcType, IDictionary<string, TypeFilter> filters)
        {
            //todo Add
            string tempFilter = "1 == 1";
            int i = 0;
            var list = new List<object>(filters.Count);
            foreach (var propertyFilter in filters)
            {
                LambdaExpression propertyValidator;
                if (!String.IsNullOrEmpty(propertyFilter.Value.Filter))
                {
                    tempFilter += $" && @{i++}(it)";
                    propertyValidator = GetPropertyStringValidator(srcType, propertyFilter.Key, propertyFilter.Value.Filter, FilterExpression);
                    list.Add(propertyValidator);
                }
                if(propertyFilter.Value.PropertyFilters != null && propertyFilter.Value.PropertyFilters.Count != 0)
                {
                    tempFilter += $" && @{i++}(it)";
                    propertyValidator = GetPropertyTypeValidator(srcType, propertyFilter.Key,
                        propertyFilter.Value.PropertyFilters);
                    list.Add(propertyValidator);
                }
            }
            return DynamicExpression.ParseLambda(srcType, typeof(bool), tempFilter, list.ToArray());
        }

        private static LambdaExpression GetPropertyTypeValidator(Type srcType, string propertyName, IDictionary<string, TypeFilter> filters)
        {
            var parameter = Expression.Parameter(srcType);
            var property = Expression.Property(parameter, propertyName);
            var propertyTypeConst = Expression.Constant(property.Type, typeof(Type));
            var filterConst = Expression.Constant(filters, typeof(IDictionary<string, TypeFilter>));
            var validatorExpression = Expression.Call(TypeValidatorExpressionInfo, propertyTypeConst, filterConst);
            var compileInfo = CompileInfo;
            var delegateExpression = Expression.Call(validatorExpression, compileInfo);
            var propertyArray = Expression.NewArrayInit(typeof(object), property);
            var invokeInfo = Expression.Call(delegateExpression, nameof(Delegate.DynamicInvoke), null, propertyArray);
            var resultConvert = Expression.Convert(invokeInfo, typeof(bool));
            return Expression.Lambda(resultConvert, parameter);
        }

        private static LambdaExpression GetPropertyStringValidator(Type srcType, string propertyName, string filter, params object[] args)
        {
            var parameter = Expression.Parameter(srcType);
            var property = Expression.Property(parameter, propertyName);
            var propertyTypeConst = Expression.Constant(property.Type, typeof(Type));
            var filterConst = Expression.Constant(filter, typeof(string));
            var argsConst = Expression.Constant(args, typeof(object[]));
            var validatorExpression = Expression.Call(ValidatorExpressionInfo, propertyTypeConst, filterConst, argsConst);
            var compileInfo = CompileInfo;
            var delegateExpression = Expression.Call(validatorExpression, compileInfo);
            var propertyArray = Expression.NewArrayInit(typeof(object), property);
            var invokeInfo = Expression.Call(delegateExpression, nameof(Delegate.DynamicInvoke), null, propertyArray);
            var resultConvert = Expression.Convert(invokeInfo, typeof(bool));
            return Expression.Lambda(resultConvert, parameter);
        }

        private static Expression<Func<string, string, bool>> FilterExpression { get; } = (value, pattern) => !String.IsNullOrEmpty(value) && Regex.IsMatch(value, pattern);

    }
}