/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;

namespace HomeHubService.Utils
{
    /// <summary>
    /// Helper class that helps determine if the current user belongs to
    /// certain Active Directory group
    /// </summary>
    public static class GroupHelper
    {
        /// <summary>
        /// Static constructor to read values from config and assign them
        /// </summary>
        static GroupHelper()
        {
            Level1Id= ConfigurationManager.AppSettings.Get(nameof(Level1Id));
            LevelSecretId = ConfigurationManager.AppSettings.Get(nameof(LevelSecretId));
        }

        /// <summary>
        /// If the current user is part of Level 1 group
        /// </summary>
        public static bool IsLevel1
        {
            get
            {
                return IsLevel(Level1Id);
            }
        }

        /// <summary>
        /// If the current user is part of Secret Level group
        /// </summary>
        public static bool IsSecretLevel
        {
            get
            {
                return IsLevel(LevelSecretId);
            }
        }

        /// <summary>
        /// Checks if the current user belongs to the given group
        /// </summary>
        /// <param name="level">group id</param>
        /// <returns></returns>
        private static bool IsLevel(string level)
        {
            // validate the parameters
            if (string.IsNullOrWhiteSpace(level))
            {
                throw new ArgumentNullException(nameof(level));
            }

            // get the claims of the current user
            ClaimsIdentity userClaimsId = ClaimsPrincipal.Current.Identity as ClaimsIdentity;
            // get the groups in the claim
            // NOTE: In order to retrieve the groups, the manifest should have the declaration
            var groups = userClaimsId.FindAll("groups").Select(c => c.Value);

            // check if user belongs to the desired group
            return groups.Any((grp) => level.Equals(grp, StringComparison.InvariantCultureIgnoreCase));
        }

        private static readonly string Level1Id;
        private static readonly string LevelSecretId;
    }
}