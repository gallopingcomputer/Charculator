using StrEnc;
using StrEnc.UI;

namespace StrEncApplication
{
	public partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.panelleft = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panelbox_textenc = new System.Windows.Forms.GroupBox();
			this.table_enc_options = new System.Windows.Forms.TableLayoutPanel();
			this.textbox_enc_name = new System.Windows.Forms.TextBox();
			this.textbox_enc = new System.Windows.Forms.TextBox();
			this.panel_hashalg = new System.Windows.Forms.TableLayoutPanel();
			this.textbox_hash = new System.Windows.Forms.TextBox();
			this.panelbox_hashalg = new System.Windows.Forms.GroupBox();
			this.table_hashalg_options = new System.Windows.Forms.TableLayoutPanel();
			this.panelright = new System.Windows.Forms.TableLayoutPanel();
			this.textbox_et = new System.Windows.Forms.TextBox();
			this.label_position = new System.Windows.Forms.Label();
			this.textbox_text = new System.Windows.Forms.TextBox();
			this.table_hformattingoptions = new System.Windows.Forms.TableLayoutPanel();
			this.label_etgr = new System.Windows.Forms.Label();
			this.textbox_et_gr = new System.Windows.Forms.TextBox();
			this.cbutton_et_markerrors = new System.Windows.Forms.CheckBox();
			this.textbox_errstring = new System.Windows.Forms.TextBox();
			this.table_tdisplayoptions = new System.Windows.Forms.TableLayoutPanel();
			this.cbutton_tdisplayoptions_wrap = new System.Windows.Forms.CheckBox();
			this.cbutton_tdisplayoptions_monospaced = new System.Windows.Forms.CheckBox();
			this.label_textbox = new System.Windows.Forms.Label();
			this.RadioButton_enc_codepage = new StrEnc.UI.RadioButton_TextEnc();
			this.RadioButton_enc_UTF32LE = new StrEnc.UI.RadioButton_TextEnc();
			this.RadioButton_enc_ASCII = new StrEnc.UI.RadioButton_TextEnc();
			this.RadioButton_enc_UTF7 = new StrEnc.UI.RadioButton_TextEnc();
			this.RadioButton_enc_UTF8 = new StrEnc.UI.RadioButton_TextEnc();
			this.RadioButton_enc_UTF16LE = new StrEnc.UI.RadioButton_TextEnc();
			this.RadioButton_HashAlg_SHA384 = new StrEnc.UI.RadioButton_HashAlg();
			this.RadioButton_HashAlg_SHA1 = new StrEnc.UI.RadioButton_HashAlg();
			this.RadioButton_HashAlg_MD5 = new StrEnc.UI.RadioButton_HashAlg();
			this.RadioButton_HashAlg_SHA256 = new StrEnc.UI.RadioButton_HashAlg();
			this.RadioButton_HashAlg_SHA512 = new StrEnc.UI.RadioButton_HashAlg();
			this.panelleft.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panelbox_textenc.SuspendLayout();
			this.table_enc_options.SuspendLayout();
			this.panel_hashalg.SuspendLayout();
			this.panelbox_hashalg.SuspendLayout();
			this.table_hashalg_options.SuspendLayout();
			this.panelright.SuspendLayout();
			this.table_hformattingoptions.SuspendLayout();
			this.table_tdisplayoptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelleft
			// 
			this.panelleft.AutoSize = true;
			this.panelleft.Controls.Add(this.panel1);
			this.panelleft.Dock = System.Windows.Forms.DockStyle.Left;
			this.panelleft.Location = new System.Drawing.Point(0, 0);
			this.panelleft.Margin = new System.Windows.Forms.Padding(0);
			this.panelleft.Name = "panelleft";
			this.panelleft.Size = new System.Drawing.Size(245, 400);
			this.panelleft.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.AutoSize = true;
			this.panel1.ColumnCount = 1;
			this.panel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
			this.panel1.Controls.Add(this.panelbox_textenc, 0, 0);
			this.panel1.Controls.Add(this.panel_hashalg, 0, 1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(10, 5, 5, 5);
			this.panel1.RowCount = 2;
			this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panel1.Size = new System.Drawing.Size(245, 400);
			this.panel1.TabIndex = 0;
			// 
			// panelbox_textenc
			// 
			this.panelbox_textenc.AutoSize = true;
			this.panelbox_textenc.Controls.Add(this.table_enc_options);
			this.panelbox_textenc.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelbox_textenc.Location = new System.Drawing.Point(10, 10);
			this.panelbox_textenc.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.panelbox_textenc.Name = "panelbox_textenc";
			this.panelbox_textenc.Padding = new System.Windows.Forms.Padding(5);
			this.panelbox_textenc.Size = new System.Drawing.Size(230, 151);
			this.panelbox_textenc.TabIndex = 0;
			this.panelbox_textenc.TabStop = false;
			this.panelbox_textenc.Text = "text encoding";
			// 
			// table_enc_options
			// 
			this.table_enc_options.AutoSize = true;
			this.table_enc_options.ColumnCount = 2;
			this.table_enc_options.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.table_enc_options.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.table_enc_options.Controls.Add(this.textbox_enc_name, 0, 4);
			this.table_enc_options.Controls.Add(this.RadioButton_enc_codepage, 0, 3);
			this.table_enc_options.Controls.Add(this.textbox_enc, 1, 3);
			this.table_enc_options.Controls.Add(this.RadioButton_enc_UTF32LE, 1, 2);
			this.table_enc_options.Controls.Add(this.RadioButton_enc_ASCII, 0, 0);
			this.table_enc_options.Controls.Add(this.RadioButton_enc_UTF7, 0, 1);
			this.table_enc_options.Controls.Add(this.RadioButton_enc_UTF8, 1, 1);
			this.table_enc_options.Controls.Add(this.RadioButton_enc_UTF16LE, 0, 2);
			this.table_enc_options.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table_enc_options.Location = new System.Drawing.Point(5, 21);
			this.table_enc_options.Margin = new System.Windows.Forms.Padding(0);
			this.table_enc_options.Name = "table_enc_options";
			this.table_enc_options.RowCount = 5;
			this.table_enc_options.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table_enc_options.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table_enc_options.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table_enc_options.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table_enc_options.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table_enc_options.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.table_enc_options.Size = new System.Drawing.Size(220, 125);
			this.table_enc_options.TabIndex = 0;
			// 
			// textbox_enc_name
			// 
			this.textbox_enc_name.BackColor = System.Drawing.Color.WhiteSmoke;
			this.table_enc_options.SetColumnSpan(this.textbox_enc_name, 2);
			this.textbox_enc_name.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.textbox_enc_name.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.textbox_enc_name.Location = new System.Drawing.Point(0, 102);
			this.textbox_enc_name.Margin = new System.Windows.Forms.Padding(0, 2, 0, 0);
			this.textbox_enc_name.MaxLength = 255;
			this.textbox_enc_name.Name = "textbox_enc_name";
			this.textbox_enc_name.ReadOnly = true;
			this.textbox_enc_name.Size = new System.Drawing.Size(220, 23);
			this.textbox_enc_name.TabIndex = 8;
			// 
			// textbox_enc
			// 
			this.textbox_enc.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.textbox_enc.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.textbox_enc.Location = new System.Drawing.Point(110, 77);
			this.textbox_enc.Margin = new System.Windows.Forms.Padding(0);
			this.textbox_enc.MaxLength = 5;
			this.textbox_enc.Name = "textbox_enc";
			this.textbox_enc.Size = new System.Drawing.Size(110, 23);
			this.textbox_enc.TabIndex = 7;
			this.textbox_enc.Click += new System.EventHandler(this.textbox_codepage_Click);
			this.textbox_enc.TextChanged += new System.EventHandler(this.textbox_codepage_TextChanged);
			this.textbox_enc.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_codepage_KeyDown);
			// 
			// panel_hashalg
			// 
			this.panel_hashalg.AutoSize = true;
			this.panel_hashalg.ColumnCount = 1;
			this.panel_hashalg.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panel_hashalg.Controls.Add(this.textbox_hash, 0, 1);
			this.panel_hashalg.Controls.Add(this.panelbox_hashalg, 0, 0);
			this.panel_hashalg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel_hashalg.Location = new System.Drawing.Point(10, 171);
			this.panel_hashalg.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.panel_hashalg.Name = "panel_hashalg";
			this.panel_hashalg.RowCount = 2;
			this.panel_hashalg.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panel_hashalg.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panel_hashalg.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.panel_hashalg.Size = new System.Drawing.Size(230, 219);
			this.panel_hashalg.TabIndex = 1;
			// 
			// textbox_hash
			// 
			this.textbox_hash.BackColor = System.Drawing.Color.WhiteSmoke;
			this.textbox_hash.CharacterCasing = System.Windows.Forms.CharacterCasing.Lower;
			this.textbox_hash.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textbox_hash.Font = new System.Drawing.Font("Consolas", 9F);
			this.textbox_hash.Location = new System.Drawing.Point(0, 106);
			this.textbox_hash.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.textbox_hash.Multiline = true;
			this.textbox_hash.Name = "textbox_hash";
			this.textbox_hash.ReadOnly = true;
			this.textbox_hash.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textbox_hash.Size = new System.Drawing.Size(230, 113);
			this.textbox_hash.TabIndex = 2;
			// 
			// panelbox_hashalg
			// 
			this.panelbox_hashalg.AutoSize = true;
			this.panelbox_hashalg.Controls.Add(this.table_hashalg_options);
			this.panelbox_hashalg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelbox_hashalg.Location = new System.Drawing.Point(0, 0);
			this.panelbox_hashalg.Margin = new System.Windows.Forms.Padding(0);
			this.panelbox_hashalg.Name = "panelbox_hashalg";
			this.panelbox_hashalg.Padding = new System.Windows.Forms.Padding(5);
			this.panelbox_hashalg.Size = new System.Drawing.Size(230, 101);
			this.panelbox_hashalg.TabIndex = 1;
			this.panelbox_hashalg.TabStop = false;
			this.panelbox_hashalg.Text = "select a hash algorithm";
			// 
			// table_hashalg_options
			// 
			this.table_hashalg_options.AutoSize = true;
			this.table_hashalg_options.ColumnCount = 2;
			this.table_hashalg_options.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.table_hashalg_options.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.table_hashalg_options.Controls.Add(this.RadioButton_HashAlg_SHA384, 0, 2);
			this.table_hashalg_options.Controls.Add(this.RadioButton_HashAlg_SHA1, 0, 0);
			this.table_hashalg_options.Controls.Add(this.RadioButton_HashAlg_MD5, 1, 0);
			this.table_hashalg_options.Controls.Add(this.RadioButton_HashAlg_SHA256, 0, 1);
			this.table_hashalg_options.Controls.Add(this.RadioButton_HashAlg_SHA512, 1, 1);
			this.table_hashalg_options.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table_hashalg_options.Location = new System.Drawing.Point(5, 21);
			this.table_hashalg_options.Margin = new System.Windows.Forms.Padding(0);
			this.table_hashalg_options.Name = "table_hashalg_options";
			this.table_hashalg_options.RowCount = 3;
			this.table_hashalg_options.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table_hashalg_options.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table_hashalg_options.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table_hashalg_options.Size = new System.Drawing.Size(220, 75);
			this.table_hashalg_options.TabIndex = 0;
			// 
			// panelright
			// 
			this.panelright.ColumnCount = 1;
			this.panelright.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.panelright.Controls.Add(this.textbox_et, 0, 5);
			this.panelright.Controls.Add(this.label_position, 0, 3);
			this.panelright.Controls.Add(this.textbox_text, 0, 2);
			this.panelright.Controls.Add(this.table_hformattingoptions, 0, 4);
			this.panelright.Controls.Add(this.table_tdisplayoptions, 0, 1);
			this.panelright.Controls.Add(this.label_textbox, 0, 0);
			this.panelright.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelright.Location = new System.Drawing.Point(245, 0);
			this.panelright.Margin = new System.Windows.Forms.Padding(0);
			this.panelright.Name = "panelright";
			this.panelright.Padding = new System.Windows.Forms.Padding(5, 5, 10, 5);
			this.panelright.RowCount = 6;
			this.panelright.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelright.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelright.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.panelright.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelright.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.panelright.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.panelright.Size = new System.Drawing.Size(395, 400);
			this.panelright.TabIndex = 2;
			// 
			// textbox_et
			// 
			this.textbox_et.BackColor = System.Drawing.Color.WhiteSmoke;
			this.textbox_et.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textbox_et.Font = new System.Drawing.Font("Consolas", 9F);
			this.textbox_et.HideSelection = false;
			this.textbox_et.Location = new System.Drawing.Point(5, 251);
			this.textbox_et.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.textbox_et.Multiline = true;
			this.textbox_et.Name = "textbox_et";
			this.textbox_et.ReadOnly = true;
			this.textbox_et.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textbox_et.Size = new System.Drawing.Size(380, 139);
			this.textbox_et.TabIndex = 5;
			// 
			// label_position
			// 
			this.label_position.AutoSize = true;
			this.label_position.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label_position.Location = new System.Drawing.Point(5, 200);
			this.label_position.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
			this.label_position.Name = "label_position";
			this.label_position.Size = new System.Drawing.Size(380, 15);
			this.label_position.TabIndex = 4;
			this.label_position.Text = "(SELECTION)";
			// 
			// textbox_text
			// 
			this.textbox_text.AcceptsTab = true;
			this.textbox_text.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textbox_text.HideSelection = false;
			this.textbox_text.Location = new System.Drawing.Point(5, 54);
			this.textbox_text.Margin = new System.Windows.Forms.Padding(0, 5, 0, 5);
			this.textbox_text.Multiline = true;
			this.textbox_text.Name = "textbox_text";
			this.textbox_text.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textbox_text.Size = new System.Drawing.Size(380, 138);
			this.textbox_text.TabIndex = 1;
			this.textbox_text.WordWrap = false;
			this.textbox_text.TextChanged += new System.EventHandler(this.textbox_text_TextChanged);
			this.textbox_text.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_text_keydown);
			this.textbox_text.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textbox_text_keyup);
			this.textbox_text.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textbox_text_mouse);
			this.textbox_text.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textbox_text_mouse);
			// 
			// table_hformattingoptions
			// 
			this.table_hformattingoptions.AutoSize = true;
			this.table_hformattingoptions.ColumnCount = 4;
			this.table_hformattingoptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table_hformattingoptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table_hformattingoptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table_hformattingoptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table_hformattingoptions.Controls.Add(this.label_etgr, 0, 0);
			this.table_hformattingoptions.Controls.Add(this.textbox_et_gr, 1, 0);
			this.table_hformattingoptions.Controls.Add(this.cbutton_et_markerrors, 2, 0);
			this.table_hformattingoptions.Controls.Add(this.textbox_errstring, 3, 0);
			this.table_hformattingoptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table_hformattingoptions.Location = new System.Drawing.Point(5, 223);
			this.table_hformattingoptions.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.table_hformattingoptions.Name = "table_hformattingoptions";
			this.table_hformattingoptions.RowCount = 1;
			this.table_hformattingoptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.table_hformattingoptions.Size = new System.Drawing.Size(380, 23);
			this.table_hformattingoptions.TabIndex = 3;
			// 
			// label_etgr
			// 
			this.label_etgr.AutoSize = true;
			this.label_etgr.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label_etgr.Location = new System.Drawing.Point(0, 0);
			this.label_etgr.Margin = new System.Windows.Forms.Padding(0);
			this.label_etgr.Name = "label_etgr";
			this.label_etgr.Size = new System.Drawing.Size(62, 23);
			this.label_etgr.TabIndex = 0;
			this.label_etgr.Text = "grouping: ";
			this.label_etgr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// textbox_et_gr
			// 
			this.textbox_et_gr.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.textbox_et_gr.Location = new System.Drawing.Point(62, 0);
			this.textbox_et_gr.Margin = new System.Windows.Forms.Padding(0);
			this.textbox_et_gr.MaxLength = 5;
			this.textbox_et_gr.MinimumSize = new System.Drawing.Size(40, 20);
			this.textbox_et_gr.Name = "textbox_et_gr";
			this.textbox_et_gr.Size = new System.Drawing.Size(40, 23);
			this.textbox_et_gr.TabIndex = 1;
			this.textbox_et_gr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.textbox_et_gr.TextChanged += new System.EventHandler(this.textbox_etgr_TextChanged);
			this.textbox_et_gr.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_etgr_KeyDown);
			this.textbox_et_gr.Leave += new System.EventHandler(this.textbox_etgr_Leave);
			// 
			// cbutton_et_markerrors
			// 
			this.cbutton_et_markerrors.AutoSize = true;
			this.cbutton_et_markerrors.Dock = System.Windows.Forms.DockStyle.Left;
			this.cbutton_et_markerrors.Location = new System.Drawing.Point(112, 4);
			this.cbutton_et_markerrors.Margin = new System.Windows.Forms.Padding(10, 4, 0, 0);
			this.cbutton_et_markerrors.Name = "cbutton_et_markerrors";
			this.cbutton_et_markerrors.Size = new System.Drawing.Size(169, 19);
			this.cbutton_et_markerrors.TabIndex = 2;
			this.cbutton_et_markerrors.Text = "display encoding errors as: ";
			this.cbutton_et_markerrors.UseVisualStyleBackColor = true;
			this.cbutton_et_markerrors.CheckedChanged += new System.EventHandler(this.et_markerrors_CheckedChanged);
			// 
			// textbox_errstring
			// 
			this.textbox_errstring.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.textbox_errstring.Location = new System.Drawing.Point(281, 0);
			this.textbox_errstring.Margin = new System.Windows.Forms.Padding(0);
			this.textbox_errstring.MinimumSize = new System.Drawing.Size(40, 20);
			this.textbox_errstring.Name = "textbox_errstring";
			this.textbox_errstring.Size = new System.Drawing.Size(62, 23);
			this.textbox_errstring.TabIndex = 3;
			this.textbox_errstring.TextChanged += new System.EventHandler(this.textbox_errstring_TextChanged);
			// 
			// table_tdisplayoptions
			// 
			this.table_tdisplayoptions.AutoSize = true;
			this.table_tdisplayoptions.ColumnCount = 4;
			this.table_tdisplayoptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table_tdisplayoptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table_tdisplayoptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table_tdisplayoptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.table_tdisplayoptions.Controls.Add(this.cbutton_tdisplayoptions_wrap, 2, 0);
			this.table_tdisplayoptions.Controls.Add(this.cbutton_tdisplayoptions_monospaced, 1, 0);
			this.table_tdisplayoptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.table_tdisplayoptions.Location = new System.Drawing.Point(5, 30);
			this.table_tdisplayoptions.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.table_tdisplayoptions.Name = "table_tdisplayoptions";
			this.table_tdisplayoptions.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.table_tdisplayoptions.RowCount = 1;
			this.table_tdisplayoptions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.table_tdisplayoptions.Size = new System.Drawing.Size(380, 19);
			this.table_tdisplayoptions.TabIndex = 1;
			// 
			// cbutton_tdisplayoptions_wrap
			// 
			this.cbutton_tdisplayoptions_wrap.AutoSize = true;
			this.cbutton_tdisplayoptions_wrap.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbutton_tdisplayoptions_wrap.Location = new System.Drawing.Point(98, 0);
			this.cbutton_tdisplayoptions_wrap.Margin = new System.Windows.Forms.Padding(0);
			this.cbutton_tdisplayoptions_wrap.Name = "cbutton_tdisplayoptions_wrap";
			this.cbutton_tdisplayoptions_wrap.Size = new System.Drawing.Size(106, 19);
			this.cbutton_tdisplayoptions_wrap.TabIndex = 2;
			this.cbutton_tdisplayoptions_wrap.Text = "word wrapping";
			this.cbutton_tdisplayoptions_wrap.UseVisualStyleBackColor = true;
			this.cbutton_tdisplayoptions_wrap.CheckedChanged += new System.EventHandler(this.tdisplayoptions_wrap_CheckedChanged);
			// 
			// cbutton_tdisplayoptions_monospaced
			// 
			this.cbutton_tdisplayoptions_monospaced.AutoSize = true;
			this.cbutton_tdisplayoptions_monospaced.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cbutton_tdisplayoptions_monospaced.Location = new System.Drawing.Point(3, 0);
			this.cbutton_tdisplayoptions_monospaced.Margin = new System.Windows.Forms.Padding(0);
			this.cbutton_tdisplayoptions_monospaced.Name = "cbutton_tdisplayoptions_monospaced";
			this.cbutton_tdisplayoptions_monospaced.Size = new System.Drawing.Size(95, 19);
			this.cbutton_tdisplayoptions_monospaced.TabIndex = 1;
			this.cbutton_tdisplayoptions_monospaced.Text = "monospaced";
			this.cbutton_tdisplayoptions_monospaced.UseVisualStyleBackColor = true;
			this.cbutton_tdisplayoptions_monospaced.CheckedChanged += new System.EventHandler(this.tdisplayoptions_monospaced_CheckedChanged);
			// 
			// label_textbox
			// 
			this.label_textbox.AutoSize = true;
			this.label_textbox.BackColor = System.Drawing.Color.WhiteSmoke;
			this.label_textbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label_textbox.Location = new System.Drawing.Point(5, 10);
			this.label_textbox.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
			this.label_textbox.Name = "label_textbox";
			this.label_textbox.Size = new System.Drawing.Size(380, 15);
			this.label_textbox.TabIndex = 0;
			this.label_textbox.Text = "Text display options:";
			this.label_textbox.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RadioButton_enc_codepage
			// 
			this.RadioButton_enc_codepage.AutoSize = true;
			this.RadioButton_enc_codepage.CodepageId = 0;
			this.RadioButton_enc_codepage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_enc_codepage.Location = new System.Drawing.Point(3, 78);
			this.RadioButton_enc_codepage.Name = "RadioButton_enc_codepage";
			this.RadioButton_enc_codepage.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_enc_codepage.TabIndex = 6;
			this.RadioButton_enc_codepage.Text = "Codepage:";
			this.RadioButton_enc_codepage.UseVisualStyleBackColor = true;
			this.RadioButton_enc_codepage.CheckedChanged += new System.EventHandler(this.radiobutton_textenc_change);
			// 
			// RadioButton_enc_UTF32LE
			// 
			this.RadioButton_enc_UTF32LE.AutoSize = true;
			this.RadioButton_enc_UTF32LE.CodepageId = 12000;
			this.RadioButton_enc_UTF32LE.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_enc_UTF32LE.Location = new System.Drawing.Point(113, 53);
			this.RadioButton_enc_UTF32LE.Name = "RadioButton_enc_UTF32LE";
			this.RadioButton_enc_UTF32LE.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_enc_UTF32LE.TabIndex = 5;
			this.RadioButton_enc_UTF32LE.Text = "UTF-32 LE";
			this.RadioButton_enc_UTF32LE.UseVisualStyleBackColor = true;
			this.RadioButton_enc_UTF32LE.CheckedChanged += new System.EventHandler(this.radiobutton_textenc_change);
			// 
			// RadioButton_enc_ASCII
			// 
			this.RadioButton_enc_ASCII.AutoSize = true;
			this.RadioButton_enc_ASCII.CodepageId = 20127;
			this.RadioButton_enc_ASCII.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_enc_ASCII.Location = new System.Drawing.Point(3, 3);
			this.RadioButton_enc_ASCII.Name = "RadioButton_enc_ASCII";
			this.RadioButton_enc_ASCII.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_enc_ASCII.TabIndex = 1;
			this.RadioButton_enc_ASCII.Text = "ASCII";
			this.RadioButton_enc_ASCII.UseVisualStyleBackColor = true;
			this.RadioButton_enc_ASCII.CheckedChanged += new System.EventHandler(this.radiobutton_textenc_change);
			// 
			// RadioButton_enc_UTF7
			// 
			this.RadioButton_enc_UTF7.AutoSize = true;
			this.RadioButton_enc_UTF7.CodepageId = 65000;
			this.RadioButton_enc_UTF7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_enc_UTF7.Location = new System.Drawing.Point(3, 28);
			this.RadioButton_enc_UTF7.Name = "RadioButton_enc_UTF7";
			this.RadioButton_enc_UTF7.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_enc_UTF7.TabIndex = 2;
			this.RadioButton_enc_UTF7.Text = "UTF-7";
			this.RadioButton_enc_UTF7.UseVisualStyleBackColor = true;
			this.RadioButton_enc_UTF7.CheckedChanged += new System.EventHandler(this.radiobutton_textenc_change);
			// 
			// RadioButton_enc_UTF8
			// 
			this.RadioButton_enc_UTF8.AutoSize = true;
			this.RadioButton_enc_UTF8.CodepageId = 65001;
			this.RadioButton_enc_UTF8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_enc_UTF8.Location = new System.Drawing.Point(113, 28);
			this.RadioButton_enc_UTF8.Name = "RadioButton_enc_UTF8";
			this.RadioButton_enc_UTF8.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_enc_UTF8.TabIndex = 3;
			this.RadioButton_enc_UTF8.Text = "UTF-8";
			this.RadioButton_enc_UTF8.UseVisualStyleBackColor = true;
			this.RadioButton_enc_UTF8.CheckedChanged += new System.EventHandler(this.radiobutton_textenc_change);
			// 
			// RadioButton_enc_UTF16LE
			// 
			this.RadioButton_enc_UTF16LE.AutoSize = true;
			this.RadioButton_enc_UTF16LE.CodepageId = 1200;
			this.RadioButton_enc_UTF16LE.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_enc_UTF16LE.Location = new System.Drawing.Point(3, 53);
			this.RadioButton_enc_UTF16LE.Name = "RadioButton_enc_UTF16LE";
			this.RadioButton_enc_UTF16LE.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_enc_UTF16LE.TabIndex = 4;
			this.RadioButton_enc_UTF16LE.Text = "UTF-16 LE";
			this.RadioButton_enc_UTF16LE.UseVisualStyleBackColor = true;
			this.RadioButton_enc_UTF16LE.CheckedChanged += new System.EventHandler(this.radiobutton_textenc_change);
			// 
			// RadioButton_HashAlg_SHA384
			// 
			this.RadioButton_HashAlg_SHA384.AutoSize = true;
			this.RadioButton_HashAlg_SHA384.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_HashAlg_SHA384.HashAlgorithmIdentifier = StrEnc.StrEncInfo.HashAlgId.SHA384;
			this.RadioButton_HashAlg_SHA384.Location = new System.Drawing.Point(3, 53);
			this.RadioButton_HashAlg_SHA384.Name = "RadioButton_HashAlg_SHA384";
			this.RadioButton_HashAlg_SHA384.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_HashAlg_SHA384.TabIndex = 5;
			this.RadioButton_HashAlg_SHA384.Text = "SHA-384";
			this.RadioButton_HashAlg_SHA384.UseVisualStyleBackColor = true;
			this.RadioButton_HashAlg_SHA384.CheckedChanged += new System.EventHandler(this.hashalg_CheckedChanged);
			// 
			// RadioButton_HashAlg_SHA1
			// 
			this.RadioButton_HashAlg_SHA1.AutoSize = true;
			this.RadioButton_HashAlg_SHA1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_HashAlg_SHA1.HashAlgorithmIdentifier = StrEnc.StrEncInfo.HashAlgId.SHA1;
			this.RadioButton_HashAlg_SHA1.Location = new System.Drawing.Point(3, 3);
			this.RadioButton_HashAlg_SHA1.Name = "RadioButton_HashAlg_SHA1";
			this.RadioButton_HashAlg_SHA1.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_HashAlg_SHA1.TabIndex = 1;
			this.RadioButton_HashAlg_SHA1.Text = "SHA-1";
			this.RadioButton_HashAlg_SHA1.UseVisualStyleBackColor = true;
			this.RadioButton_HashAlg_SHA1.CheckedChanged += new System.EventHandler(this.hashalg_CheckedChanged);
			// 
			// RadioButton_HashAlg_MD5
			// 
			this.RadioButton_HashAlg_MD5.AutoSize = true;
			this.RadioButton_HashAlg_MD5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_HashAlg_MD5.HashAlgorithmIdentifier = StrEnc.StrEncInfo.HashAlgId.MD5;
			this.RadioButton_HashAlg_MD5.Location = new System.Drawing.Point(113, 3);
			this.RadioButton_HashAlg_MD5.Name = "RadioButton_HashAlg_MD5";
			this.RadioButton_HashAlg_MD5.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_HashAlg_MD5.TabIndex = 2;
			this.RadioButton_HashAlg_MD5.Text = "MD5";
			this.RadioButton_HashAlg_MD5.UseVisualStyleBackColor = true;
			this.RadioButton_HashAlg_MD5.CheckedChanged += new System.EventHandler(this.hashalg_CheckedChanged);
			// 
			// RadioButton_HashAlg_SHA256
			// 
			this.RadioButton_HashAlg_SHA256.AutoSize = true;
			this.RadioButton_HashAlg_SHA256.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_HashAlg_SHA256.HashAlgorithmIdentifier = StrEnc.StrEncInfo.HashAlgId.SHA256;
			this.RadioButton_HashAlg_SHA256.Location = new System.Drawing.Point(3, 28);
			this.RadioButton_HashAlg_SHA256.Name = "RadioButton_HashAlg_SHA256";
			this.RadioButton_HashAlg_SHA256.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_HashAlg_SHA256.TabIndex = 3;
			this.RadioButton_HashAlg_SHA256.Text = "SHA-256";
			this.RadioButton_HashAlg_SHA256.UseVisualStyleBackColor = true;
			this.RadioButton_HashAlg_SHA256.CheckedChanged += new System.EventHandler(this.hashalg_CheckedChanged);
			// 
			// RadioButton_HashAlg_SHA512
			// 
			this.RadioButton_HashAlg_SHA512.AutoSize = true;
			this.RadioButton_HashAlg_SHA512.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RadioButton_HashAlg_SHA512.HashAlgorithmIdentifier = StrEnc.StrEncInfo.HashAlgId.SHA512;
			this.RadioButton_HashAlg_SHA512.Location = new System.Drawing.Point(113, 28);
			this.RadioButton_HashAlg_SHA512.Name = "RadioButton_HashAlg_SHA512";
			this.RadioButton_HashAlg_SHA512.Size = new System.Drawing.Size(104, 19);
			this.RadioButton_HashAlg_SHA512.TabIndex = 4;
			this.RadioButton_HashAlg_SHA512.Text = "SHA-512";
			this.RadioButton_HashAlg_SHA512.UseVisualStyleBackColor = true;
			this.RadioButton_HashAlg_SHA512.CheckedChanged += new System.EventHandler(this.hashalg_CheckedChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(640, 400);
			this.Controls.Add(this.panelright);
			this.Controls.Add(this.panelleft);
			this.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MinimumSize = new System.Drawing.Size(656, 408);
			this.Name = "MainForm";
			this.Text = "StrEnc";
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
			this.panelleft.ResumeLayout(false);
			this.panelleft.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panelbox_textenc.ResumeLayout(false);
			this.panelbox_textenc.PerformLayout();
			this.table_enc_options.ResumeLayout(false);
			this.table_enc_options.PerformLayout();
			this.panel_hashalg.ResumeLayout(false);
			this.panel_hashalg.PerformLayout();
			this.panelbox_hashalg.ResumeLayout(false);
			this.panelbox_hashalg.PerformLayout();
			this.table_hashalg_options.ResumeLayout(false);
			this.table_hashalg_options.PerformLayout();
			this.panelright.ResumeLayout(false);
			this.panelright.PerformLayout();
			this.table_hformattingoptions.ResumeLayout(false);
			this.table_hformattingoptions.PerformLayout();
			this.table_tdisplayoptions.ResumeLayout(false);
			this.table_tdisplayoptions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panelleft;
		private System.Windows.Forms.TableLayoutPanel panel1;
		private System.Windows.Forms.GroupBox panelbox_textenc;
		private System.Windows.Forms.TableLayoutPanel table_enc_options;
		private StrEnc.UI.RadioButton_TextEnc RadioButton_enc_UTF32LE;
		private StrEnc.UI.RadioButton_TextEnc RadioButton_enc_ASCII;
		private StrEnc.UI.RadioButton_TextEnc RadioButton_enc_UTF7;
		private StrEnc.UI.RadioButton_TextEnc RadioButton_enc_UTF8;
		private StrEnc.UI.RadioButton_TextEnc RadioButton_enc_UTF16LE;
		private System.Windows.Forms.TableLayoutPanel panel_hashalg;
		private System.Windows.Forms.TextBox textbox_hash;
		private System.Windows.Forms.GroupBox panelbox_hashalg;
		private System.Windows.Forms.TableLayoutPanel table_hashalg_options;
		private StrEnc.UI.RadioButton_HashAlg RadioButton_HashAlg_SHA384;
		private StrEnc.UI.RadioButton_HashAlg RadioButton_HashAlg_SHA1;
		private StrEnc.UI.RadioButton_HashAlg RadioButton_HashAlg_MD5;
		private StrEnc.UI.RadioButton_HashAlg RadioButton_HashAlg_SHA256;
		private StrEnc.UI.RadioButton_HashAlg RadioButton_HashAlg_SHA512;
		private System.Windows.Forms.TableLayoutPanel panelright;
		private System.Windows.Forms.TextBox textbox_et;
		private System.Windows.Forms.Label label_position;
		private System.Windows.Forms.TextBox textbox_enc;
		private StrEnc.UI.RadioButton_TextEnc RadioButton_enc_codepage;
		private System.Windows.Forms.TextBox textbox_enc_name;
		private System.Windows.Forms.TextBox textbox_text;
		private System.Windows.Forms.TableLayoutPanel table_hformattingoptions;
		private System.Windows.Forms.Label label_etgr;
		private System.Windows.Forms.TextBox textbox_et_gr;
		private System.Windows.Forms.TableLayoutPanel table_tdisplayoptions;
		private System.Windows.Forms.CheckBox cbutton_tdisplayoptions_monospaced;
		private System.Windows.Forms.CheckBox cbutton_tdisplayoptions_wrap;
		private System.Windows.Forms.Label label_textbox;
		private System.Windows.Forms.CheckBox cbutton_et_markerrors;
		private System.Windows.Forms.TextBox textbox_errstring;
	}
}

