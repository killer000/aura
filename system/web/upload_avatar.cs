﻿using Swebs;
using Swebs.RequestHandlers.CSharp;
using System.Text.RegularExpressions;

/// <remarks>
/// Parameters:
///   Files:
///     userfile : snapshot.jpg
///     usertext : snapshot.txt
///     
///   Post:
///     user_id : admin
///     name_char : Zerono
///     name_server : Aura
///     char_id : 4503599627370498
///     gender : M
///     age : 23
///     key :
///     title : 60001
///     name_mate :
/// 
/// Response:
///   "1" for success
/// </remarks>
public class UploadAvatarController : Controller
{
	public override void Handle(HttpRequestEventArgs args, string requestuestPath, string localPath)
	{
		var request = args.Request;
		var response = args.Response;

		var charId = request.Parameter("char_id");
		var serverName = request.Parameter("name_server");
		var userFile = request.File("userfile");
		var userText = request.File("usertext");

		if (charId == null || !Regex.IsMatch(charId, @"^[0-9]+$") || serverName == null || !Regex.IsMatch(serverName, @"^[0-9A-Za-z_]+$") || userFile == null || userText == null)
			return;

		var key = charId.Substring(charId.Length - 3);
		var folder = "user/web/save/avatar/" + serverName + "/" + key + "/" + charId + "/";

		userFile.MoveTo(folder + "snapshot.jpg");
		userText.MoveTo(folder + "snapshot.txt");

		response.Send("1");
	}
}
