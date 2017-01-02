/***********************************************************************************************
 * Copyrights 2016 Sameer Khandekar
 * License: MIT License
***********************************************************************************************/
using System;

namespace HomeHub.Model
{
    /// <summary>
    /// Model to transmit data between client and WebAPI
    /// </summary>
    public class LightSwitchModel
    {
        /// <summary>
        /// Id of the light
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Indicator if it is on
        /// </summary>
        public bool IsOn { get; set; }
    }
}
