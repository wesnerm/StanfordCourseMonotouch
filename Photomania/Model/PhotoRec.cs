using System;
using System.Collections.Generic;
using SQLite;

namespace Photomania
{
    public class PhotoRec
    {
        public PhotoRec()
        {
        }

        [PrimaryKey, AutoIncrement]
        public int Id {get; set; }
        public string ImageUrl  {get; set; }
        public string Subtitle  {get; set; }
        public string Title  {get; set; }
        public string Unique  {get; set; }

        [Indexed]
        public PhotographerRec WhoTook  {get; set; }

        public override string ToString()
        {
            return string.Format("[PhotoRec: Id={0}, title={1}]", Id, Title);
        }
    }

    public class PhotographerRec
    {
        public PhotographerRec()
        {
        }

        [PrimaryKey, AutoIncrement]
        public int Id  {get; set; }
        public string Name  {get; set; }
        public List<PhotoRec> Photos  { get; set; }

        public override string ToString()
        {
            return string.Format("[PhotographerRec: Id={0}, name={1}, photos={2}]", Id, Name, Photos);
        }
    }
}

