﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nop.Plugin.Widgets.BlogCategory.Authorization.Requirements;

namespace Nop.Plugin.Widgets.BlogCategory.Authorization.Policies
{
    public class CustomerRoleAuthorizationPolicy : AuthorizationHandler<CustomerRoleRequirement>
    {
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomerRoleRequirement requirement)
        {
            if (await requirement.IsCustomerInRoleAsync())
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
