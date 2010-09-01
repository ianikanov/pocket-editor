using System.Collections.Generic;

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
        l_sysword, l_class, l_method, l_var, l_separator
	}
}