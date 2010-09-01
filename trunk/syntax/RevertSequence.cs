using System.Collections.Generic;

namespace Syntax
{
	public class RevertSequence : IRevertsElement
	{
		List<IRevertsElement> Operands { get; set; }
		List<IRevertsElement> Actions { get; set; }

        public List<IRevertsElement> GetSequence()
        {
            throw new System.NotImplementedException();
        }
    }
}