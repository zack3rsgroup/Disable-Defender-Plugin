using System;
using System.Security.Principal;

namespace DisableWindowsDefender
{
	internal static class User
	{
		private static bool? _isAdministrator;

		public static bool IsAdministrator
		{
			get
			{
				bool? isAdministrator = _isAdministrator;
				if (!isAdministrator.HasValue)
				{
					bool? flag = (_isAdministrator = IsUserAdministrator());
					return flag.Value;
				}
				return isAdministrator.GetValueOrDefault();
			}
		}

		private static bool IsUserAdministrator()
		{
			try
			{
				WindowsIdentity current = WindowsIdentity.GetCurrent();
				if (current == null)
				{
					return false;
				}
				return new WindowsPrincipal(current).IsInRole(WindowsBuiltInRole.Administrator);
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}
		}
	}
}
