using System.Collections.Generic;

namespace Syntax
{
	public interface IRevertsElement
	{
		List<IRevertsElement> GetSequence();
	}
}