using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace StrEnc.Application
{
    using StrEnc;

    public partial class MainForm : Form
    {
        bool ui_advancedmode = true;

        public int selection_offset = 0;
        public int selection_length = 0;

        StringInfoView strinfo = new StringInfoView();

        bool suppress_update = false;
        bool suppress_refresh;
        // the main purpose of this is to prevent unnecessary refreshing

        private void SuppressRefresh(Action a)
        {
            suppress_refresh = true;
            a();
            suppress_refresh = false;
        }

        private void SuppressUpdate(Action a)
        {
            suppress_update = true;
            a();
            suppress_update = false;
        }


        public MainForm()
        {
            InitializeComponent();

            SuppressRefresh(() =>
            {
                //*| some quick and dirty layout stuff, since designer code keeps getting overwritten.
                textbox_hash.Size = new System.Drawing.Size(230, 0);
                textbox_errstring.Size = new System.Drawing.Size(0, 23);

                //*| initial values for display options and parameters
                //* [^] these raise the corresponding events, which is fine; note that the suppress_refresh field is used to prevent the wrong code from executing in the event handlers. These could have been done in InitializeComponent instead, but the designer might mess them up
                cbutton_tdisplayoptions_monospaced.Checked = false;
                cbutton_tdisplayoptions_wrap.Checked = true;

                textbox_errstring.Text = "?-";
                textbox_GR.Text = "0";
                RadioButton_enc_UTF16LE.Checked = true;
                RadioButton_HashAlg_SHA1.Checked = true;
                // cbutton_et_markerrors.Checked = true; 
            });

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
        // ─── DISPLAY OPTIONS ──────────────────────────────────────────
        //

        #region formatting_options

        // this only converts the text input to int, and doesn't do any validation
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
            catch (FormatException) { }
            catch (OverflowException) { }
            return (false, 0);
        }

        private bool? ChangeGroupingMode((bool, int)? r)
        {
            if (!r.HasValue) return null;
            (bool a, int gr) = r.Value;
            if (!a) return false;

            if (gr == strinfo.GroupingMode)
            {
                return null;
                //TODO| consider moving the null checking to strinfo.SetGroupingMode
            }
            else if (strinfo.SetGroupingMode(gr))
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
                textbox_GR.Text = strinfo.GroupingMode.ToString();
        }

        private void textbox_GR_KeyDown(object sender, KeyEventArgs e)
        {
            int gr_diff = 0;

            switch (e.KeyCode) {
            case Keys.Up: gr_diff = 1; break;
            case Keys.Down: gr_diff = -1; break;
            default: return;
            }

            int new_gr = strinfo.GroupingMode + gr_diff;

            switch (ChangeGroupingMode((true, new_gr)))
            {
            case true:
                SuppressUpdate(() => { textbox_GR.Text = strinfo.GroupingMode.ToString(); });
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
            if (suppress_update) return;

            switch (ChangeGroupingMode(ConvertGRText(textbox_GR.Text)))
            {
            case null:
                break;

            case true:
                SuppressUpdate(() => { textbox_GR.Text = strinfo.GroupingMode.ToString(); });
                // this takes care of the trimming //TODO: fix selection
                break;

            case false:
                //(var sel_begin, var sel_len) = (textbox_GR.SelectionStart, textbox_GR.SelectionLength);
                //NOTE| That does not work, because TextChanged really happens *after* selection is changed. Damn.

                SuppressUpdate(() => { textbox_GR.Text = strinfo.GroupingMode.ToString(); });
                System.Media.SystemSounds.Beep.Play();
                textbox_GR.SelectAll();

                break;
            }
        }

        private void textbox_errstring_TextChanged(object sender, EventArgs e)
        {
            if (suppress_update) return;

            if (strinfo.SetErrorString(textbox_errstring.Text))
            {
                if (!suppress_refresh) RefreshEncodedView();
            }
            else
                SuppressUpdate(() => { textbox_errstring.Text = strinfo.ErrorString; });
        }

        private void RefreshEncodedView()
        {
            if (!strinfo.IsEncodingSupported())
                return;

            string text = strinfo.GetHexadecimalText();
            textbox_et.Text = text ?? "(Cannot display results with the selected grouping mode, because not all characters are supported by the selected encoding)";

            string fontname = (text == null) ? "Segoe UI" : "Consolas";
            textbox_et.Font = new System.Drawing.Font(fontname, 9);
            textbox_et.Enabled = !(text == null);

            Color backcolor = (strinfo.GetEncodingErrorState()) ? Color.Chocolate : Color.SteelBlue;
            panel_statusbar.BackColor = backcolor;

            label_stats.Text = String.Format("{0} characters", strinfo.Text.Length);

            if (strinfo.GetEncodingErrorState())
                label_stats.Text += String.Format(" ({0} errors)", strinfo.GetEncodingErrorCount());
            else
                label_stats.Text += String.Format(" => {0} bytes total", strinfo.GetEncodedLength());

            //No longer a TODO| consider moving the "bad codepage" checking to RefreshAll entirely (making sure that these won't be called under that) - NOPE, it's a bad idea, because you might be calling this when you change something when wait what

            RefreshSelectionDisplay();
        }

        #endregion

        //
        // ─── TEXT SELECTION ──────────────────────────────────────────────
        //

        #region textbox_text_selection

        /* 
            triggers for selection change include (but might not be limited to) the following conditions:
            1. mouse click (right after (?) mousedown)
            2. navigation/selection keys (arrow keys, Home/End, PgUp/PgDn, Ctrl+A, etc.)
            3. text input hotkeys (Ctrl+V, delete)
            4. character input

            it would appear (?) that during keyboard events, selection only changes *after* the corresponding KeyDown and KeyPress events. This means it would be necessary to catch KeyPress/KeyUp; there is no real need to catch KeyDown.
            This does not appear very consistent, at least not when debugging (?) - something KeyUp events seem to be dropped for no reason
        */

        private void textbox_text_MouseUp(object sender, MouseEventArgs e) => TextSelectionChanged();
        private void textbox_text_KeyUp(object sender, KeyEventArgs e) => TextSelectionChanged();
        private void textbox_text_KeyPress(object sender, KeyPressEventArgs e) => TextSelectionChanged();

        private void TextSelectionChanged()
        {
            if (selection_length != textbox_text.SelectionLength || selection_offset != textbox_text.SelectionStart) 
            {
                if (!suppress_refresh) RefreshSelectionDisplay(); 
            }
        }

        private void RefreshSelectionDisplay()
        {
            if (!strinfo.IsEncodingSupported())
                return;

            selection_offset = textbox_text.SelectionStart;
            selection_length = textbox_text.SelectionLength;
            int selection_end = selection_offset + selection_length;

            int errcount_o = strinfo.GetEncodingErrorCount(selection_offset);
            int length_o = strinfo.GetEncodedLength(selection_offset);
            int errcount_l = strinfo.GetEncodingErrorCount(selection_end, selection_offset);
            int length_l = strinfo.GetEncodedLength(selection_end, selection_offset);

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

            // select the corresponding portion of the raw bytes text

            int disp_off = 0;
            int disp_len = 0;

            switch (strinfo.GroupingMode)
            {
            case -1:
            case 0:
                int mode = (1 + strinfo.GroupingMode);
                disp_off = length_o * 2 + selection_offset * mode + errcount_o * (strinfo.ErrorString.Length + mode);
                disp_len = length_l * 2 + selection_length * mode + errcount_l * (strinfo.ErrorString.Length + mode);
                if (mode != 0 & disp_len != 0) disp_len--;
                break;
            default:
                if (strinfo.GetEncodingErrorState()) return;

                int raw_len_pt1 = (length_o * 2 + errcount_o * strinfo.ErrorString.Length);
                int raw_len = ((length_l + length_o) * 2 + (errcount_l + errcount_o) * strinfo.ErrorString.Length);

                int n = strinfo.GroupingMode * 2;

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
            if (n.Checked && hashalg_id != strinfo.hashalg_id)
            {
                strinfo.hashalg_id = hashalg_id;
                if (!suppress_refresh) RefreshHashText(); 
            }
        }

        private void RefreshHashText()
        {
            if (strinfo.IsEncodingSupported()) {
                textbox_hash.Text = strinfo.GetHashText();
                textbox_hash.Enabled = true;
            }
            else {
                textbox_hash.Text = "(n/a)"; 
                textbox_hash.Enabled = false; 
            }
        }

        #endregion

        //
        // ─── TEXT ENCODING OPTIONS ───────────────────────────────────────
        //

        #region encoding_options

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

            if (enc_id == (int)strinfo.EncodingID)
                return null;
            else if (strinfo.SetEncoding(enc_id))
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
                textbox_enc.Text = strinfo.EncodingID.GetNumericalName();
        }

        private void textbox_codepage_KeyDown(object sender, KeyEventArgs e)
        {
            int enc_diff = 0;
            
            switch (e.KeyCode) 
            {
            case Keys.Up:   enc_diff =  1;  break;
            case Keys.Down: enc_diff = -1;  break;
            default: return;
            }

            // this deals with up/down arrow keys

            int new_enc_id = (int)strinfo.EncodingID + enc_diff;

            switch (ChangeEncoding((true, new_enc_id)))
            {
            case true:
                SuppressUpdate(() => { textbox_enc.Text = new_enc_id.ToString(); });
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
            if (suppress_update) return;

            bool? r = ChangeEncoding(ConvertCodepageIdText(textbox_enc.Text));

            SuppressUpdate(() => { textbox_enc.Text = strinfo.EncodingID.GetNumericalName(); }); // takes care of the trimming; this restore the previous value if it failed

            if (r == false)
            {
                System.Media.SystemSounds.Beep.Play();
                SuppressUpdate(() => { textbox_enc.Text = strinfo.EncodingID.GetNumericalName(); }); // 
                textbox_enc.SelectAll();
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

        #endregion

        //
        // ─── MAIN ────────────────────────────────────────────────────────
        //

        private void textbox_text_TextChanged(object sender, EventArgs e) 
        {
            if (suppress_update) return;

            strinfo.Text = textbox_text.Text;
            if (!suppress_refresh) RefreshAll();
        }


        private void RefreshAll()
        {
            if (!strinfo.IsEncodingSupported()) /* unsupported codepage */
            {
                textbox_enc_name.Text = strinfo.EncodingID.GetNumericalName() + " | (unsupported)";
                textbox_enc_name.Enabled = false;
                textbox_et.Font = new System.Drawing.Font("Consolas", 9);
                panelright.Enabled = false;
                textbox_hash.Enabled = false;
                // You could also move these to the corresponding Refresh *subfunctions* instead (there are arguments for both approaches)
            }
            else 
            {
                textbox_enc_name.Text = Encodings.GetEncodingFullName(strinfo.Encoder);
                textbox_enc_name.Enabled = true;
                textbox_et.Font = new System.Drawing.Font("Segoe UI", 9);
                panelright.Enabled = true;
                textbox_hash.Enabled = true;

                //if (strinfo.GetEncodingErrorState()) /* has encoding errors */ SuppressRefresh(() => { if (strinfo.GroupingMode > 0) textbox_GR.Text = "0"; }); /* do not allow GR > 0 when there are encoding errors */ /* SuppressRefresh is added to prevent RefreshEncodedView from being called twice when GR text has changed, because it has to be called later to ensure update in every case */
                // Just show an error message instead. See RefreshEncodedView
            }
            RefreshEncodedView();
            RefreshHashText();
        }
    }
}