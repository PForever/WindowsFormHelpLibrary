using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormHelpLibrary
{
    public partial class DigitalIntervalPicker : Form
    {
        private const int TypesCount = 7;
        private static readonly Type[] IntTypes = new Type[] { typeof(byte), typeof(short), typeof(int), typeof(long) };
        private static readonly Type[] ComaTypes = new Type[] { typeof(decimal), typeof(float), typeof(double) };
        private static Dictionary<Type, object> _maxValues;
        private static Dictionary<Type, object> _minValues = new Dictionary<Type, object>(TypesCount);


        public decimal From => nudFrom.Value;
        public decimal To => nudTo.Value;

        static DigitalIntervalPicker()
        {
            _maxValues = new Dictionary<Type, object>
            {
                {typeof(byte), byte.MaxValue },
                {typeof(short), short.MaxValue },
                {typeof(int), int.MaxValue },
                {typeof(long), long.MaxValue },
                {typeof(decimal), decimal.MaxValue },
                {typeof(float), float.MaxValue },
                {typeof(double), double.MaxValue },
            };
            _minValues = new Dictionary<Type, object>
            {
                {typeof(byte), byte.MinValue },
                {typeof(short), short.MinValue },
                {typeof(int), int.MinValue },
                {typeof(long), long.MinValue },
                {typeof(decimal), decimal.MinValue },
                {typeof(float), float.MinValue },
                {typeof(double), double.MinValue },
            };
        }
        public DigitalIntervalPicker(Type type)
        {
            InitializeComponent();
            if (IntTypes.Contains(type))
            {
                nudFrom.DecimalPlaces = 0;
                nudTo.DecimalPlaces = 0;
            }
            else if (!ComaTypes.Contains(type)) throw new ArgumentException("Parameter is not a decimal type", nameof(type));

            nudFrom.Maximum = Convert.ToDecimal(_maxValues[type]);
            nudTo.Maximum = Convert.ToDecimal(_maxValues[type]);
            nudFrom.Minimum = Convert.ToDecimal(_minValues[type]);
            nudTo.Minimum = Convert.ToDecimal(_minValues[type]);
        }

        private void OnOk(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnDelete(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
