#region License

/*
Copyright (c) 2011 Gareth Lennox (garethl@dwakn.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
    this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
    this list of conditions and the following disclaimer in the documentation
    and/or other materials provided with the distribution.
    * Neither the name of Gareth Lennox nor the names of its
    contributors may be used to endorse or promote products derived from this
    software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/

#endregion

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;

namespace GitAutoCommit
{
    /// <summary>
    ///     Helper for default fonts etc
    /// </summary>
    public static class FontHelper
    {
        /// <summary>
        ///     Static constructor
        /// </summary>
        static FontHelper()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Runtime)
            {
                DefaultGuiFont = ChooseFont(8.25f, "Segoe UI", "Calibri", "Tahoma", "Arial", "sans-serif");
            }
            else
            {
                DefaultGuiFont = ChooseFont(8.25f, "Tahoma", "Arial", "sans-serif");
            }

            var headingFont = ChooseFont(8.25f, "Segoe UI Light", "Segoe UI", "Calibri", "Tahoma", "Arial", "sans-serif");

            SubHeadingGuiFont = new Font(headingFont.FontFamily, 13f, FontStyle.Regular);
            HeadingGuiFont = new Font(headingFont.FontFamily, 20f, FontStyle.Regular);

            MonospaceFont = ChooseFont(9.75f, "Consolas", "Courier New", "monospace");
        }

        /// <summary>
        ///     Our default gui font
        /// </summary>
        public static Font DefaultGuiFont { get; set; }

        /// <summary>
        ///     Heading gui font
        /// </summary>
        public static Font HeadingGuiFont { get; set; }

        /// <summary>
        ///     Heading gui font
        /// </summary>
        public static Font SubHeadingGuiFont { get; set; }

        /// <summary>
        ///     Monospace font
        /// </summary>
        public static Font MonospaceFont { get; set; }

        /// <summary>
        ///     Given a list of fonts, returns the first one found
        /// </summary>
        /// <param name="size">Size of the font</param>
        /// <param name="requestedFonts">The list of requested fonts</param>
        /// <returns>The font, or the system default font if none of the fonts were found</returns>
        private static Font ChooseFont(float size, params string[] requestedFonts)
        {
            var fonts = new InstalledFontCollection();

            foreach (var name in requestedFonts)
            {
                foreach (var family in fonts.Families)
                    if (family.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                        return new Font(family, size);
            }

            return SystemFonts.DefaultFont;
        }
    }
}