// Ishan Pranav's REBUS: EntityGrid.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using Rebus.Client.Windows.Properties;

namespace Rebus.Client.Windows
{
    internal partial class EntityGrid : UserControl
    {
        [Browsable(false)]
        public object SelectedObject
        {
            get
            {
                return myPropertyGrid.SelectedObject;
            }
            set
            {
                myPropertyGrid.SelectedObject = value;
            }
        }

        public ObjectSaver? Saver { get; set; }

        public EntityGrid()
        {
            InitializeComponent();
        }

        public bool ValidateObject()
        {
            ValidationContext context = new ValidationContext(SelectedObject);
            List<ValidationResult> results = new List<ValidationResult>();

            if (Validator.TryValidateObject(SelectedObject, context, results))
            {
                return true;
            }
            else
            {
                if (results.Count > 0)
                {
                    MessageBox.Show(results[0].ErrorMessage, Resources.WarningMessage, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return false;
            }
        }

        private bool ValidateProperty(PropertyDescriptor property, object? oldValue = null)
        {
            ValidationContext context = new ValidationContext(SelectedObject)
            {
                MemberName = property.Name
            };
            List<ValidationResult> results = new List<ValidationResult>();

            if (Validator.TryValidateProperty(property.GetValue(SelectedObject), context, results))
            {
                return true;
            }
            else
            {
                property.SetValue(SelectedObject, oldValue);

                if (results.Count > 0)
                {
                    MessageBox.Show(results[0].ErrorMessage, Resources.WarningMessage, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                return false;
            }
        }

        [SuppressMessage("Usage", "VSTHRD100:Avoid async void methods", Justification = "Event")]
        private async void OnMyPropertyGridPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem?.PropertyDescriptor is not null)
            {
                if (ValidateProperty(e.ChangedItem.PropertyDescriptor, e.OldValue))
                {
                    if (Saver is not null)
                    {
                        await Saver.SaveAsync(myPropertyGrid.SelectedObject);
                    }
                }
            }
        }
    }
}
