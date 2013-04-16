using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace BSWork.ComponentsSettingsForms
{
    public static class UIExtensions
    {
        public static void AdjustWidth(this ComboBox ioComboBox)
        {
            ioComboBox.DropDownWidth = (from object item in ioComboBox.Items
                                            select
                                                TextRenderer.MeasureText(item.ToString(), ioComboBox.Font).Width).Max();
        }

        public static void uintTextBox_Validating(object iSender, CancelEventArgs e)
        {
            uint tmp;
            TextBox sender = iSender as TextBox;
            if (!uint.TryParse(sender.Text, out tmp))
                e.Cancel = true;
            sender.Select(0, sender.Text.Length);
        }

        public static void doubleTextBox_Validating(object iSender, CancelEventArgs e)
        {
            double tmp;
            TextBox sender = iSender as TextBox;
            if (!double.TryParse(sender.Text, out tmp))
                e.Cancel = true;
            sender.Select(0, sender.Text.Length);
        }

        public static Label LabelWithTextAndName(string iText, string iName=null)
        {
            Label aLabel = new Label();
            aLabel.Text = iText;
            aLabel.AutoSize = true;
            aLabel.Name = iName;
            return aLabel;
        }

    }
}
