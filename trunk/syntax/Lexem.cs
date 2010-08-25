//using ...

namespace Syntax
{
	public class Lexem : IRevertsElement
	{
		private List<IRevertsElement> children;
		
		public List<IRevertsElement> GetSequence()
		{
			return children;
		}
		
		public string Text { get; set; }
		
		public LexemType LxType { get; set; }
	}
	
	public enum LexemType
	{
		sysword,class,method,var,separator
	}
}