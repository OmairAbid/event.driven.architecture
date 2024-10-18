﻿using QueryRepository.Repository;

namespace Application.BoundedContexts.UserAccountManagement.QueryObjects
{
    public class AdminInfo : QueryEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
