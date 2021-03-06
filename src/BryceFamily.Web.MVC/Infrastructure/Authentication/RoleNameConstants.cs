﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Infrastructure.Authentication
{
    public static class RoleNameConstants
    {
        public const string UserRole = "user";
        public const string AdminRole = "admin";
        public const string SuperAdminRole = "superAdmin";
        public const string AllAdminRoles = "admin,superAdmin";
        public const string AllRoles = "admin,superAdmin,user";
    }
}
