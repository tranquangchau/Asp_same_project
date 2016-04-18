using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;

namespace ImageBrowser.Entities
{
	/// <summary>
	/// Holds all the information of the contents of a directoty
	/// </summary>
	public class DirectoryWrapper
	{
		private ArrayList directories =  new ArrayList();
		private ArrayList images =  new ArrayList();
		private string directory;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dir">Directory to hold contents of</param>
		public DirectoryWrapper(string dir)
		{
			directory = dir;

			// add the sub-directories
			foreach ( string s in Directory.GetDirectories(ImageTools.RootDirectory + "/" + directory) )
			{
				string[] path = s.Replace( "\\", "/" ).Split('/');

				if ( path[path.Length - 1] != "thumbs" && path[path.Length - 1] != "webpics" && path[path.Length - 1][0] != '_' )
				{
					directories.Add(ImageTools.GetSubDirectoryWrapper(directory + "/" + path[path.Length - 1]));
				}
			}

			// add pictures
			foreach ( string s in Directory.GetFiles(ImageTools.RootDirectory + "/" + directory) )
			{
				if (  s[0] != '_' )
				{
					string extension = null;
					if (s.IndexOf(".") > 0)
					{
						string[] parts = s.Split('.');
						extension = parts[parts.Length - 1];
					}
					if ( extension == null ) continue;

					extension = extension.ToLower();

					if ( extension == "jpg" ||
						extension == "png" ||
						extension == "gif" )
					{
						string[] path = s.Replace(@"\","/").Split('/');
						images.Add(ImageTools.GetImageWrapper(directory + "/" + path[path.Length - 1]));
					}
				}
			}
		}

		/// <summary>
		/// Subdirectories
		/// </summary>
		public ArrayList Directories
		{
			get
			{
				return directories;
			}
		}

		/// <summary>
		/// images
		/// </summary>
		public ArrayList Images
		{
			get
			{
				return images;
			}
		}

		/// <summary>
		/// The name of this directory
		/// </summary>
		public string Name
		{
			get
			{
				string[] paths = directory.Replace(@"\","/").Split('/');
				return paths[paths.Length - 1];
			}
		}
		
		/// <summary>
		/// The blurb - loaded from a file with the same name as the directory,
		/// in the directory
		/// </summary>
		public string Blurb
		{
			get
			{
				FileStream fs = null;
				try
				{

					string[] dirs = directory.Replace(@"\","/").Split('/');
					string name = dirs[dirs.Length - 1];

					if ( name == null || name.Length == 0 ) name = "Home";

					if ( File.Exists(ImageTools.RootDirectory + "/" + directory + "/" + name + ".txt" ) )
					{
						fs = File.OpenRead(ImageTools.RootDirectory + "/" + directory + "/" + name + ".txt" );

						byte[] b = new Byte[fs.Length];
						fs.Read(b,0,(int)fs.Length);
						fs.Close();
						return System.Text.ASCIIEncoding.ASCII.GetString(b);
					}
				}
				catch(Exception)
				{
					if ( fs != null && fs.CanRead ) fs.Close();
				}
				return "";
			}
		}
	}

	/// <summary>
	/// Wraps a directory object ( not to be confused with the directory that your in.
	/// </summary>
	public class SubDirectoryWrapper
	{
		private string directory;
		private string name;
		private string defaultImage;

		public SubDirectoryWrapper(string dir, string defaultFolderImage)
		{
			directory = dir;
			defaultImage = defaultFolderImage;

			string[] dirs = directory.Replace(@"\","/").Split('/');
			name = dirs[dirs.Length - 1];
		}

		/// <summary>
		/// Name of the subdirectory
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}

		/// <summary>
		/// Imahe to use to represent the subdirectory
		/// </summary>
		public string Src
		{
			get
			{
				string parent = ImageTools.RootDirectory + "/" + directory;
				string[] files = Directory.GetFiles( parent, name + @".*");

				if ( files.Length > 0 )
				{
					string file = null;

					foreach ( string s in files )
					{
						string extension = null;
						if (s.IndexOf(".") > 0)
						{
							string[] parts = s.Split('.');
							extension = parts[parts.Length - 1];
						}
						if ( extension == null ) continue;

						extension = extension.ToLower();

						if ( extension == "jpg" ||
							extension == "png" ||
							extension == "gif" )
						{
							string[] filepath = s.Replace(@"\","/").Split('/');
							file = directory + "/" + filepath[filepath.Length - 1];
							break;
						}
					}
					if ( file != null )
					{
						string[] filename = file.Replace(@"\",@"/").Split('/');

						string thumb = string.Join("/",filename,0,filename.Length - 1) + "/thumbs/" + filename[filename.Length - 1];

						ImageTools.CreateImage(file,thumb,100);
						return ImageTools.VirtualDirectory + thumb;
					}
				}
				return defaultImage;
			}
		}

		/// <summary>
		/// The link to use to get to the subdirectory
		/// </summary>
		public string HREF
		{
			get
			{
				return ImageTools.VirtualDirectory + directory;
			}
		}

	}

	/// <summary>
	/// Wraps an image object for when you ar looking at it in a directory
	/// </summary>
	public class ImageWrapper
	{
		private string file;
		private string name;

		public ImageWrapper(string fileName)
		{
			file = fileName;
			string[] dirs = file.Replace(@"\","/").Split('/');
			name = dirs[dirs.Length - 1];

		}
	
		/// <summary>
		/// Name of the file
		/// </summary>
		public string Name
		{
			get
			{
				return name;
			}
		}

		/// <summary>
		/// Link to use when they click on it
		/// This will take them to a 'web friendly' version of the image
		/// </summary>
		public string WebImageHref
		{
			get
			{
				string[] name = file.Split('/');

				string thumb = string.Join("/",name,0,name.Length - 1) + "/webpics/" + name[name.Length - 1];

				ImageTools.CreateImage(ImageTools.GetPath(file),ImageTools.GetPath(thumb),640);
				
				
				return ImageTools.VirtualDirectory + ImageTools.GetPath(thumb);
			}
		}

		/// <summary>
		/// The path of the orgional image
		/// </summary>
		public string FullImageHref
		{
			get
			{
				return ImageTools.VirtualDirectory + ImageTools.GetPath(file);
			}
		}

		/// <summary>
		/// The path of the thumbnail
		/// </summary>
		public string ThumbHref
		{
			get
			{
				string[] name = file.Split('/');

				string thumb = string.Join("/",name,0,name.Length - 1) + "/thumbs/" + name[name.Length - 1];

				ImageTools.CreateImage(ImageTools.GetPath(file),ImageTools.GetPath(thumb),100);

				return ImageTools.VirtualDirectory + ImageTools.GetPath(thumb);
			}
		}
	}
}
