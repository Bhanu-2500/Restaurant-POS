using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS
{
    public class NavigationStore
    {
        public NavigationStore(object currentView)
        {
            CurrentView = currentView;
        }

        public Object CurrentView { get; set; }
    }
}
