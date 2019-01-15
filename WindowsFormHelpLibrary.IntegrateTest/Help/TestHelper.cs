using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WindowsFormHelpLibrary.FilterHelp;
using WindowsFormHelpLibrary.SortableBindingList;

namespace WindowsFormHelpLibrary.Tests.Help
{
    public static class TestHelper
    {
        public static TypeFilter ParsFilter(PropertiesFilter filters)
        {
            var result = new TypeFilter(new Dictionary<string, TypeFilter>(filters.Count));
            foreach (var filter in filters.Values.Where(f => f.Value != null))
            {
                switch (filter.Value)
                {
                    case PropertiesFilter innerFilter:
                        result.PropertyFilters.Add(filter.Key.Name, ParsFilter(innerFilter));
                        break;
                    case Delegate predecate:
                        result.PropertyFilters.Add(filter.Key.Name, new TypeFilter(predecate));
                        break;
                    default:
                        result.PropertyFilters.Add(filter.Key.Name, new TypeFilter(filter.Value.ToString()));
                        break;
                }
            }
            return result.PropertyFilters.Count > 0 ? result : null;
        }
    }

    public class MainBitch
    {
        public Bitch Bitch { get; set; }
    }
    public class Bitch
    {
        public string Name { get; set; }
        public Owner Owner { get; set; }

        public Bitch(string name, Owner owner)
        {
            Name = name;
            Owner = owner;
        }
    }

    public class Owner
    {
        public string NameOwner { get; set; }

        public Owner(string name)
        {
            NameOwner = name;
        }
    }
}