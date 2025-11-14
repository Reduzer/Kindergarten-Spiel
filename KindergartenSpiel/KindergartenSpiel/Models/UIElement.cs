namespace KindergartenSpiel.Models
{
	class UIElement
	{
		public Guid oID { get; private set; }
		public bool bIsCorrect { get; private set; }

		public UIElement(bool bIsCorrect)
		{
			oID = Guid.NewGuid();
			this.bIsCorrect = bIsCorrect;
		}
	}
}
