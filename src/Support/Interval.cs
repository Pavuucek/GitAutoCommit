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

using GitAutoCommit.Properties;

namespace GitAutoCommit.Support
{

    /// <summary>
    /// Class for interval in which checking for changes occurs
    /// </summary>
    public class Interval
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Interval"/> class.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        public Interval(int seconds)
        {
            Seconds = seconds;
        }

        /// <summary>
        /// Gets or sets the seconds.
        /// </summary>
        /// <value>
        /// The seconds.
        /// </value>
        public int Seconds { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (Seconds > 60*60)
                return string.Format(Resources.Interval_hours, Seconds/60/60);

            if (Seconds == 60*60)
                return Resources.Interval_1_hour;

            if (Seconds == 60)
                return Resources.Interval_1_minute;

            if (Seconds > 60)
                return string.Format(Resources.Interval_minutes, Seconds/60);
            if (Seconds == 1)
                return Resources.Interval_1_second;
            return string.Format(Resources.Interval_seconds, Seconds);
        }
    }
}