using System;

namespace WindowsFormHelpLibrary.FilterHelp
{
    public class PropertyValidate
    {
        public PropertyNamePosition Key { get; }
        public Type PropertyType { get; }
        public Delegate Validator { get; }
        public object Value { get; set; }

        public PropertyValidate(Delegate validator, object value, Type propertyType, PropertyNamePosition key)
        {
            Validator = validator ?? throw new ArgumentNullException(nameof(validator));
            Value = value;
            PropertyType = propertyType;
            Key = key;
        }

        public PropertyValidate(Delegate validator, Type propertyType, PropertyNamePosition key) : this((Func<object, bool>)(v => true), default, propertyType, key)
        {
        }

        public PropertyValidate(object value, Type propertyType, PropertyNamePosition key) : this((Func<object, bool>) (v => true), value, propertyType, key)
        {
        }
        public PropertyValidate(Type propertyType, PropertyNamePosition key) : this((Func<object, bool>)(v => true), null, propertyType, key)
        {
            PropertyType = propertyType;
        }

        public PropertyValidate(PropertyNamePosition key) : this((Func<object, bool>)(v => true), null, typeof(object), key) { }
    }
}