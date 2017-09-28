using System;
using System.Windows.Forms;

namespace StrEnc.Application
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			Form MainForm = new MainForm();
            System.Windows.Forms.Application.Run(MainForm);
		}
	}
}

