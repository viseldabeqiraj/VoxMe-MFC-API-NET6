namespace MFC_VoxMe_API.Dtos
{
	//use this class in case we have a form with file upload 
	//get file contents using IFormFile
	public class Document
	{
		public string Title { get; set; }
		public string Version { get; set; }
		public IFormFile File { get; set; }
	}
}
