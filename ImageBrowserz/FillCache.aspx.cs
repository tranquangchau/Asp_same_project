using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace ImageBrowser
{
	/// <summary>
	/// Summary description for FillCache.
	/// </summary>
	public partial class FillCache : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}

		private void DeleteCacheImages(DirectoryInfo di)
		{
			DirectoryInfo[] subs = di.GetDirectories("webpics");
			if ( subs.Length == 1 && subs[0].GetDirectories().Length == 0 )
			{
				Response.Write("DELETING:" + subs[0].FullName + "<BR>");
				Debug.WriteLine("DELETING:" + subs[0].FullName);
				subs[0].Delete(true);
			}
			subs = di.GetDirectories("thumbs");
			if ( subs.Length == 1 && subs[0].GetDirectories().Length == 0 )
				subs[0].Delete(true);

			foreach ( DirectoryInfo sub in di.GetDirectories() )
			{
				DeleteCacheImages(sub);
			}
		}



		Hashtable paths = new Hashtable();

		private void Search(string HREF)
		{
			if ( HREF == null || HREF.Length == 0 || paths.Contains(HREF) ) return;
			
			Regex regex = new Regex(@"<a href=""([^""]+)""", RegexOptions.Compiled | RegexOptions.IgnoreCase);

			string data = GetURL( HREF );

			paths[HREF] = "Searched";
			Response.Write(HREF + "<BR>");
			Debug.WriteLine(HREF);

			MatchCollection matches = regex.Matches(data);

			foreach ( Match match in matches )
			{
				Search(CreateHREF(match.Groups[1].Value));
			}
		}

		private string CreateHREF(string path)
		{
			path = path.Replace("&amp;","&");

			if ( path[0] == '/' )
			{
				if ( path.StartsWith(Request.ApplicationPath) ) // dont leave the host or virtual dir
					return "http://" + Request.Url.Host + path;
				else
					return null;
			}
			else if ( path.StartsWith("http") )
			{
				if ( path.StartsWith( "http://" + Request.Url.Host + Request.ApplicationPath) ) // dont leave the host or virtual dir
					return path;
				else
					return null;
			}
			else
			{
				return "http://" + Request.Url.Host + Request.ApplicationPath + path;
			}
		}

		private WebClient client = new WebClient();
		private string GetURL(string href)
		{
			try
			{
				byte[] data = client.DownloadData(href);
				return System.Text.ASCIIEncoding.ASCII.GetString(data);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex);
				return "error";
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

		protected void Fill_Click(object sender, System.EventArgs e)
		{
			Search("http://" + Request.Url.Host + Request.ApplicationPath);
			Response.End();
		}

		protected void DeleteFill_Click(object sender, System.EventArgs e)
		{
			DeleteCacheImages(new DirectoryInfo(Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ImageVirtualDir"])));
			Search("http://" + Request.Url.Host + Request.ApplicationPath);
			Response.End();
		}

		protected void Delete_Click(object sender, System.EventArgs e)
		{
			DeleteCacheImages(new DirectoryInfo(Server.MapPath(System.Configuration.ConfigurationSettings.AppSettings["ImageVirtualDir"])));
			Response.End();
		}
	}
}
