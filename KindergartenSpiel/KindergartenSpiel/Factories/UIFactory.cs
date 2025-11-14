namespace KindergartenSpiel.Factories
{
	public class UIFactory
	{
		private static UIFactory oInstance;

		private UIFactory()
		{
		
		}

		public static UIFactory Instance
		{
			get {
				if (oInstance == null) {
					oInstance = new UIFactory();
				}
				return oInstance;
			}
		}
	}
}
