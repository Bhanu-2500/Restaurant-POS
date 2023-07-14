using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS.Messages
{
    public class ViewUpdated : ValueChangedMessage<string>
    {

        public ViewUpdated(string value) : base(value)
        {
        }
    }
}
