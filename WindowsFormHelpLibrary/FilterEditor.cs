using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
            _filters = filters;
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
            foreach (var kvP in Enumerable.Cast<KvP>(bsFilters))
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

        private void OnFilterCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == btFilterDelete.Index)
            {
                var row = dgvFilters.Rows[e.RowIndex];
                if(row.IsNewRow) return;
                dgvFilters.Rows.RemoveAt(e.RowIndex);
                _filters[e.RowIndex].Value = null;
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
    }
}
