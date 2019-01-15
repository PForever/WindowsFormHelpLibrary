using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormHelpLibrary.IntegrateTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            cbType.DataSource = new List<CellTypeEnum> { CellTypeEnum.TextBox, CellTypeEnum.ComboBox, CellTypeEnum.Flag };
        }
        private void OnCellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == cbType.Index)
            {
                switch ((CellTypeEnum) ((ComboBox) dataGridView1.EditingControl).SelectedValue)
                {
                    case CellTypeEnum.TextBox:
                        dataGridView1[tbValue.Index, e.RowIndex] = new DataGridViewTextBoxCell();
                        break;
                    case CellTypeEnum.ComboBox:
                        dataGridView1[tbValue.Index, e.RowIndex] = new DataGridViewComboBoxCell();
                        break;
                    case CellTypeEnum.Flag:
                        dataGridView1[tbValue.Index, e.RowIndex] = new DataGridViewCheckBoxCell();
                        break;
                }
            }
        }
    }

    enum CellTypeEnum
    {
        TextBox,
        ComboBox,
        Flag
    }

    class DynamicColumn : DataGridViewColumn
    {
        private CellTypeEnum _cellType;
        private DataGridViewCell[] cells = new DataGridViewCell[] {new DataGridViewTextBoxCell(), new DataGridViewComboBoxCell{Items = { ""}}, new DataGridViewCheckBoxCell()};

        public DynamicColumn(DataGridViewCell cellTemplate) : base(cellTemplate)
        {
        }

        public DynamicColumn()
        {
            CellTemplate = cells[0];
        }

        private DataGridViewCell _cellTemplate;
        public override DataGridViewCell CellTemplate
        {
            get => _cellTemplate;
            set
            {
                _cellTemplate = value;
            }
        }

        public virtual CellTypeEnum CellType
        {
            get => _cellType; set
            {
                switch (value)
                {
                    case CellTypeEnum.TextBox:
                        CellTemplate = cells[0];
                        break;
                    case CellTypeEnum.ComboBox:
                        CellTemplate = cells[1];
                        break;
                    case CellTypeEnum.Flag:
                        CellTemplate = cells[2];
                        break;
                }
                _cellType = value;
            }
        }
        public override object Clone()
        {
            return base.Clone();
        }
    }
}
