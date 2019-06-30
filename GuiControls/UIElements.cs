using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GuiControls
{
    [JsonArray]
    public class UIElements : IEnumerable<UIElement>
    {
        private readonly Dictionary<string, UIElement> _elements;

        private UIElements()
        {
            _elements = new Dictionary<string, UIElement>();
        }

        public static UIElements Create()
        {
            return new UIElements();
        }

        public void Add(UIElement element)
        {
            _elements.Add(element.Name, element);
        }

        public void Remove(string elementKey)
        {
            _elements.Remove(elementKey);
        }

        public UIElement this[string elementKey]
        {
            get
            {
                return _elements[elementKey];
            }
        }

        public IEnumerator<UIElement> GetEnumerator()
        {
            return _elements.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}