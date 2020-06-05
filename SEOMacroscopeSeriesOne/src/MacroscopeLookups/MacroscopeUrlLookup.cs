﻿/*

	This file is part of SEOMacroscope.

	Copyright 2020 Jason Holland.

	The GitHub repository may be found at:

		https://github.com/nazuke/SEOMacroscope

	SEOMacroscope is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	SEOMacroscope is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with SEOMacroscope.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;
using System.Text;

namespace SEOMacroscope
{

  /// <summary>
  /// MacroscopeUrlLookup is responsible for maintaining a centralized
  /// store of URL to ulong mappings.
  /// </summary>

  [Serializable()]
  public static class MacroscopeUrlLookup
  {

    /**************************************************************************/

    private static object Locker;
    public static ulong Counter;
    public static Dictionary<string, ulong> Mappings;

    /**************************************************************************/

    static MacroscopeUrlLookup ()
    {
      Locker = new object();
      Counter = 0;
      Mappings = new Dictionary<string, ulong>( 4096 );
    }

    /**************************************************************************/

    public static void Clear ()
    {
      Counter = 0;
      Mappings.Clear();
    }

    /**************************************************************************/

    public static ulong Lookup ( string Url )
    {

      ulong stored;

      lock( Locker )
      {

        if( Mappings.ContainsKey( key: Url ) )
        {
          stored = Mappings[ Url ];
        }
        else
        {
          stored = Counter;
          Mappings.Add( key: Url, value: stored );
          Counter++;
        }

      }

      return ( stored );

    }

    /**************************************************************************/

  }

}
