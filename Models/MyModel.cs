using Prism.Mvvm;

namespace LabTask2.Models
{
    public partial class MyModel : BindableBase
    {
        private string _image = string.Empty;
        public string Image
        {
            get => _image;
            set => SetProperty(ref _image, value);
        }

        private string _hello = string.Empty;
        public string Hello
        {
            get => _hello;
            set => SetProperty(ref _hello, value);
        }

        private string _welcome = string.Empty;
        public string Welcome
        {
            get => _welcome;
            set => SetProperty(ref _welcome, value);
        }

        // keep if you still use them elsewhere
        private string _name = string.Empty;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int _value;
        public int Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}
