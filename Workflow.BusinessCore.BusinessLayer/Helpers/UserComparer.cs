using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Helpers
{
    public class UserComparer : IEqualityComparer<User>
    {
        private Func<User, User, bool> _isEqualsUser = null;

        public UserComparer(Func<User, User, bool> isEqualsUser)
        {
            _isEqualsUser = isEqualsUser;
        }

        bool IEqualityComparer<User>.Equals(User x, User y)
        {
            if (Object.ReferenceEquals(x, y))
                return true;

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return _isEqualsUser(x, y);
        }

        int IEqualityComparer<User>.GetHashCode(User obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return 0;

            int hash = obj.Email?.GetHashCode() ?? 0;
            hash ^= obj.EmployeeID?.GetHashCode() ?? 0;
            hash ^= obj.Firstname?.GetHashCode() ?? 0;
            hash ^= obj.Lastname?.GetHashCode() ?? 0;
            hash ^= obj.Login?.GetHashCode() ?? 0;
            hash ^= obj.Name?.GetHashCode() ?? 0;
            hash ^= obj.Password?.GetHashCode() ?? 0;
            hash ^= obj.Role?.GetHashCode() ?? 0;

            return hash;
        }
    }
}
