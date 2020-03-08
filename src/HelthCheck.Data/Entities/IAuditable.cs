using System;

namespace HelthCheck.Web.Data
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }

        DateTime LastModifiedDate { get; set; }
    }
}
