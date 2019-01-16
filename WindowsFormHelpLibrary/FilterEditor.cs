using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WindowsFormHelpLibrary.FilterHelp;

namespace WindowsFormHelpLibrary
{
    public partial class FilterEditor : Form
    {
        private readonly PropertiesFilter _filters;
        private static readonly PropertyNamePosition NonProperty = new PropertyNamePosition(-1, "");
        public FilterEditor(PropertiesFilter filters)
        {
            _filters = filters ?? throw new ArgumentNullException(nameof(filters));
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            var list = _filters.Select(f => f.Value.Key).ToList();
            list.Add(NonProperty);
            bsProperties.DataSource = list;
            bsFilters.DataSource = _filters.Where(f => f.Value.Value != null).Select(f => new KvP(f.Value.Key.Position, f.Value.Value)).ToList();
            base.OnLoad(e);
        }

        private void OnOk(object sender, EventArgs e)
        {
            foreach (var kvP in bsFilters.Cast<KvP>())
            {
                _filters[kvP.Position].Value = kvP.Value;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void OnAllDelete(object sender, EventArgs e)
        {
            foreach (var kvP in Enumerable.Cast<KvP>(bsFilters))
            {
                _filters[kvP.Position].Value = null;
            }
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnAddingFilter(object sender, AddingNewEventArgs e)
        {
            var positions = ((IList<KvP>) bsFilters.List).Select(kvp => kvp.Position).ToArray();
            e.NewObject = _filters.Where(f => !positions.Contains(f.Key))
                    .Select(f => new KvP(f.Key, f.Value.Value)).First();
            if(bsFilters.Count == _filters.Count - 1)
            {
                dgvFilters.AllowUserToAddRows = false;
            }
        }

        private void OnFilterCellContentClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == btFilterDelete.Index && e.RowIndex >= 0)
            {
                var row = dgvFilters.Rows[e.RowIndex];
                if(row.IsNewRow) return;
                _filters[((KvP)row.DataBoundItem).Position].Value = null;
                dgvFilters.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void OnFiltersChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemDeleted)
            {
                dgvFilters.AllowUserToAddRows = true;
            }
        }

        private void OnFilterCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == cbName.Index)
            {
                var cb = (ComboBox) dgvFilters.EditingControl;
                var filters = (IList<KvP>)bsFilters.List;
                int currentPosition = (int)cb.SelectedValue;
                bool any = false;
                for (var i = 0; i < filters.Count; i++)
                {
                    if (filters[i].Position == currentPosition && i != bsFilters.Position)
                    {
                        any = true;
                        break;
                    }
                }
                if (!any) return;
                MessageBox.Show($"Фильтер столбца {e.FormattedValue} уже добавлен", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cb.DroppedDown = true;
                e.Cancel = true;
            }
        }
        private sealed class KvP
        {
            public int Position { get; set; }
            public object Value { get; set; }
            public KvP(int position, object value)
            {
                Position = position;
                Value = value;
            }
            public KvP()
            {
                Position = -1;
            }
        }

        private void OnFilterCellDoubleClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == tbValue.Index)
            {
                var property = (KvP) bsFilters[e.RowIndex];
                var propertyInfo = _filters[property.Position];
                var type = propertyInfo.PropertyType;
                if(type == typeof(DateTime)) CreateDateTimeFilter(propertyInfo, property);
                else if(DigitalTypes.Contains(type)) CreateDigitalTimeFilter(propertyInfo, property);
                else if (type.IsPrimitive || type == typeof(string) || propertyInfo.SourceList != null) return;
                else CreateInnerFilter(propertyInfo, property);
            }
        }

        private void CreateDigitalTimeFilter(PropertyValidate propertyInfo, KvP property)
        {
            Func<decimal, bool> Between(decimal from, decimal to) => src => from <= src && src <= to;
            var intervalPicker = new DigitalIntervalPicker(propertyInfo.PropertyType);
            switch (intervalPicker.ShowDialog())
            {
                case DialogResult.OK:
                    property.Value = Between(intervalPicker.From, intervalPicker.To);
                    bsFilters.ResetCurrentItem();
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Abort:
                    property.Value = null;
                    bsFilters.ResetCurrentItem();
                    break;
            }
        }

        private static readonly Type[] DigitalTypes = new Type[] { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(decimal), typeof(float), typeof(double) };
        private void CreateDateTimeFilter(PropertyValidate propertyInfo, KvP property)
        {
            Func<DateTime, bool> Between(DateTime from, DateTime to) => src => from <= src && src <= to;
            var intervalPicker = new IntervalPicker();
            switch (intervalPicker.ShowDialog())
            {
                case DialogResult.OK:
                    property.Value = Between(intervalPicker.From, intervalPicker.To);
                    bsFilters.ResetCurrentItem();
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Abort:
                    property.Value = null;
                    bsFilters.ResetCurrentItem();
                    break;
            }
        }

        private void CreateInnerFilter(PropertyValidate propertyInfo, KvP property)
        {
            var filter = new PropertiesFilter(propertyInfo.PropertyType);
            switch (new FilterEditor(filter).ShowDialog())
            {
                case DialogResult.OK:
                    property.Value = filter;
                    bsFilters.ResetCurrentItem();
                    break;
                case DialogResult.Cancel:
                    break;
                case DialogResult.Abort:
                    property.Value = null;
                    bsFilters.ResetCurrentItem();
                    break;
            }
        }

        private void OnFilterCellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == tbValue.Index)
            {
                var row = dgvFilters.Rows[e.RowIndex];
                if (row.IsNewRow) return;
                var property = (KvP)row.DataBoundItem;
                var propertyInfo = _filters[property.Position];
                var type = propertyInfo.PropertyType;
                if (type.IsPrimitive && !DigitalTypes.Contains(type) || type == typeof(string) || propertyInfo.SourceList != null) return;
                e.Value = property.Value != null ? "ФИЛЬТР УСТАНОВЛЕН" : "ФИЛЬТР НЕ УСТАНОВЛЕН";
                e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Italic);
                e.CellStyle.BackColor = Color.Orange;
                e.CellStyle.SelectionBackColor = Color.Red;
                var cell = row.Cells[e.ColumnIndex];
                e.FormattingApplied = true;
            }
        }
        private bool CheckType(Type type) => type.IsPrimitive || type == typeof(string) || type == typeof(DateTime);

        private void OnFilterCellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
        }

        private void OnFilterCellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }
        private static AutoCompleteStringCollection CreateAutoCompleteSource(IEnumerable<string> sourceList)
        {
            var result = new AutoCompleteStringCollection();
            foreach (string s in sourceList)
            {
                result.Add(s);
            }
            return result;
        }

        private void OnFilterCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == tbValue.Index)
            {
                var tb = (TextBox)dgvFilters.EditingControl;
                if (tb.AutoCompleteMode != AutoCompleteMode.None) return;

                var row = dgvFilters.Rows[e.RowIndex];
                KvP property;
                if (row.IsNewRow)
                {
                    int position = (int)dgvFilters[cbName.Index, e.RowIndex].Value;
                    property = bsFilters.Cast<KvP>().First(kvp => kvp.Position == position);
                }
                else property = (KvP)row.DataBoundItem;
                var propertyInfo = _filters[property.Position];
                if (propertyInfo.SourceList == null) return;

                tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                tb.AutoCompleteCustomSource = CreateAutoCompleteSource(propertyInfo.SourceList.Keys);
            }
        }
    }
}
