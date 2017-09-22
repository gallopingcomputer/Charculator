using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrEnc.UI
{
    public partial class RadioButton_TextEnc : System.Windows.Forms.RadioButton
    {
        public RadioButton_TextEnc()
        {
            InitializeComponent();
        }

        private int codepage;
        public int CodepageId
        { get { return codepage; } set { codepage = value; } }
    }
}
