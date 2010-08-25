//using ...

namespace Syntax
{
	public class RevertSequence : IRevertsElement
	{
		List<IRevertsElement> Operands { get; set; }
		List<IRevertsElement> Actions { get; set; }
	}
}