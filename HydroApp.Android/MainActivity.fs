// Copyright 2018 Elmish.XamarinForms contributors. See LICENSE.md for license.
namespace HydroApp.Android

open System
open System.IO

open Android.App
open Android.Content
open Android.Content.PM
open Android.Runtime
open Android.Views
open Android.Widget
open Android.OS
open Xamarin.Forms.Platform.Android

[<Activity (Label = "HydroApp.Android", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = (ConfigChanges.ScreenSize ||| ConfigChanges.Orientation))>]
type MainActivity() =
    inherit FormsAppCompatActivity()

    let getDbFile() =
        let path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        Path.Combine(path, "hydro.db");

    let readWriteStream (ins : Stream) (outs : Stream) : unit = 
        let buffer : Byte[] = Array.zeroCreate 256
        let rec work (bytesRead : int) : unit = 
            if bytesRead <= 0 then 
                ins.Close() 
                outs.Close ()
            else
                outs.Write(buffer, 0, bytesRead)
                let bytesRead = ins.Read(buffer, 0, 256)
                work bytesRead
        let bytesRead = ins.Read(buffer, 0, 256)
        work bytesRead

    override this.OnCreate (bundle: Bundle) =
        FormsAppCompatActivity.TabLayoutResource <- Resources.Layout.Tabbar
        FormsAppCompatActivity.ToolbarResource <- Resources.Layout.Toolbar
        base.OnCreate (bundle)

        let dbFile = getDbFile ()
        if not <| System.IO.File.Exists(dbFile) then 
            let ins : Stream  = this.ApplicationContext.Resources.OpenRawResource(Resources.Raw.HydroApp)
            let outs : FileStream = new FileStream(dbFile, FileMode.OpenOrCreate, FileAccess.Write)
            readWriteStream ins outs

        
        Xamarin.Essentials.Platform.Init(this, bundle)

        Xamarin.Forms.Forms.Init (this, bundle)
       
        let dbPath = getDbFile()
        let appcore  = new HydroApp.App(dbPath)
        this.LoadApplication (appcore)

    override this.OnRequestPermissionsResult(requestCode: int, permissions: string[], [<GeneratedEnum>] grantResults: Android.Content.PM.Permission[]) =
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults)

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults)
