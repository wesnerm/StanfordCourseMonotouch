using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MonoTouch.CoreFoundation;
using MonoTouch.Foundation;
using SQLite;

namespace Photomania
{
	public class Database : SQLiteConnection
	{
		public DispatchQueue Queue = new DispatchQueue("Database");

		public Database(string path) : base(path)
		{
			CreateTable<Photo>();
			CreateTable<Photographer>();
		}

		public static Database ConstructDatabase()
		{
			var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			path = Path.Combine(path, "Photomania.db");
			return new Database(path);
		}

		public TableQuery<Photographer> Photographers
		{
			get { return Table<Photographer>(); }
		}

		public TableQuery<Photo> Photos
		{
			get { return Table<Photo>(); }
		}

		public TableQuery<Photo> PhotosFrom(int photoId)
		{
			return Table<Photo>().Where(p => p.PhotographerId == photoId);
		}
	}
}
