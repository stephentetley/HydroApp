// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

namespace HydroApp

open System.Diagnostics
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms

open HydroApp.Common
open HydroApp.Database
open HydroApp.DataModel

module Application = 
    


    /// The messages dispatched by the view
    type Msg =
        | SitePageMsg of SitePage.Msg
        | HydroPageMsg of HydroPage.Msg




    /// Returns the initial state
    let init (dbPath:string) () : Model = 
        { PageStack = ["Home"]
          SurveyInfo = { EngineerName = ""; DateOfSurvey = System.DateTime.Now; Site = "" }
          LevelControl = 
            { LCType = HydroPlus
              TypeIfOther = ""
              SerialNumber = ""
              Span = None
              Spill = None
              Relays = Map.empty }
        }
          

    /// The funtion to update the view
    let update (msg:Msg) (model:Model) : Model =
        match msg with
        | _ -> model

    


    /// The view function giving updated content for the page
    let view (model: Model) (dispatch : Msg -> unit) : ViewElement =
        
        let homePage : ViewElement = 
            (SitePage.view model (SitePageMsg >> dispatch)).HasNavigationBar(true).HasBackButton(false)
        
        let hydroPage : ViewElement = 
            (HydroPage.view model (HydroPageMsg >> dispatch)).HasNavigationBar(true).HasBackButton(true)

        View.NavigationPage(
            pages = [homePage; hydroPage]
            )


    // let program = Program.mkProgram init update view

type App (dbPath:string) as app =
    inherit Application ()

    let runner =
        // Program.mkProgram App.init App.update App.view
        Program.mkSimple (Application.init dbPath) Application.update Application.view
        |> Program.withConsoleTrace
        |> Program.runWithDynamicView app


