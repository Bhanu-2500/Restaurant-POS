using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS.Models
{
    public partial class TemporaryCategory : ObservableObject
    {
        [ObservableProperty]
        public string name;
        [ObservableProperty]
        public string imagePath;
        [ObservableProperty]
        public bool isEnabled;

    }
}
