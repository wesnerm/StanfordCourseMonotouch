
#region Usings

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using SQLite;

#endregion

namespace Photomania
{
	public partial class Photographer
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Name { get; set; }

		[Ignore]
		public List<Photo> Photos { get; set; }

		public static Photographer WithName(string name, Database database)
		{
			Photographer photographer = null;

			// This is just like Photo(Flickr)'s method.  Look there for commentary.

			if (name.Length > 0)
			{
				photographer  =
					database.Photographers.Where(p => p.Name== name).Take(1).
						SingleOrDefault();

				if (photographer == null)
				{
					photographer = new Photographer();
					photographer.Name = name;
					database.Insert(photographer);
				}
			}

			return photographer;
		}

		public override string ToString()
		{
			return string.Format("[PhotographerRec: Id={0}, name={1}, photos={2}]", Id, Name, Photos);
		}
	}
}