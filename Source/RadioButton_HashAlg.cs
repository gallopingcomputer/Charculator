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
    public partial class RadioButton_HashAlg : System.Windows.Forms.RadioButton
    {
        public RadioButton_HashAlg()
        {
            InitializeComponent();
        }

        private StrEnc.StrEncInfo.HashAlgId hashalg;
        public StrEnc.StrEncInfo.HashAlgId HashAlgorithmIdentifier
        { get { return hashalg; } set { hashalg = value; } }
    }
}
