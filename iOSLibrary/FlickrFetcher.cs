using System;
using MonoTouch.Foundation;

namespace WM
{
    public static class FlickrFetcher
    {
	public const string FlickrApiKey = "--your api key--";

        public const string FlickrPhotoTitle = "title";
        public const string FlickrPhotoDescription = "description._content";
        public const string FlickrPlaceName = "_content";
        public const string FlickrPhotoId = "id";
        public const string FlickrLatitude = "latitude";
        public const string FlickrLongitude = "longitude";
        public const string FlickrPhotoOwner = "ownername";
        public const string FlickrPhotoPlaceName = "derived_place";
        public const string FlickrTags = "tags";

	    public static NSArray TopPlaces
	    {
		    get
		    {
			    const string request = "http://api.flickr.com/services/rest/?method=flickr.places.getTopPlacesList&place_type_id=7";
			    return (NSArray) ExecuteFlickrFetch(request).ValueForKeyPath((NSString) "places.place");
		    }
	    }

		public static NSArray PhotosInPlace(NSDictionary place, int maxResults)
        {
			const string request = "http://api.flickr.com/services/rest/?user_id=48247111@N07&format=json&nojsoncallback=1&extras=original_format,tags,description,geo,date_upload,owner_name&page=1&method=flickr.photos.search";
		    return (NSArray) ExecuteFlickrFetch(request).ValueForKeyPath((NSString) "photos.photo");
        }

		public static string UrlStringForPhoto(NSDictionary photo, FlickrPhotoFormat format)
		{
			var farm = photo["farm"];
			var server = photo["server"];
			var id = photo["id"];
			var secret = photo["secret"];
			

			if (format == FlickrPhotoFormat.Original)
				secret = photo["originalsecret"];

			string fileType = "jpg";
			if (format == FlickrPhotoFormat.Original)
				fileType = (NSString)photo["originalformat"];

			if (farm == null
				|| server == null
				|| id == null
				|| secret == null)
				return null;

			var formatString = "s";
			switch(format)
			{
			case FlickrPhotoFormat.Square:
				formatString = "s";
				break;
			case FlickrPhotoFormat.Large:
				formatString = "b";
				break;
			case FlickrPhotoFormat.Original:
				formatString = "o";
				break;
			}

			return string.Format("http://farm{0}.static.flickr.com/{1}/{2}_{3}_{4}.{5}", farm, server, id, secret,
			                     formatString, fileType);
		}

		public static NSUrl UrlForPhoto(NSDictionary photo, FlickrPhotoFormat format)
        {
			return new NSUrl(UrlStringForPhoto(photo, format));
        }

        public static NSArray RecentGeoreferencedPhotos()
        {
	        const string request = "http://api.flickr.com/services/rest/?method=flickr.photos.search&per_page=500&license=1,2,4,7&has_geo=1&extras=original_format,tags,description,geo,date_upload,owner_name,place_url";
	        return (NSArray) ExecuteFlickrFetch(request).ValueForKeyPath((NSString)"photos.photo");
        }

		public static NSDictionary ExecuteFlickrFetch(string query)
		{
			query = string.Format(@"{0}&format=json&nojsoncallback=1&api_key={1}", query, FlickrApiKey);
			query = Uri.EscapeUriString(query);

			var jsonData = NSData.FromUrl(new NSUrl(query));

			NSDictionary results = null;
			NSError error = new NSError();
			if (jsonData != null)
				results =
					(NSDictionary)
					NSJsonSerialization.Deserialize(jsonData,
					                                NSJsonReadingOptions.MutableContainers | NSJsonReadingOptions.MutableLeaves, out error);
			return results;
		}



    }

	public enum FlickrPhotoFormat
	{
		Square = 1,
		Large = 2,
		Original = 64,
	};
}

