using System;

namespace HelthCheck.Data.Entities
{
    public interface IAuditable
    {
        DateTime CreatedDate { get; set; }

        DateTime LastModifiedDate { get; set; }
    }
}
