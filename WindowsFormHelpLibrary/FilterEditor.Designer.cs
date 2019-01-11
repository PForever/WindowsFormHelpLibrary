using WindowsFormHelpLibrary.FilterHelp;

namespace WindowsFormHelpLibrary
{
    partial class FilterEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvFilters = new System.Windows.Forms.DataGridView();
            this.btFilterDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.cbName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.bsProperties = new System.Windows.Forms.BindingSource(this.components);
            this.tbValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsFilters = new System.Windows.Forms.BindingSource(this.components);
            this.btDelete = new System.Windows.Forms.Button();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFilters)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvFilters);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btDelete);
            this.splitContainer1.Panel2.Controls.Add(this.btOk);
            this.splitContainer1.Panel2.Controls.Add(this.btCancel);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer1.Size = new System.Drawing.Size(394, 351);
            this.splitContainer1.SplitterDistance = 319;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvFilters
            // 
            this.dgvFilters.AutoGenerateColumns = false;
            this.dgvFilters.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dgvFilters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.btFilterDelete,
            this.cbName,
            this.tbValue});
            this.dgvFilters.DataSource = this.bsFilters;
            this.dgvFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFilters.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvFilters.Location = new System.Drawing.Point(0, 0);
            this.dgvFilters.Name = "dgvFilters";
            this.dgvFilters.Size = new System.Drawing.Size(394, 319);
            this.dgvFilters.TabIndex = 0;
            this.dgvFilters.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnFilterCellClicked);
            this.dgvFilters.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.OnFilterCellValidating);
            // 
            // btFilterDelete
            // 
            this.btFilterDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.btFilterDelete.HeaderText = "Удалить";
            this.btFilterDelete.Name = "btFilterDelete";
            this.btFilterDelete.Text = "Удалить";
            this.btFilterDelete.UseColumnTextForButtonValue = true;
            this.btFilterDelete.Width = 56;
            // 
            // cbName
            // 
            this.cbName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cbName.DataPropertyName = "Position";
            this.cbName.DataSource = this.bsProperties;
            this.cbName.DisplayMember = "Name";
            this.cbName.HeaderText = "Имя столбца";
            this.cbName.Name = "cbName";
            this.cbName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cbName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cbName.ValueMember = "Position";
            this.cbName.Width = 98;
            // 
            // bsProperties
            // 
            this.bsProperties.DataSource = typeof(PropertyNamePosition);
            // 
            // tbValue
            // 
            this.tbValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.tbValue.DataPropertyName = "Value";
            this.tbValue.HeaderText = "Значение";
            this.tbValue.Name = "tbValue";
            // 
            // bsFilters
            // 
            this.bsFilters.DataSource = typeof(WindowsFormHelpLibrary.FilterEditor.KvP);
            this.bsFilters.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.OnAddingFilter);
            this.bsFilters.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.OnFiltersChanged);
            // 
            // btDelete
            // 
            this.btDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btDelete.Dock = System.Windows.Forms.DockStyle.Right;
            this.btDelete.Location = new System.Drawing.Point(237, 3);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(79, 22);
            this.btDelete.TabIndex = 2;
            this.btDelete.Text = "Удалить все";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.OnAllDelete);
            // 
            // btOk
            // 
            this.btOk.Dock = System.Windows.Forms.DockStyle.Right;
            this.btOk.Location = new System.Drawing.Point(316, 3);
            this.btOk.Name = "btOk";
            this.btOk.Size = new System.Drawing.Size(75, 22);
            this.btOk.TabIndex = 1;
            this.btOk.Text = "Применить";
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.OnOk);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btCancel.Location = new System.Drawing.Point(3, 3);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 22);
            this.btCancel.TabIndex = 0;
            this.btCancel.Text = "Отмена";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.OnCancel);
            // 
            // FilterEditor
            // 
            this.AcceptButton = this.btOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(394, 351);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FilterEditor";
            this.Text = "Фильтр";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFilters)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.DataGridView dgvFilters;
        private System.Windows.Forms.BindingSource bsFilters;
        private System.Windows.Forms.BindingSource bsProperties;
        private System.Windows.Forms.DataGridViewButtonColumn btFilterDelete;
        private System.Windows.Forms.DataGridViewComboBoxColumn cbName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbValue;
    }
}