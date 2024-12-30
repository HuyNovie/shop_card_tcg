using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DoanLTW.Models
{
    public class UsersRoleProvider : RoleProvider
    {
        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (TCGShopDBContext context = new TCGShopDBContext())
            {
                var userRoles = (from user in context.Users
                                 join roleMapping in context.UserRolesMappings
                                 on user.ID equals roleMapping.UserID
                                 join role in context.RoleMasters
                                 on roleMapping.RoleID equals role.ID
                                 where user.UserName == username
                                 select role.RoleName).ToArray();

                return userRoles;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (TCGShopDBContext context = new TCGShopDBContext())
            {
                return (from user in context.Users
                        join roleMapping in context.UserRolesMappings
                        on user.ID equals roleMapping.UserID
                        join role in context.RoleMasters
                        on roleMapping.RoleID equals role.ID
                        where user.UserName == username && role.RoleName == roleName
                        select role).Any();
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }

}