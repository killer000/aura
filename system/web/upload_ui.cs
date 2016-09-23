using Aura.Shared.Util;
using Swebs;
using Swebs.RequestHandlers.CSharp;
using System;
using System.Text.RegularExpressions;

public class UiStorageController : Controller
{
	/// <remarks>
	/// Parameters:
	/// 
	///   Files:
	///     ui  ^[0-9]{16}\.xml$  XML file containing the ui settings
	/// 
	///   Post:
	///     char_id          long    Id of the character
	///     name_server      string  Name of the server
	///     ui_load_success  bool    Whether loading was successful
	/// 
	/// Security:
	///   Since the client doesn't give us anything but char id and
	///   server name, the settings could easily be overwritten
	///   by anybody.
	/// </remarks>
	public override void Handle(HttpRequestEventArgs args, string requestuestPath, string localPath)
	{
		var request = args.Request;
		var response = args.Response;

		response.ContentType = "text/plain";

		// Get file
		var file = request.File("ui");
		if (file == null)
		{
			Log.Debug("UiStorageController: Missing file.");
			return;
		}

		// Check file name
		if (!Regex.IsMatch(file.FileName, @"^[0-9]{16}\.xml$"))
		{
			Log.Debug("UiStorageController: Invalid file name '{0}'.", file.FileName);
			return;
		}

		var charId = request.Parameter("char_id", null);
		var serverName = request.Parameter("name_server", null);
		var loadSuccess = request.Parameter("ui_load_success", null);

		// Check parameters
		if (!Regex.IsMatch(charId, @"^[0-9]{16}$") || !Regex.IsMatch(charId, @"^[0-9A-Za-z_ ]+$"))
		{
			Log.Debug("UiStorageController: Invalid character id ({0}) or server name ({1}).", charId, serverName);
			return;
		}

		var group = charId.Substring(charId.Length - 3);

		// Move file
		try
		{
			file.MoveTo("user/web/save/ui/" + serverName + "/" + group + "/" + file.FileName);
		}
		catch (Exception ex)
		{
			Log.Exception(ex, "UiStorageController: Failed to move file.");
		}

		// Success
		response.Send("1");
	}
}
