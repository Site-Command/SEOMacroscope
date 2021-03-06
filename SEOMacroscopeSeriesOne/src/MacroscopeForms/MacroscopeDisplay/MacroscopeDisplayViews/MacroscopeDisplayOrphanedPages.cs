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
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

namespace SEOMacroscope
{

  public sealed class MacroscopeDisplayOrphanedPages : MacroscopeDisplayListView
  {

    /**************************************************************************/

    private const int COL_URL = 0;
    private const int COL_STATUS_CODE = 1;
    private const int COL_STATUS = 2;

    /**************************************************************************/

    public MacroscopeDisplayOrphanedPages ( MacroscopeMainForm MainForm, ListView TargetListView )
      : base( MainForm, TargetListView )
    {

      this.MainForm = MainForm;
      this.DisplayListView = TargetListView;

      if ( this.MainForm.InvokeRequired )
      {
        this.MainForm.Invoke(
          new MethodInvoker(
            delegate
            {
              this.ConfigureListView();
            }
          )
        );
      }
      else
      {
        this.ConfigureListView();
      }

    }

    /**************************************************************************/

    protected override void ConfigureListView ()
    {
      if ( !this.ListViewConfigured )
      {
        this.ListViewConfigured = true;
      }
    }

    /**************************************************************************/

    public new void RefreshData ( MacroscopeDocumentCollection DocCollection )
    {

      if ( DocCollection.CountDocuments() <= 0 )
      {
        return;
      }

      if ( this.MainForm.InvokeRequired )
      {
        this.MainForm.Invoke(
          new MethodInvoker(
            delegate
            {
              Cursor.Current = Cursors.WaitCursor;
              this.DisplayListView.BeginUpdate();
              this.RenderListView( DocCollection: DocCollection );
              this.RenderUrlCount();
              this.DisplayListView.EndUpdate();
              Cursor.Current = Cursors.Default;
            }
          )
        );
      }
      else
      {
        Cursor.Current = Cursors.WaitCursor;
        this.DisplayListView.BeginUpdate();
        this.RenderListView( DocCollection: DocCollection );
        this.RenderUrlCount();
        this.DisplayListView.EndUpdate();
        Cursor.Current = Cursors.Default;
      }

    }

    /**************************************************************************/

    public new void RenderListView ( MacroscopeDocumentCollection DocCollection )
    {

      List<ListViewItem> ListViewItems = new List<ListViewItem>( 1 );
      MacroscopeDocumentList OrphanedDocumentList = DocCollection.GetOrphanedDocumentList();

      this.ClearData();

      if ( OrphanedDocumentList != null )
      {

        foreach ( MacroscopeDocument msDoc in OrphanedDocumentList.IterateDocuments() )
        {

          this.RenderListView(
            ListViewItems: ListViewItems,
            DocCollection: DocCollection,
            msDoc: msDoc,
            Url: msDoc.GetUrl()
          );
        }

      }

      this.DisplayListView.Items.AddRange( ListViewItems.ToArray() );

    }

    /**************************************************************************/

    protected override void RenderListView (
      List<ListViewItem> ListViewItems,
      MacroscopeDocumentCollection DocCollection,
      MacroscopeDocument msDoc,
      string Url
    )
    {

      string StatusCode = ( (int) msDoc.GetStatusCode() ).ToString();
      string Status = msDoc.GetStatusCode().ToString();
      string PairKey = string.Join( "", Url );
      ListViewItem lvItem = null;

      if ( this.DisplayListView.Items.ContainsKey( PairKey ) )
      {

        try
        {

          lvItem = this.DisplayListView.Items[ PairKey ];
          lvItem.SubItems[ COL_URL ].Text = Url;
          lvItem.SubItems[ COL_STATUS_CODE ].Text = StatusCode;
          lvItem.SubItems[ COL_STATUS ].Text = Status;

        }
        catch ( Exception ex )
        {
          DebugMsg( string.Format( "MacroscopeDisplayOrphanedPages 1: {0}", ex.Message ) );
        }

      }
      else
      {

        try
        {

          lvItem = new ListViewItem( PairKey );
          lvItem.UseItemStyleForSubItems = false;
          lvItem.Name = PairKey;

          lvItem.SubItems[ COL_URL ].Text = Url;
          lvItem.SubItems.Add( StatusCode );
          lvItem.SubItems.Add( Status );

          ListViewItems.Add( lvItem );

        }
        catch ( Exception ex )
        {
          DebugMsg( string.Format( "MacroscopeDisplayOrphanedPages 2: {0}", ex.Message ) );
        }

      }

      if ( lvItem != null )
      {

        lvItem.ForeColor = Color.Blue;

        if ( msDoc.GetIsInternal() )
        {
          lvItem.SubItems[ COL_URL ].ForeColor = Color.Green;
        }
        else
        {
          lvItem.SubItems[ COL_URL ].ForeColor = Color.Gray;
        }

        if ( Regex.IsMatch( StatusCode, "^[2]" ) )
        {
          lvItem.SubItems[ COL_STATUS_CODE ].ForeColor = Color.Green;
          lvItem.SubItems[ COL_STATUS ].ForeColor = Color.Green;
        }
        else
        if ( Regex.IsMatch( StatusCode, "^[3]" ) )
        {
          lvItem.SubItems[ COL_STATUS_CODE ].ForeColor = Color.Goldenrod;
          lvItem.SubItems[ COL_STATUS ].ForeColor = Color.Goldenrod;
        }
        else
        if ( Regex.IsMatch( StatusCode, "^[45]" ) )
        {
          lvItem.SubItems[ COL_STATUS_CODE ].ForeColor = Color.Red;
          lvItem.SubItems[ COL_STATUS ].ForeColor = Color.Red;
        }
        else
        {
          lvItem.SubItems[ COL_STATUS_CODE ].ForeColor = Color.Blue;
          lvItem.SubItems[ COL_STATUS ].ForeColor = Color.Blue;
        }

      }

    }

    /**************************************************************************/

    protected override void RenderUrlCount ()
    {
    }

    /**************************************************************************/

  }

}
