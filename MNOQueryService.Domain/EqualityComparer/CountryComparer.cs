// <copyright file="PermissionInfoCategoryComparer.cs" company="Leatherback">
// Copyright (c) Leatherback. All rights reserved.
// </copyright>

using MNOQueryService.Domain.Entities;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MNOQueryService.Domain.EqualityComparer
{
    public class CountryComparer : IEqualityComparer<Country>
    {
        public bool Equals(Country? x, Country? y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }
            
            return x.CountryCode == y.CountryCode || x.CountryIso == y.CountryIso ;
        }

        public int GetHashCode([DisallowNull] Country obj)
        {
            return base.GetHashCode();
        }
    }
}
