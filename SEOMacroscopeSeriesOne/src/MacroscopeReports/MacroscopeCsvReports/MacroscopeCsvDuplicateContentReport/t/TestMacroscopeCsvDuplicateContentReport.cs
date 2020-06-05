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
using System.IO;
using NUnit.Framework;

namespace SEOMacroscope
{

  [TestFixture]
  public class TestMacroscopeCsvDuplicateContentReport : Macroscope
  {

    /**************************************************************************/

    [Test]
    public void TestWriteCsv ()
    {
      MacroscopeJobMaster JobMaster = new MacroscopeJobMaster( MacroscopeConstants.RunTimeMode.LIVE );
      MacroscopeCsvDuplicateContentReport ReportFile = new MacroscopeCsvDuplicateContentReport();
      List<MacroscopeCsvDuplicateContentReport.OutputWorksheet> SelectedOutputWorksheets = new List<MacroscopeCsvDuplicateContentReport.OutputWorksheet>();
      SelectedOutputWorksheets.Add( MacroscopeCsvDuplicateContentReport.OutputWorksheet.CHECKSUMS);
      SelectedOutputWorksheets.Add( MacroscopeCsvDuplicateContentReport.OutputWorksheet.ETAGS );
      SelectedOutputWorksheets.Add( MacroscopeCsvDuplicateContentReport.OutputWorksheet.PAGES );
      SelectedOutputWorksheets.Add( MacroscopeCsvDuplicateContentReport.OutputWorksheet.TITLES );
      foreach ( MacroscopeCsvDuplicateContentReport.OutputWorksheet SelectedOutputWorksheet in SelectedOutputWorksheets )
      {
        string Filename = string.Join( ".", Path.GetTempFileName(), ".csv" );
        ReportFile.WriteCsv( JobMaster: JobMaster, SelectedOutputWorksheet: SelectedOutputWorksheet, OutputFilename: Filename );
        Assert.IsTrue( File.Exists( Filename ) );
        File.Delete( Filename );
      }
    }

    /**************************************************************************/

  }

}
