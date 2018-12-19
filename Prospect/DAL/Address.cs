using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.cloud.prospect
{
    public partial class Address : DALModel<Address>
    {
        public static List<Address> Addresses;
        public Address()
            : base("S_Address")
        {

        }

        public void Insert()
        {
            CommonInsert(this, new string[] { "DetailAddress" });
            CommonSelectSingleRow(this,
                "Address_Key = (SELECT MAX(Address_Key) FROM S_Address NOLOCK)", null);
        }

        public void Delete()
        {
            CommonDelete(this, "Address_Key = " + Address_Key);
        }

        public List<Address> SelectAll()
        {
            if (Addresses == null)
                Addresses = CommonSelect(this, "1 = 1", null);
            return Addresses;
        }

    }
}
