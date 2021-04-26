using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Common
{
    public enum Status
    {
        All = 0,
        Active = 1, // đã kích hoạt
        WaitActive = 2, // không kích hoạt
        Hide= 3 // Tạm ẩn
    }
    public enum ProductStatus
    {
        Active = 1,
        WaitActive = 2, // chờ kích hoạt

    }
    public enum BillStatus
    {
        New,
        InProgress,
        Returned,
        Cancelled,
        Completed
    }
    public enum PaymentMethod
    {
        CashOnDelivery,
        OnlinBanking,
        PaymentGateway,
        Visa,
        MasterCard,
        PayPal,
        Atm
    }
}
