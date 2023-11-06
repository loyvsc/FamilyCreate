using GraphX.Common.Models;

namespace FamilyCreate.Controls.GraphArea
{

    public class DataEdge : EdgeBase<DataVertex>
    {
        public DataEdge(DataVertex source, DataVertex target)
            : base(source, target, 1) { }

        public DataEdge() : base(null, null) { }

        public string Text { get; set; }

        public override string ToString() => Text;
    }
}
