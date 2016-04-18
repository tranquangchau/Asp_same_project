namespace ImageBrowser.Controls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Entities;

	/// <summary>
	///		Summary description for ImageBrowser.
	/// </summary>
	public partial  class ImageBrowser : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string path = ImageTools.GetPath(Request["path"]);

			DirectoryWrapper data = new DirectoryWrapper(path);

			// draw navigation
			HtmlTools.RendenderLinkPath( ImageBrowserPanel.Controls, path, Request.Path + "?page=" + "ImageBrowser.ascx&path=" );

			ImageBrowserPanel.Controls.Add(HtmlTools.HR);


			// setup titles, headings etc.
			if ( data.Name != null && data.Name.Length > 0 )
				((Literal)this.Page.FindControl("title")).Text = System.Configuration.ConfigurationSettings.AppSettings["SiteTitle"] + " - " + data.Name;
			else
				((Literal)this.Page.FindControl("title")).Text = System.Configuration.ConfigurationSettings.AppSettings["SiteTitle"] + " - My Pictures";


			// pump out the blurb
			Label blurb = new Label();
			blurb.Text = data.Blurb;
			ImageBrowserPanel.Controls.Add(blurb);

			// draw a line if appropriate
			if ( blurb.Text.Length > 0 && ( data.Directories.Count > 0 || data.Images.Count > 0)  )
			{
				ImageBrowserPanel.Controls.Add(HtmlTools.HR);
			}


			// draw subdirectories
			ImageBrowserPanel.Controls.Add( HtmlTools.RenderDirectoryTable(4,data, Request.Path + "?page=" + "ImageBrowser.ascx&path=") );


			// draw a line if appropriate
			if ( data.Directories.Count > 0 && data.Images.Count > 0 )
			{
				ImageBrowserPanel.Controls.Add(HtmlTools.HR);
			}

			// draw images
			ImageBrowserPanel.Controls.Add( HtmlTools.RenderImageTable(7,0,data, Request.Path + "?page=" + "WebImage.ascx&path=") );
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
		
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{

		}
		#endregion
	}
}
