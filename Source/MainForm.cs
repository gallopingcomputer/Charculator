
/*––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––//
	WARNING: PITFALLS
 * [!] beware the order by which events are triggered!
			[!] in RTB, changing the text leads to a short-lived inconsistent state.
 * [!] beware the nature of "change" events (both built-in and the functions in which you check for changes). If input has not changed, nothing will happen.
			thus, do not use them to initialize data members.
			When using these functions, take pains to ensure that default values are consistent.
 * [!] "change" events (instead of KeyDown, mouse Leave, etc.) need to check for the parameter initialized (except for trivial display options that do not depend on the initialization of data parameters) before calling update functions that depend on values processed from input text. note that enc_update (instead of enc_changed) is the one that does this, because it calls et_update() after getting the encoding object.
 * [!] do not modify values outside their dedicated methods (esp. for event handlers)
 * [^] initial values; be careful not to trigger update
 * [^] Do not call "update" functions when nothing has really changed. 
		"prev" is needed when a "changed" function is called (input has changed) but the result might be the same 
		(e.g. in the case of et_gr, where the text may be different but the value may be the same; and it might be called after emptying the textbox and moving the focus outside the textbox; same for encoding)
//––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––*/

#define use_et_cl
//#define use_RTB

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using StrEnc;

namespace StrEncApplication
{
	public partial class MainForm : Form
	{
		bool initialized = false;

		// encoding & display (output formatting) options
		bool ui_advancedmode = true;

		int enc_id = -1;
		Encoding enc;
		int et_gr;
		bool et_markerrs;
		string errstring = "";
		StrEncInfo.HashAlgId hashalg;
		//bool et_highlightprevchar = true; /* TODO: implement control */

		// encoding state and cached values
	#if use_et_cl
		List<byte> et;
		byte[] cl;
	#else
		List<byte[]> et0;
	#endif
		List<int> enc_errors_pos;
		int t_seloffset = 0; int t_sellength = 0;

		public MainForm()
		{
			InitializeComponent(); 
			this.textbox_hash.Size = new System.Drawing.Size(230, 0); /* EXT size of textbox. This is quick but dirty. 
																	   * EXT size of textbox (continued): next line currently seems unnecessary because you limited the minimum size */
			this.textbox_errstring.Size = new System.Drawing.Size(0, 23);

			// tooltip
			System.Windows.Forms.ToolTip tooltip_lbl_pos = new System.Windows.Forms.ToolTip();
			tooltip_lbl_pos.SetToolTip(label_position, "The offset and position of selected text.");

			cbutton_tdisplayoptions_monospaced.Checked = false; /* [^] uses event, but doesn't hurt. */
			cbutton_tdisplayoptions_wrap.Checked = true; /* same */
			InitializeParameters();
		}
		public void InitializeParameters()
		{	initialized = false;
		
			// create objects
		#if use_et_cl
			et = new List<byte>(2048); /* EXT does this make any difference? */
				//cl = new byte[0]; /* initialize cl to empty array so that textbox_selection_change and et_updatedisplay will not try to use a null cl. */
				/* [resolved issue!] this seems no longer necessary because et_update will be called at initialization, which calls get_et and initializes cl. */
		#else
			et0 = new List<byte[]>(2048);
		#endif
			enc_errors_pos = new List<int>();

			// initial values. beware events!
			/* [!] do not change the values directly. Instead, change them from the controls 
				(which use the "change" functions). This will ensure values are consistent. */
			textbox_et_gr.Text = "0"; /* [!] won't trigger et_updatedisplay because ... equals default value - ONLY BEFORE YOU ADDED "initialized" */
			textbox_errstring.Text = "?-"; /* [!] same here */
			RadioButton_HashAlg_SHA1.Checked = true; /* [.] will not trigger hash_update because initialized is false. This call sets the value for hashalg. */
									/* if "initialized" were not used, would call hash update (that doesn't generate error) unless the corresponding hashalg value had been set. */
			cbutton_et_markerrors.Checked = true; // [.] 
									/* if "initialized" is not used, will cause error when cl is not yet initialized under use_et_cl (see cl init comment above)
										this setting is not considered to be moved to InitializeComponent (although the default there is Checked = true). */
			RadioButton_enc_UTF16LE.Checked = true; /* when placed after "initialized = true", triggers full update */

			initialized = true; /* [!] prior to this point, nothing should rely on et. */
			et_update(); /* [!] special. ensures the initialization of the remaining auxiliary parameters & gets encoding for initial text */

			/*textbox_text.Text = "hahaha"; // for testing; or just "", it's the same issue. see next comment
			/* ARGH for some reason text is not considered to have changed if you do not manually call et_update() here. */
		}

		#region display_options

		private void MainForm_KeyDown(object sender, KeyEventArgs e)
		{ if (e.Control && e.KeyCode == Keys.D) { toggleadvancedmode(); } }

		bool toggleadvancedmode()
		{
			if (ui_advancedmode == true)
			{
				table_hformattingoptions.Hide();
				table_tdisplayoptions.Hide();
				label_textbox.Text = "Source text";
				ui_advancedmode = false;
			}
			else
			{
				table_hformattingoptions.Show();
				table_tdisplayoptions.Show();
				label_textbox.Text = "Text display options:";
				ui_advancedmode = true;
			}
			return ui_advancedmode;
		}

		#endregion

		#region et_formatting_options
			private void textbox_etgr_TextChanged(object sender, EventArgs e) { 
			etgr_update(); } private void etgr_update() {
				int etgr_prev = et_gr;
				int selstart = textbox_et_gr.SelectionStart;
				byte minussign = 0;
				int pos = 0;
				if (textbox_et_gr.Text.Length > 0 && textbox_et_gr.Text[0] == 45)
				{ minussign = 1; ++pos; }
				int absl = textbox_et_gr.Text.Length - minussign;

				textbox_et_gr.Text = (minussign == 0 ? "" : "-") + textbox_et_gr.Text.Substring(pos, textbox_et_gr.Text.Length - pos);

				//(input.Where(c => char.IsDigit(c)).ToArray())
				//while (pos < textbox_et_gr.Text.Length) /* remove non-numeric characters */
				/*
				{
					int character = Encoding.Unicode.GetBytes(textbox_et_gr.Text.Substring(pos, 1))[0];
					if ((character >= 48 && character <= 57)) { ++pos; }
					else { textbox_et_gr.Text = textbox_et_gr.Text.Remove(pos, 1); 
						textbox_et_gr.SelectionStart = selstart - 1; return; }
				}
				*/
				if (absl != 0) {
					int i;
					if (minussign == 1) { i = (-1) * Convert.ToInt32(textbox_et_gr.Text.Substring(1)); }
					else { i = Convert.ToInt32(textbox_et_gr.Text); }
					if ((enc_errors_pos.Count > 0 && (i > 0 || i < -1)) || (enc_errors_pos.Count == 0 && (i > 8 || i < -1)))
					/* the range of input. also, do not use et_gr > 0 when there are char encoding errors */
					{ selstart -= 1; System.Media.SystemSounds.Beep.Play();
						textbox_et_gr.Text = textbox_et_gr.Text.Remove(selstart, 1); } }
				textbox_et_gr.SelectionStart = selstart;

				if (textbox_et_gr.Text.Length > 0 && textbox_et_gr.Text[0] == 45) 
				{ minussign = 1; }
				if (textbox_et_gr.TextLength - minussign == 0) { /* do nothing */ }
				else {
					if (minussign == 1) 
					{ et_gr = (-1) * Convert.ToInt32(textbox_et_gr.Text.Substring(1)); }
					else { et_gr = Convert.ToInt32(textbox_et_gr.Text); }
					if (initialized)
					{
						if (et_gr != etgr_prev)
						{
#if VERBOSE
							MessageBox.Show("etgr text changed (will call et_updatedisplay)", "textbox_etgr_TextChanged", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
							et_updatedisplay(); /* do not call update when not initialized */ }
						else {
#if VERBOSE
							MessageBox.Show("etgr text changed (will not call et_updatedisplay)", "textbox_etgr_TextChanged", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
						}
					}
					else {
#if VERBOSE
						MessageBox.Show("etgr text changed (before fully initialized)", "textbox_etgr_TextChanged", MessageBoxButtons.OK, MessageBoxIcon.Question);
#endif
					}
				}
			}
			private void textbox_etgr_Leave(object sender, EventArgs e)
			{
				if (textbox_et_gr.Text.Length == 0 ||
					(textbox_et_gr.Text[0] == 45 && textbox_et_gr.Text.Length == 1))
					textbox_et_gr.Text = "0";
			}
			private void textbox_etgr_KeyDown(object sender, KeyEventArgs e)
			{
				switch (e.KeyCode) {
					case Keys.Up: int i;
						if (textbox_et_gr.Text.Length == 0) return;
						if (textbox_et_gr.Text[0] == 45)
						{ if (textbox_et_gr.Text.Length < 2) return;
							i = (-1) * Convert.ToInt32(textbox_et_gr.Text.Substring(1)) + 1; }
						else
						{ i = Convert.ToInt32(textbox_et_gr.Text) + 1; }
						/* limit the range of input. also, do not use et_gr > 0 when there are char encoding errors */
						if ((enc_errors_pos.Count > 0 && i <= 0) || (enc_errors_pos.Count == 0 && i <= 8)) 
						{ textbox_et_gr.Text = i.ToString(); }
						else { System.Media.SystemSounds.Beep.Play(); }
						break;
					case Keys.Down: int j;
						if (textbox_et_gr.Text.Length == 0) return;
						if (textbox_et_gr.Text[0] == 45)
						{ if (textbox_et_gr.Text.Length < 2) return;
							j = (-1) * Convert.ToInt32(textbox_et_gr.Text.Substring(1)) - 1; }
						else
						{ j = Convert.ToInt32(textbox_et_gr.Text) - 1; }
						if (j >= -1) { textbox_et_gr.Text = j.ToString(); }
						else { System.Media.SystemSounds.Beep.Play(); }
						break;
			} }

			private void et_markerrors_CheckedChanged(object sender, EventArgs e)
				{ CheckBox cbtn_etme = sender as CheckBox;
				et_markerrs = cbtn_etme.Checked;
				if (initialized) et_updatedisplay(); }
			private void textbox_errstring_TextChanged(object sender, EventArgs e)
			{
				/* TODO check if valid ( - excludes some characters - numbers, letters and spaces; what about special characters and control characters?????) */
				/* consider "if something is entered, check the CheckBox" - that shouldn't be done here becasue this function does not distinguish user input and internal changes */
				errstring = textbox_errstring.Text;
				if (initialized) et_updatedisplay(); 
			}
		#endregion
		
		#region t_display_options_trivial
		// don't worry about these. They don't have to check for initialized because it doesn't affect them.
			private void tdisplayoptions_monospaced_CheckedChanged(object sender, EventArgs e) {
				switch (cbutton_tdisplayoptions_monospaced.Checked) {
					case true: textbox_text.Font = new System.Drawing.Font("Consolas", 9); break;
					default: textbox_text.Font = new System.Drawing.Font("Segoe UI", 9); break; }
			}
			private void tdisplayoptions_wrap_CheckedChanged(object sender, EventArgs e) {
				textbox_text.WordWrap = cbutton_tdisplayoptions_wrap.Checked;
				switch (cbutton_tdisplayoptions_wrap.Checked) {
				#if use_RTB
					case true: textbox_text.ScrollBars = RichTextBoxScrollBars.Vertical; break;
					default: textbox_text.ScrollBars = RichTextBoxScrollBars.Both; break; 
				#else
					case true: textbox_text.ScrollBars = ScrollBars.Vertical; break;
					default: textbox_text.ScrollBars = ScrollBars.Both; break; 
				#endif
				}
			}
		#endregion

		#region hashalg_change
			private void hashalg_CheckedChanged(object sender, EventArgs e) /* handles hashalg radio button changes */ {
				StrEnc.UI.RadioButton_HashAlg selectedrb = sender as StrEnc.UI.RadioButton_HashAlg;
				if (selectedrb.Checked && hashalg != selectedrb.HashAlgorithmIdentifier)
					{ hashalg = selectedrb.HashAlgorithmIdentifier;
#if VERBOSE
					MessageBox.Show("HashAlg button changed", "radiobutton_hashalg_change", MessageBoxButtons.OK);
#endif
					if (initialized) hash_update(); }
			}
		#endregion

		#region textbox_text_selection
			// <NOTE> for some keys (such as Ctrl+A, but not including navigation keys), update selection is called in both keyup and keydown.
			//private void textbox1_selection_mouse(object sender, MouseEventArgs e) { textbox_selection_change(); }
			//private void textbox1_selection_keyup(object sender, KeyEventArgs e) { textbox_selection_change(); }

		#if !use_RTB
			private void textbox_text_mouse(object sender, MouseEventArgs e) { text_selection_change(); }
			private void textbox_text_keyup(object sender, KeyEventArgs e) {
#if VERBOSE
				MessageBox.Show("textbox_text keyup", "textbox_text_keyup", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
				text_selection_change(); }
			private void textbox_text_keydown(object sender, KeyEventArgs e) {
#if VERBOSE
				MessageBox.Show("textbox_text keydown", "textbox_text_keydown", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
				if (e.Control && e.KeyCode == Keys.A) { textbox_text.SelectAll(); e.Handled = true; e.SuppressKeyPress = true; } 
				text_selection_change(); }
		#else
			private void textbox_text_keydown(object sender, KeyEventArgs e)
				{ if (e.Control && e.KeyCode == Keys.A) 
					{ textbox_text.SelectAll(); e.Handled = true; e.SuppressKeyPress = true; } }
			private void textbox_text_SelectionChanged(object sender, EventArgs e) { text_selection_change(); }
		#endif
			private void text_selection_change()
			{
				if (initialized) {
					if (t_sellength != textbox_text.SelectionLength || t_seloffset != textbox_text.SelectionStart) { /* this is actually unnecessary for RichTextBox. */

						/* [!!!] RTB SelectionChanged occurs before TextChanged.
						* if you don't do the following, updateselection will be called before et & cl are updated, and may try to access an out-of-bound index. */
						/* This doesn't happen for a regular TextBox because the relevant keyboard events all use KeyUp which is called after TextChanged. */
					#if use_RTB
						#if use_et_cl
							if (textbox_text.Text.Length != cl.Length)
							{
	#if VERBOSE
								MessageBox.Show("text selection changed (inconsistent state): " + textbox_text.TextLength + " != " + cl.Length, "text_selection_change", MessageBoxButtons.OK, MessageBoxIcon.Error);
	#endif
						#else
							if (textbox_text.Text.Length != et0.Count)
							{
	#if VERBOSE
								MessageBox.Show("text selection changed (inconsistent state): " + textbox_text.TextLength + " != " + et0.Count, "text_selection_change", MessageBoxButtons.OK, MessageBoxIcon.Error);
	#endif
						#endif
								return; }
					#endif

						/* no debug message is required here. They will be displayed in et_updateselection(). */
					//added temporarily
					#if use_et_cl
#if VERBOSE
							MessageBox.Show("text selection changed " + textbox_text.TextLength + "==" + cl.Length + "\n" + textbox_text.SelectionStart + " == " + t_seloffset + "\n" + textbox_text.SelectionLength + " == " + t_sellength, "text_selection_change", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
					#else
#if VERBOSE
							MessageBox.Show("text selection changed " + textbox_text.TextLenght + "==" + et0.Count + "\n" + textbox_text.SelectionStart + " == " + t_seloffset + "\n" + textbox_text.SelectionLength + " == " + t_sellength, "text_selection_change", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
#endif

						et_updateselection(); }
					else {
#if VERBOSE
						MessageBox.Show("text selection appears to have changed (but in fact nope)", "textbox_text_SelectionChanged", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
					}
				}
				else { 
#if VERBOSE
					MessageBox.Show("text selection may have changed (but before fully initialized)", "textbox_text_SelectionChanged", MessageBoxButtons.OK, MessageBoxIcon.Question);
#endif
					}
			}
		#endregion

		#region textenc_change
			private void radiobutton_textenc_change(object sender, EventArgs e) /* handles textenc radio button changes */ {
				StrEnc.UI.RadioButton_TextEnc selectedrb = sender as StrEnc.UI.RadioButton_TextEnc;
				if (!selectedrb.Checked) { return; }
				enc_changed(selectedrb);
			}
			private void textbox_codepage_Click(object sender, EventArgs e) { RadioButton_enc_codepage.Checked = true; }
			private void textbox_codepage_KeyDown(object sender, KeyEventArgs e)
			{
				bool chk = false; switch (e.KeyCode) {
					case Keys.Enter: chk = true;
						break;
					case Keys.Up: chk = true; int i;
						if (textbox_enc.Text.Length == 0) { i = 1; } else { i = Convert.ToInt32(textbox_enc.Text) + 1; }
						if (i < 65536) { textbox_enc.Text = i.ToString(); }
						break;
					case Keys.Down: chk = true; int j;
						if (textbox_enc.Text.Length == 0) { j = 0; } else { j = Convert.ToInt32(textbox_enc.Text) - 1; }
						if (j >= 0) { textbox_enc.Text = j.ToString(); }
						break; }
				if (chk == true) { RadioButton_enc_codepage.Checked = true; e.Handled = true; }
			}
			private void textbox_codepage_TextChanged(object sender, EventArgs e)
			{
				int selstart = textbox_enc.SelectionStart;
				int pos = 0;
				while (pos < textbox_enc.Text.Length) /* remove non-numeric characters */
				{
					int character = Encoding.Unicode.GetBytes(textbox_enc.Text.Substring(pos, 1))[0];
					if (character >= 48 && character <= 57) { pos += 1; }
					else { textbox_enc.Text = textbox_enc.Text.Remove(pos, 1); 
						textbox_enc.SelectionStart = selstart - 1; return; }
				}
				if (textbox_enc.Text.Length == 0) { }
				else {
					if (Convert.ToInt32(textbox_enc.Text) >= 65536)
					{ selstart -= 1;
						textbox_enc.Text = textbox_enc.Text.Remove(selstart, 1);
						textbox_enc.SelectionStart = selstart; return; } }
				enc_changed(RadioButton_enc_codepage);
			}
			private void enc_changed(StrEnc.UI.RadioButton_TextEnc selectedrb)
			{
#if VERBOSE
				MessageBox.Show("encoding settings changed", "enc_changed", MessageBoxButtons.OK);
#endif
				int enc_id_prev = enc_id;
				enc_id = selectedrb.CodepageId;
				if (RadioButton_enc_codepage.Checked) { /* manually specified codepage */
					if (textbox_enc.Text.Length == 0) { /* unspecified codepage */ enc_id = -1; }
					else { enc_id = Convert.ToInt32(textbox_enc.Text); }
				}
				if (enc_id != enc_id_prev) { int v = enc_update();
					if (v > 0) { textbox_enc_name.Text = enc_id + " | " + enc.EncodingName; textbox_enc_name.Enabled = true; } 
				else if (v < 0) { textbox_enc_name.Text = enc_id + " | (unsupported)"; textbox_enc_name.Enabled = false; }
				else if (v == 0) { textbox_enc_name.Text = "n/a"; textbox_enc_name.Enabled = false; }
				}
			}
			private int enc_update() // call only when initialized
			{
				int state = 0; // state: -1 for invalid codepage, 1 for valid, 0 for unspecified (where enc_id was -1)
				// get encoding
				if (enc_id >= 0) {
					try
					{ enc = Encoding.GetEncoding(enc_id,
							EncoderExceptionFallback.ExceptionFallback, DecoderExceptionFallback.ExceptionFallback); state = 1; }
					catch (Exception exc){
						if (exc is ArgumentException || exc is NotSupportedException) /* unsupported codepage id */ { state = -1; }
						else { throw; }
					}
				}
				if (!initialized)
				{
#if VERBOSE
					MessageBox.Show("enc_update called (before fully initialized)", "enc_update", MessageBoxButtons.OK, MessageBoxIcon.Question);
#endif
					return state; // do not call update when !initialized
				}
#if VERBOSE
				MessageBox.Show("enc_update called", "enc_update", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
				et_update(state); 
				return state;
			}
		#endregion

		#region text_change
			private void textbox_text_TextChanged(object sender, EventArgs e) 
			{
				if (initialized) {
#if VERBOSE
					MessageBox.Show("text changed", "textbox_text_change", MessageBoxButtons.OK, MessageBoxIcon.Information);
#endif
					et_update();
				}
				else {
#if VERBOSE
					MessageBox.Show("text changed (before fully initialized)", "textbox_text_change", MessageBoxButtons.OK, MessageBoxIcon.Question);
#endif
				}
			}
		#endregion

		// core functions
		// call only when initialized. 
		private void et_update(int state = 1) 
		{
			if (state <= 0) { /* state <= 0 when encoding settings are invalid. This DOESN'T clear et-cl/et0 */
				panelright.Enabled = false; textbox_et.Font = new System.Drawing.Font("Segoe UI", 9);
				textbox_et.Text = "(unspecified or unsupported codepage)";
				et_updateselection(false);
				hash_update(false); /* since encoding settings are invalid */
			}
			else {
				panelright.Enabled = true; textbox_et.Font = new System.Drawing.Font("Consolas", 9);
				/* do not allow et_gr > 0 when there are encoding errors */
		#if use_et_cl
				StrEncInfo.get_et(enc, textbox_text.Text, ref et, out cl, ref enc_errors_pos);
		#else
				StrEncInfo.get_et(enc, textbox_text.Text, ref et0, ref enc_errors_pos);
		#endif
				if (enc_errors_pos.Count > 0) {
					initialized = false; /* [^] this is added to prevent et_updatedisplay from being called twice when etgr text has changed, because it has to be called later to ensure update in every case */
					if (et_gr > 0) textbox_et_gr.Text = "0"; /* [!] this alone might not trigger update - not when etgr text hasn't changed. That is not enough for a full update. */
					initialized = true;
				}
				et_updatedisplay();

				if (enc_errors_pos.Count == 0) hash_update();
				else hash_update(false); /* also done here because there are encoding errors. */
			}
		}

		private void et_updatedisplay() 
		{
#if VERBOSE
			MessageBox.Show("et_updatedisplay called", "et_updatedisplay", MessageBoxButtons.OK);
#endif
			StringBuilder str = new StringBuilder(256);
		#if use_et_cl
			if (et_markerrs)
				StrEncInfo.strx2g_e(ref et, ref cl, et_gr, out str, errstring);
			else
				StrEncInfo.strx2g(ref et, ref cl, et_gr, out str);
		#else
			if (et_markerrs)
				StrEncInfo.strx2g_e(ref et0, et_gr, out str, errstring);
			else
				StrEncInfo.strx2g(ref et0, et_gr, out str);
		#endif
			textbox_et.Text = str.ToString();
			et_updateselection();
		}

		private int enc_error_count(int pos)
		{	/* Finds the count of encoding errors up to character at index */
			int n = 0;
			for (int x = 0; x < enc_errors_pos.Count; ++x)
			{ if (enc_errors_pos[x] <= pos) { ++n; } }
			return n;
		}

		private void et_updateselection(bool valid = true) 
			/** updates selection in hexadecimal view based on text selection. does not check previous seloffset and sellength. */
			/* call only when (initialized == true) */
		{
			if (valid == false)
			{ textbox_et.SelectionStart = 0; textbox_et.SelectionLength = 0;
				label_position.Text = "selection offset and length: n/a";
			#if use_et_cl
#if VERBOSE
				MessageBox.Show("et_updateselection called when encoding is invalid\n" + textbox_text.TextLength + " " + cl.Length, "et_updateselection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
				return; }
#if VERBOSE
			MessageBox.Show("et_updateselection called\n" + textbox_text.TextLength + " " + cl.Length, "et_updateselection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
			#else
#if VERBOSE
				MessageBox.Show("et_updateselection called when encoding is invalid\n" + textbox_text.TextLength + " " + et0.Count, "et_updateselection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
				return; }
#if VERBOSE
			MessageBox.Show("et_updateselection called\n" + textbox_text.TextLength + " " + et0.Count, "et_updateselection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
#endif
			#endif

			/* [resolved issue!] if you didn't check for Text.Length, at program startup cl.Length would have been called when cl hasn't been initialized.
			 fixed by always initializing cl. ADDENDUM: the previous description of the issue no longer applies, because et_update will be called to ensure proper initialization at startup.
			 * This issue did not exist when clearing the textbox after initialization, because get_et will always initialize *out* cl. 
				if (textbox_text.Text.Length == 0) { return; }*/
			t_seloffset = textbox_text.SelectionStart;
			t_sellength = textbox_text.SelectionLength;
			int t_highlightoffset = t_seloffset;
			int t_highlightlength = t_sellength;
			int et_seloffset = 0; 
			int et_sellength = 0;
			int enc_errcount_pre = enc_error_count(t_highlightoffset - 1);
			/* note: effect of evaluating enc_errcount_sel here: the error string will not be implicitly selected */
			int enc_errcount_sel = enc_error_count(t_highlightlength + t_highlightoffset - 1) - enc_errcount_pre;
			int textbox_et_seloffset = 0; 
			int textbox_et_sellength = 0;
			/*if (et_highlightprevchar && t_highlightlength == 0)
			{ // implicitly "select" (highlight) the previous character at caret position
				if (t_highlightoffset == 0) { }
				else { --t_highlightoffset; ++t_highlightlength; }
			}*/ // DON'T - updated Dec 20, 2015
			
			int i = 0;

			if (et_gr == 0)
			{
			#if use_et_cl
				for (; i < t_highlightoffset; ++i) { et_seloffset += cl[i];
					textbox_et_seloffset += 2 * cl[i] + 1; }
				for (; i < t_highlightoffset + t_highlightlength; ++i) { et_sellength += cl[i];
					textbox_et_sellength += 2 * cl[i] + 1; }
			#else
				for (; i < t_highlightoffset; ++i) { et_seloffset += et0[i].Length;
					textbox_et_seloffset += 2 * et0[i].Length + 1; }
				for (; i < t_highlightoffset + t_highlightlength; ++i) { et_sellength += et0[i].Length;
					textbox_et_sellength += 2 * et0[i].Length + 1; }
			#endif
				if (!et_markerrs) { 
					textbox_et_seloffset -= enc_errcount_pre;
					textbox_et_sellength -= enc_errcount_sel; }
				else /* mark_enc_errors */ { 
					textbox_et_seloffset += (errstring.Length) * enc_errcount_pre;
					textbox_et_sellength += (errstring.Length) * enc_errcount_sel; }
				if (textbox_et_sellength > 0) --textbox_et_sellength; 
			}
			else // (et_gr != 0)
			{
			#if use_et_cl
				for (; i < t_highlightoffset; ++i) { et_seloffset += cl[i]; }
				for (; i < t_highlightoffset + t_highlightlength; ++i) { et_sellength += cl[i]; }
			#else
				for (; i < t_highlightoffset; ++i) { et_seloffset += et0[i].Length; }
				for (; i < t_highlightoffset + t_highlightlength; ++i) { et_sellength += et0[i].Length; }
			#endif
				if (et_gr < 0) // (et_gr < 0)
				{
					textbox_et_seloffset = 2 * et_seloffset;
					textbox_et_sellength = 2 * et_sellength;
					if (et_markerrs)
					{
						textbox_et_seloffset += (errstring.Length) * enc_errcount_pre;
						textbox_et_sellength += (errstring.Length) * 
							(enc_error_count(t_highlightlength + t_highlightoffset - 1) - enc_errcount_pre); }
				}
				else // (et_gr > 0)
				{
					// do not proceed if there are encoding errors.
					if (enc_errors_pos.Count > 0) throw new NotSupportedException();
					else 
					{
						int ir = (et_seloffset % et_gr); // number of bytes left
						int i1 = (et_seloffset - ir) / et_gr; // number of complete groups, 
						// not a narrowing conversion, equals (int) (et_seloffset / et_gr)
						textbox_et_seloffset = (et_gr * 2 + 1) * i1 + ir * 2;
						int tr = ((et_seloffset + et_sellength) % et_gr); // total
						int t1 = (et_seloffset + et_sellength - tr) / et_gr;
						textbox_et_sellength = (et_gr * 2 + 1) * t1 + tr * 2 - textbox_et_seloffset;
						if (tr == 0 && textbox_et_sellength > 0) --textbox_et_sellength;
					}
				}
			}
			textbox_et.SelectionStart = textbox_et_seloffset;
			textbox_et.SelectionLength = textbox_et_sellength;
			textbox_et.ScrollToCaret();
			label_position.Text = "selection: " +
					t_highlightoffset.ToString() + ", " + t_highlightlength.ToString() + " (chars); ";
			if (enc_errcount_pre + enc_errcount_sel == 0)
			{
				if (enc_errors_pos.Count == 0) label_position.ForeColor = base.ForeColor;
				label_position.Text += 
					et_seloffset.ToString() + ", " + et_sellength.ToString() + " (bytes)"; }
			else { label_position.ForeColor = Color.Maroon;
				label_position.Text += 
					"[" + enc_errcount_pre + " ERROR(S)], [" + enc_errcount_sel + " ERROR(S)]"; }
		}

		private void hash_update(bool valid = true)
		{
			if (valid == true) { // valid is false when encoding settings are invalid, which means no meaningful array can be passed to the function
		#if use_et_cl
				textbox_hash.Text = StrEncInfo.strx2(StrEncInfo.hash(et.ToArray(), hashalg)); textbox_hash.Enabled = true;
		#else
				List<byte> et_l = new List<byte>();
				for (int i = 0; i < et0.Count; ++i)
					for (int j = 0; j < et0[i].Length; ++j)  et_l.Add(et0[i][j]);
				textbox_hash.Text = StrEncInfo.strx2(StrEncInfo.hash(et_l.ToArray(), hashalg)); textbox_hash.Enabled = true;
		#endif
			}
			else { textbox_hash.Text = ""; textbox_hash.Enabled = false; }
		}

	}
}