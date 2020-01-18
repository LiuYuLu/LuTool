using System.Collections.Generic;

namespace LuTool.ElementUi
{
    public class UiTreeModel
    {
        public string Label { get; set; }
        public long Value { get; set; }

        public List<UiTreeModel> Children { get; set; }
    }
}