﻿using Kurier.Common.Enums;

namespace Kurier.Common.Models.Requests
{
    public class UserRegisterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
