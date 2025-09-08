using CommerceBack.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceBack.Data.Interfaces
{
    public interface IPaymentMethodRepository
    {
        List<PaymentMethod> GetAll();
        PaymentMethod? GetById(int id);
        bool Save(PaymentMethod paymentMethod);
        bool Delete(int id);
    }
}
