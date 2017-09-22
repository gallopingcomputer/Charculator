/**–––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––//
/// StrEnc CLR Application | Version 1.6.0.81  | January 1, 2015 ///
///–––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––*/
/**––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––//
	COMMENTING
 *	use XML comments (/**) for most important descriptions; 
	use /* when describing the function of something, incl. warning messages; 
	use // to denote "what comes next"
 * task list comment keywords - categorize by:
		- currently assigned priority/status of a task or activities related to a note
		- note vs task (descriptive vs imperative)
//––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––*/
/*––––––––LAYOUT––––––––//
 * EXTn:	autosize in .Designer.cs: containers should have normal initial size;
		textboxes should have initial size 0
			(because they do not have an autosize property to allow this be reasonably specified in Designer, thus using filling docking to autosize)
		TODO: ADD TO YOUR NOTEBOOK
 * EXTn:	this.MinimumSize, consider both client area and borders. 
		(& DPI scaling support); What would happen if system theme is changed halfway?
 */
/** TODO LIST */
/*––––––––UI CONTROLS––––––––//
 * Todo:	textboxes: move some input checking to KeyPress
 * (NOTE):	differences between RichTextBox and TextBox.
		-	borders and scroll bars; 
		-	RTB selection is funny.
		-	use SelectionChanged for RTB, kboard events for TextBox
 * (NOTE) test: when you change text internally without explicitly changing selection, selection is reset.
			In a RichTextBox, AFTER text is changed, SelectionChanged is triggered, then TextChanged.
 */
/*––––––––USER INTERFACE––––––––//
 * TODO:	consider putting hdisplayoptions etc. into groupboxes too.
 * Todo:	And probably move to the lower left corner.
*/
/*––––––––FEATURES––––––––//
 * TODO:	Option for implicitly selecting prev char.
 * TRVO:	[trivial] toggle case (for both input and output)

 * EXTn:	produce text from arbitrary HEX values. 
		(decoding: string mode and comma-separated (character) mode)
Display
 * TODO:	mark encoding errors in input text	when using RichTextBox
 * ARGH:	fix input text formatting (remove formatting) for RichTextBox
 * EXTn:	highlight implicit "selection" of text (instead of fully selecting it) when using RTB
*/

/*––––––––IMPLEMENTATION DETAILS––––––––//
* TODO	enc_error_count alternatives: "cl_state" or something else based on cl.
* TODO	ENCODING PERFORMANCE AND OUTPUT FORMATTING
	  TODO	create another function combining strx2g_* and get_et (WITH enc_error_offset optimizations for use_et_cl cases) and test performance
	  TODO	try Encoding.GetByteCount()
			  In this case, use byte[] et + cl (similar to List<byte> et + byte cl[] but is initialized to an exact size and does not require conversion back to byte[])
				  there will be a cl pass (using Encoding.GetByteCount) and an et pass with size already known (-> this pass doesn't require manual char-by-char because bad chars are already known).
	  EXTn		use Encoder (& Decoder) and Stream when processing char by char?

* EXTn:	selection: find the difference instead of reevaluating
-------- rejected proposals --------
* DONT:	detect text changes (add/remove) instead of reevaluating all values
*/
/*––––––––IMPLEMENTATION / TRIVIAL OPTIMIZATION––––––––//
 * TODO:	list/array allocation - list: use "clear" vs. re-initialize every time;
		-	when using Clear(): @ global and local initial capacity of List and StringBuilder?
		-	when not: @ initial capacity based on need? @ use out instead of ref; @ GC.Collect?
 -------- dormant proposals --------
 * DONT:	consider using linked lists - need pointers and user classes (not the builtin class) - currently unnecessary
*/


using System;
using System.Windows.Forms;

namespace StrEncApplication
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles(); 
			Application.SetCompatibleTextRenderingDefault(false);
			Form MainForm = new MainForm(); Application.Run(MainForm);
		}
	}
}

