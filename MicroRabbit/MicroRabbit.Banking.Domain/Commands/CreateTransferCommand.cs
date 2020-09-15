using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Domain.Commands
{
    public class CreateTransferCommand:TransferCommonad
    {
        public CreateTransferCommand (int from,int to,decimal amount)
        {
            From = from;
            To = To;
            Amount = amount;
        }
    }
}
