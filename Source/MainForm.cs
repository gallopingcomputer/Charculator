using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StrEnc.Application
{
    using StrEnc.Info;

    public partial class MainForm : Form
    {
        bool ui_advancedmode = true;

        public int selection_offset = 0;
        public int selection_length = 0;

        StringInfoView vim = new StringInfoView();

        bool suppress_vimupdate = false;
        bool suppress_refresh;
        // the main purpose of this is to prevent unnecessary refreshing

        public void SuppressRefresh(Action a)
        {
            suppress_refresh = true;
            a();
            suppress_refresh = false;
        }

        public void SuppressVimUpdate(Action a)
        {
            suppress_vimupdate = true;
            a();
            suppress_vimupdate = false;
        }


        public MainForm()
        {
            InitializeComponent(); 

            suppress_refresh = true;

            //*| some quick and dirty layout stuff, since designer code keeps getting overwritten.
            this.textbox_hash.Size = new System.Drawing.Size(230, 0);
            this.textbox_errstring.Size = new System.Drawing.Size(0, 23);

            //*| initial values for display options and parameters
            //* [^] these raise the corresponding events, which is fine; note that the suppress_refresh field is used to prevent the wrong code from executing in the event handlers. These could have been done in InitializeComponent instead, but the designer might mess them up
            cbutton_tdisplayoptions_monospaced.Checked = false; 
            cbutton_tdisplayoptions_wrap.Checked = true;
            
            textbox_errstring.Text = "?-";
            textbox_GR.Text = "0"; 
            RadioButton_enc_UTF16LE.Checked = true; 
            RadioButton_HashAlg_SHA1.Checked = true; 
            // cbutton_et_markerrors.Checked = true; 
            
            suppress_refresh = false;
            
            RefreshAll(); 
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
                ToggleAdvancedMode(); 
        }

        //
        // ─── UI OPTIONS ──────────────────────────────────────────────────
        //

        #region ui_options

        bool ToggleAdvancedMode()
        {
            if (ui_advancedmode == true)
            {
                table_hformattingoptions.Hide();
                table_tdisplayoptions.Hide();
                label_displayoptions_header.Text = "Source text";
                ui_advancedmode = false;
            }
            else
            {
                table_hformattingoptions.Show();
                table_tdisplayoptions.Show();
                label_displayoptions_header.Text = "Text display options:";
                ui_advancedmode = true;
            }
            return ui_advancedmode;
        }

        private void DisplayOption_Monospaced_CheckedChanged(object sender, EventArgs e) 
        {
            string fontname = cbutton_tdisplayoptions_monospaced.Checked ? "Consolas" : "Segoe UI";
            textbox_text.Font = new System.Drawing.Font(fontname, 9);
        }

        private void DisplayOption_WordWrap_CheckedChanged(object sender, EventArgs e) 
        {
            textbox_text.ScrollBars = 
                (textbox_text.WordWrap = cbutton_tdisplayoptions_wrap.Checked)
                ? ScrollBars.Vertical : ScrollBars.Both;
        }

        #endregion

        //
        // ─── FORMATTING OPTIONS ──────────────────────────────────────────
        //

        #region formatting_options
        
        // this only converts the text to int, and doesn't do any validation
        private (bool, int)? ConvertGRText(string gr_text)
        /* in case of fail: caller should revert to previous state */
        {
            bool minussign = gr_text.Length > 0 && gr_text[0] == '-';
            int abslen = gr_text.Length - Convert.ToInt32(minussign);

            if (abslen <= 0)
                return null;

            try {
                int gr = Convert.ToInt32(gr_text);
                return (true, gr);
            }
            catch (FormatException) {}
            catch (OverflowException) {}
            return (false, 0);
        }

        private bool? ChangeGroupingMode((bool, int)? r)
        {
            if (!r.HasValue) return null;
            (bool a, int gr) = r.Value;
            if (!a) return false;

            if (gr == vim.GroupingMode)
            {
                return null;
                //TODO| consider moving the null checking to vim.SetGroupingMode
            }
            else if (vim.SetGroupingMode(gr))
            {
                if (!suppress_refresh) RefreshEncodedView();
                return true;
            }
            else
                return false;
        }

        private void textbox_GR_Leave(object sender, EventArgs e)
        {
            if (textbox_GR.Text == "")
                textbox_GR.Text = vim.GroupingMode.ToString();
        }

        private void textbox_GR_KeyDown(object sender, KeyEventArgs e)
        {
            int gr_diff = 0;

            switch (e.KeyCode) {
            case Keys.Up:   gr_diff = 1;    break;
            case Keys.Down: gr_diff = -1;   break;
            default: return;
            }

            int new_gr = vim.GroupingMode + gr_diff;

            switch (ChangeGroupingMode((true, new_gr)))
            {
            case true:
                SuppressVimUpdate(() => { textbox_GR.Text = vim.GroupingMode.ToString(); });
                break;

            case false:
                System.Media.SystemSounds.Beep.Play();
                break;

            case null:
                break;
            }

            e.Handled = true; // is this necessary???
        }

        private void textbox_GR_TextChanged(object sender, EventArgs e) 
        {
            if (suppress_vimupdate) return;

            switch (ChangeGroupingMode(ConvertGRText(textbox_GR.Text)))
            {
            case null:
                break;

            case true:
                SuppressVimUpdate(() => { textbox_GR.Text = vim.GroupingMode.ToString(); });
                // this takes care of the trimming //TODO: fix selection
                break;

            case false:
                //(var sel_begin, var sel_len) = (textbox_GR.SelectionStart, textbox_GR.SelectionLength);
                //NOTE| That does not work, because TextChanged really happens *after* selection is changed. Damn.

                SuppressVimUpdate(() => { textbox_GR.Text = vim.GroupingMode.ToString(); });
                System.Media.SystemSounds.Beep.Play();
                textbox_GR.SelectAll();

                break;
            }
        } 

        private void textbox_errstring_TextChanged(object sender, EventArgs e)
        {
            if (suppress_vimupdate) return;

            if (vim.SetErrorString(textbox_errstring.Text))
            {
                if (!suppress_refresh) RefreshEncodedView();
            }
            else
                SuppressVimUpdate(() => { textbox_errstring.Text = vim.ErrorString; });
        }

        private void RefreshEncodedView() 
        {
            if (!vim.IsEncodingSupported())
                return;

            string text = vim.GetHexadecimalText();
            textbox_et.Text = text ?? "(Cannot display results with the selected grouping mode, because not all characters are supported by the selected encoding)";

            string fontname = (text == null) ? "Segoe UI" : "Consolas";
            textbox_et.Font = new System.Drawing.Font(fontname, 9);
            textbox_et.Enabled = !(text == null);

            Color backcolor = (vim.GetEncodingErrorState()) ? Color.Chocolate : Color.SteelBlue;
            panel_statusbar.BackColor = backcolor;

            label_stats.Text = String.Format("{0} characters", vim.Text.Length);

            if (vim.GetEncodingErrorState())
                label_stats.Text += String.Format(" ({0} errors)", vim.GetEncodingErrorCount());
            else
                label_stats.Text += String.Format(" => {0} bytes total", vim.GetEncodedLength());

            //No longer a TODO| consider moving the "bad codepage" checking to RefreshAll entirely (making sure that these won't be called under that) - NOPE, it's a bad idea, because you might be calling this when you change something when wait what

            RefreshSelectionDisplay();
        }

        #endregion

        //
        // ─── TEXT SELECTION ──────────────────────────────────────────────
        //

        #region textbox_text_selection
        //TODO ??!| for some keys (such as Ctrl+A, but not including navigation keys), update selection is called in both keyup and keydown. FIX THIS. Figure out how many times these are called.

        private void textbox_text_mouse(object sender, MouseEventArgs e) => TextSelectionChanged();
        private void textbox_text_keyup(object sender, KeyEventArgs e) => TextSelectionChanged();
        private void textbox_text_keydown(object sender, KeyEventArgs e) 
        {
            if (e.Control && e.KeyCode == Keys.A) {
                textbox_text.SelectAll(); 
                e.Handled = true; e.SuppressKeyPress = true; //TODO| ???
            }

            //TextSelectionChanged(); 
        }
        
        private void TextSelectionChanged()
        // For a RichTextBox, selection is changed before textchanged is fired. This could lead to problems, of course.
        //TODO| how do you know that the TB doesn't have a similar problem with those keyboard events???
        {
            if (selection_length != textbox_text.SelectionLength || selection_offset != textbox_text.SelectionStart) 
            {
                if (!suppress_refresh) RefreshSelectionDisplay(); 
            }
        }

        private void RefreshSelectionDisplay()
        {
            if (!vim.IsEncodingSupported())
                return;

            selection_offset = textbox_text.SelectionStart;
            selection_length = textbox_text.SelectionLength;
            int selection_end = selection_offset + selection_length;

            int errcount_o = vim.GetEncodingErrorCount(selection_offset);
            int length_o = vim.GetEncodedLength(selection_offset);
            int errcount_l = vim.GetEncodingErrorCount(selection_end, selection_offset);
            int length_l = vim.GetEncodedLength(selection_end, selection_offset);

            // update status bar text

            label_selectionstats.Text = "";

            if (selection_length > 0) label_selectionstats.Text +=
                String.Format("Selection: {0}", selection_length) +
                ((errcount_l == 0)
                    ? String.Format(" => {0}", length_l)
                    : String.Format(" ({0} errors)", errcount_l)
                )
                + "; "
            ;

            label_selectionstats.Text +=
                String.Format("Offset: {0}", selection_offset) +
                ((errcount_o == 0)
                    ? String.Format(" => {0}", length_o)
                    : String.Format(" ({0} errors)", errcount_o)
                )
            ;

            // select the appropriate portion of the hexadecimal text

            int disp_off = 0;
            int disp_len = 0;

            switch (vim.GroupingMode)
            {
            case -1:
            case 0:
                int mode = (1 + vim.GroupingMode);
                disp_off = length_o * 2 + selection_offset * mode + errcount_o * (vim.ErrorString.Length + mode);
                disp_len = length_l * 2 + selection_length * mode + errcount_l * (vim.ErrorString.Length + mode);
                if (mode != 0 & disp_len != 0) disp_len--;
                break;
            default:
                if (vim.GetEncodingErrorState()) return;

                int raw_len_pt1 = (length_o * 2 + errcount_o * vim.ErrorString.Length);
                int raw_len = ((length_l + length_o) * 2 + (errcount_l + errcount_o) * vim.ErrorString.Length);

                int n = vim.GroupingMode * 2;

                disp_off = (raw_len_pt1 / n) * (n + 1) + raw_len_pt1 % n;
                int disp_totallen_remainder = raw_len % n;
                int disp_totallen = (raw_len / n) * (n + 1) + disp_totallen_remainder;
                disp_len = disp_totallen - disp_off;
                if (disp_totallen_remainder == 0 && disp_len != 0) disp_len--;
                break;
            }

            textbox_et.Select(disp_off, disp_len);
        }

        #endregion

        //
        // ─── HASHING ─────────────────────────────────────────────────────
        //

        #region hashing

        private void HashAlg_CheckedChanged(object sender, EventArgs e) 
        {
            var n = sender as RadioButton;
            var hashalg_id = (Hashing.HashAlgId)n.Tag;
            if (n.Checked && hashalg_id != vim.hashalg_id)
            {
                vim.hashalg_id = hashalg_id;
                if (!suppress_refresh) RefreshHashText(); 
            }
        }

        private void RefreshHashText()
        {
            if (vim.IsEncodingSupported()) {
                textbox_hash.Text = vim.GetHashText();
                textbox_hash.Enabled = true;
            }
            else {
                textbox_hash.Text = "(n/a)"; 
                textbox_hash.Enabled = false; 
            }
        }
        
        #endregion

        //
        // ─── MAIN OPTIONS AND SOURCE TEXT ────────────────────────────────
        //

        #region main_options

        // this only converts the text to int, and doesn't do any validation
        private (bool, int)? ConvertCodepageIdText(string enc_text)
        {
            if (enc_text.Length == 0)
                return null;
            
            try {
                int a = Convert.ToInt32(enc_text);
                return (true, a);
            }
            catch (FormatException) {}
            catch (OverflowException) {}
            
            return (false, -1);
        }

        private bool? ChangeEncoding((bool, int)? r)
        {
            if (!r.HasValue) return null;
            (bool a, int enc_id) = r.Value;
            if (!a) return false;

            if (enc_id == (int)vim.EncodingID)
                return null;
            else if (vim.SetEncoding(enc_id))
            {
                if (!suppress_refresh) RefreshAll();
                return true;
            }
            else
                return false;
        }


        private void textbox_codepage_Enter(object sender, EventArgs e) 
        { 
            RadioButton_enc_Custom.Checked = true; 
        }

        private void textbox_enc_Leave(object sender, EventArgs e)
        {
            if (textbox_enc.Text == "")
                textbox_enc.Text = vim.EncodingID.GetNumericalName();
        }

        private void textbox_codepage_KeyDown(object sender, KeyEventArgs e)
        {
            int enc_diff = 0;
            
            switch (e.KeyCode) 
            {
            case Keys.Up:   enc_diff = 1;   break;
            case Keys.Down: enc_diff = -1;  break;
            default: return;
            }

            int new_enc_id = (int)vim.EncodingID + enc_diff;

            switch (ChangeEncoding((true, new_enc_id)))
            {
            case true:
                SuppressVimUpdate(() => { textbox_enc.Text = new_enc_id.ToString(); });
                break;

            case false:
                System.Media.SystemSounds.Beep.Play();
                break;

            case null:
                break;
            }
            
            e.Handled = true;
        }

        private void textbox_codepage_TextChanged(object sender, EventArgs e)
        {
            if (suppress_vimupdate) return;

            switch (ChangeEncoding(ConvertCodepageIdText(textbox_enc.Text)))
            {
            case null:
                break; // no changes

            case true:
                SuppressVimUpdate(() => { textbox_GR.Text = vim.EncodingID.GetNumericalName(); }); // takes care of the trimming
                break; // success

            case false:
                System.Media.SystemSounds.Beep.Play();
                SuppressVimUpdate(() => { textbox_enc.Text = vim.EncodingID.GetNumericalName(); }); // restore previous value (suppressupdate not strictly necessary)
                textbox_enc.SelectAll();

                break;
            }
        }

        private void EncodingMode_CheckedChanged(object sender, EventArgs e) /* handles textenc radio button changes */
        {
            var n = sender as RadioButton;
            if (!n.Checked) { return; }

            var enc_id = (int)n.Tag;
            if (enc_id != (int)Encodings.EncodingId.Undefined)
                ChangeEncoding((true, enc_id));
            else
            {
                switch (ChangeEncoding(ConvertCodepageIdText(textbox_enc.Text)))
                {
                case null:
                    break;
                case true:
                    break;
                case false:
                    // this cannot possibly be an illegal input, so none of that is needed - as long as you don't f*** up the initial value
                    break;
                }
            }
        }


        private void textbox_text_TextChanged(object sender, EventArgs e) 
        {
            if (suppress_vimupdate) return;

            vim.Text = textbox_text.Text;
            if (!suppress_refresh) RefreshAll();
        }


        private void RefreshAll()
        {
            if (!vim.IsEncodingSupported()) /* unsupported codepage */
            {
                textbox_enc_name.Text = vim.EncodingID.GetNumericalName() + " | (unsupported)";
                textbox_enc_name.Enabled = false;
                textbox_et.Font = new System.Drawing.Font("Consolas", 9);
                panelright.Enabled = false;
                textbox_hash.Enabled = false;
                // You could also move these to the corresponding Refresh *subfunctions* instead (there are arguments for both approaches)
            }
            else 
            {
                textbox_enc_name.Text = Encodings.GetEncodingFullName(vim.Encoder);
                textbox_enc_name.Enabled = true;
                textbox_et.Font = new System.Drawing.Font("Segoe UI", 9);
                panelright.Enabled = true;
                textbox_hash.Enabled = true;

                //if (vim.GetEncodingErrorState()) /* has encoding errors */ SuppressRefresh(() => { if (vim.GroupingMode > 0) textbox_GR.Text = "0"; }); /* do not allow GR > 0 when there are encoding errors */ /* SuppressRefresh is added to prevent RefreshEncodedView from being called twice when GR text has changed, because it has to be called later to ensure update in every case */
                // Just show an error message instead. See RefreshEncodedView
            }
            RefreshEncodedView();
            RefreshHashText();
        }

        #endregion
    }
}