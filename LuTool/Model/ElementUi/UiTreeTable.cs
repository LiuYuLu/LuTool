using System.Collections.Generic;

namespace LuTool.ElementUi
{
    public class UiTreeTable<T>
    {
        public List<T> Children { get; set; }
        public bool HasChildren { get; set; }
    }
}