﻿using Microsoft.AspNetCore.Identity;

namespace Project2.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public bool IsPremium { get; set; } = false;
        public int CoinCount { get; set; } = 10;
    }
}
