using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS.Messages
{
    public class UserCustomizedMessage : ValueChangedMessage<int>
    {
        public UserCustomizedMessage(int value) : base(value)
        {

        }
    }
}
