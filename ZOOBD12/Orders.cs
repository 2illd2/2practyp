//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZOOBD12
{
    using System;
    using System.Collections.Generic;
    
    public partial class Orders
    {
        public int ID_Order { get; set; }
        public int Customer_ID { get; set; }
        public int Animal_ID { get; set; }
        public int Quantity { get; set; }
        public System.DateTime OrderDate { get; set; }
    
        public virtual Animals Animals { get; set; }
        public virtual Customers Customers { get; set; }
    }
}
