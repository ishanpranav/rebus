// Ishan Pranav's REBUS: EntityGrid.Designer.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Windows.Forms;

namespace Rebus.Client.Windows
{
    partial class EntityGrid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.myPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // myPropertyGrid
            // 
            this.myPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.myPropertyGrid.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.myPropertyGrid.Name = "myPropertyGrid";
            this.myPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.myPropertyGrid.Size = new System.Drawing.Size(244, 287);
            this.myPropertyGrid.TabIndex = 0;
            this.myPropertyGrid.ToolbarVisible = false;
            this.myPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.OnMyPropertyGridPropertyValueChanged);
            // 
            // EntityGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.myPropertyGrid);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "EntityGrid";
            this.Size = new System.Drawing.Size(244, 287);
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGrid myPropertyGrid;
    }
}
