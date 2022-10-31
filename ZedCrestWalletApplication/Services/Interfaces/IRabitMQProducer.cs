using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZedCrestWalletApplication.Services.Interfaces
{
   public interface IRabitMQProducer
    {
        public void SendInterestToAccountMessage<T>(T message);
    }
}
