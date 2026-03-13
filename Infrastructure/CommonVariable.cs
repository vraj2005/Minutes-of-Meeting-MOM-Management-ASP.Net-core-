namespace MOM_Project.Infrastructure
{
	public static class CommonVariable
	{
		private static readonly IHttpContextAccessor _httpContextAccessor;

		static CommonVariable()
		{
			_httpContextAccessor = new HttpContextAccessor();
		}

		public static int? UserID()
		{
			var value = _httpContextAccessor.HttpContext?.Session.GetString("UserID");
			return string.IsNullOrWhiteSpace(value) ? null : Convert.ToInt32(value);
		}

		public static string? UserName()
			=> _httpContextAccessor.HttpContext?.Session.GetString("UserName");

		public static string? Email()
			=> _httpContextAccessor.HttpContext?.Session.GetString("Email");
	}
}
