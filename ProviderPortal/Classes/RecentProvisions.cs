using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls.Expressions;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Tribal.SkillsFundingAgency.ProviderPortal.Entities;

namespace Tribal.SkillsFundingAgency.ProviderPortal.Classes
{
    /// <summary>
    /// Stores the most recently accessed providers and organisations for a user.
    /// </summary>
    public class RecentProvisionCache
    {
        /// <summary>
        /// Gets or sets the providers.
        /// </summary>
        /// <value>
        /// The providers.
        /// </value>
        public List<SelectListItem> Providers { get; set; }
        /// <summary>
        /// Gets or sets the organisations.
        /// </summary>
        /// <value>
        /// The organisations.
        /// </value>
        public List<SelectListItem> Organisations { get; set; }
    }

    /// <summary>
    /// Manager class for recording the most recently accessed providers and organisations for a user.
    /// </summary>
    public class RecentProvisions
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the cache.
        /// </summary>
        /// <value>
        /// The cache.
        /// </value>
        public RecentProvisionCache Cache { get; set; }

        /// <summary>
        /// Gets the maximum items stored in the cache.
        /// </summary>
        /// <value>
        /// The maximum items.
        /// </value>
        public int MaxItems { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecentProvisions"/> class.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public RecentProvisions(string userId)
        {
            UserId = userId;
            MaxItems = 10;
            Load(false);
        }

        /// <summary>
        /// Adds the specified provider or organisation.
        /// </summary>
        /// <param name="id">The identifier, use P1234 for providers or O1234 for organisations.</param>
        /// <param name="name">The name.</param>
        public void Add(string id, string name)
        {
            var list = id.StartsWith("P")
                ? Cache.Providers
                : Cache.Organisations;
            if (list.Count() == MaxItems)
            {
                list.RemoveAt(list.Count() - 1);
            }
            list.RemoveAll(x => x.Value == id);
            list.Insert(0, new SelectListItem {Text = name, Value = id});
            Save();
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetProviders()
        {
            return Cache.Providers;
        }

        /// <summary>
        /// Gets the organisations.
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetOrganisations()
        {
            return Cache.Organisations;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        /// <param name="refresh">Force a reload of the data</param>
        public void Load(bool refresh = false)
        {
            Cache = (RecentProvisionCache)CacheManagement.CacheHandler.Get(CacheKey);
            if (Cache != null && !refresh) return;

            var db = new ProviderPortalEntities();
            var items = db.UserProvisionHistories.Where(x => x.UserId == UserId);
            Cache = new RecentProvisionCache
            {
                Organisations = items
                    .Where(x => x.OrganisationId != null)
                    .OrderBy(x=> x.DisplayOrder)
                    .Select(x => new SelectListItem
                    {
                        Text = x.Organisation.OrganisationName,
                        Value = "O" + x.OrganisationId
                    }).ToList(),
                Providers = items
                    .Where(x => x.ProviderId != null)
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new SelectListItem
                    {
                        Text = x.Provider.ProviderName,
                        Value = "P" + x.ProviderId
                    }).ToList(),
            };
            CacheManagement.CacheHandler.Add(CacheKey, Cache);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        private void Save()
        {
            CacheManagement.CacheHandler.Add(CacheKey, Cache);

            var db = new ProviderPortalEntities();
            foreach (UserProvisionHistory item in db.UserProvisionHistories.Where(x => x.UserId == UserId).ToList())
            {
                db.Entry(item).State = EntityState.Deleted;
            }
            db.SaveChanges();
            int displayOrder = 0;
            foreach (var item in Cache.Organisations)
            {
                db.UserProvisionHistories.Add(new UserProvisionHistory
                {
                    UserId = UserId,
                    OrganisationId = Int32.Parse(item.Value.Substring(1)),
                    DisplayOrder = ++displayOrder
                });
            }
            displayOrder = 0;
            foreach (var item in Cache.Providers)
            {
                db.UserProvisionHistories.Add(new UserProvisionHistory
                {
                    UserId = UserId,
                    ProviderId = Int32.Parse(item.Value.Substring(1)),
                    DisplayOrder = ++displayOrder
                });
            }
            db.SaveChanges();
        }

        /// <summary>
        /// Gets the cache key.
        /// </summary>
        /// <value>
        /// The cache key.
        /// </value>
        private string CacheKey
        {
            get { return UserId + "_RecentProvisions"; }
        }
    }
}