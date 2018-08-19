// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

namespace HydroApp

open System.Diagnostics
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms

open HydroApp.Common
open HydroApp.DataModel

module App = 
    


    /// The messages dispatched by the view
    type Msg =
        | PagePopped
        | FieldEngineerChanged of string
        | SiteChanged of string
        | DateOfSurveyChanged of System.DateTime
        | HomeNextPressed
        | HydroChanged of LevelControlType
        | SerialNumberChanged of string
        | SpanChanged of string
        | SpillChanged of string

    /// Returns the initial state
    let init() : Model = 
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
        | HomeNextPressed -> { model with PageStack = ("Hydro" :: model.PageStack) }
        | HydroChanged t -> { model with LevelControl = { model.LevelControl with LCType = t } }
        | SerialNumberChanged s -> { model with LevelControl = { model.LevelControl with SerialNumber = s } }
        | SpanChanged s -> 
            try
                let d = decimal s in { model with LevelControl = { model.LevelControl with Span = Some d } }
            with
            | _ -> model
        | SpillChanged s -> 
            try
                let d = decimal s in { model with LevelControl = { model.LevelControl with Spill = Some d } }
            with
            | _ -> model
    

    let startPage (model : Model) (dispatch : Msg -> unit) : ViewElement = 
        View.ContentPage(
            content = View.StackLayout(
                padding = 20.0, 
                verticalOptions = LayoutOptions.Center,
                children = 
                    [ View.Label(text="Engineer Name:")
                    ; View.Entry(text= model.SurveyInfo.EngineerName, 
                                    textChanged = fun (args:TextChangedEventArgs) -> dispatch (FieldEngineerChanged args.NewTextValue))
                    ; View.Label(text="Site:")
                    ; View.Entry(text= model.SurveyInfo.Site, 
                                    textChanged = fun (args:TextChangedEventArgs) -> dispatch (SiteChanged args.NewTextValue))
                    ; View.Label(text="Date of Survey:")
                    ; View.DatePicker( date = model.SurveyInfo.DateOfSurvey,
                                        format = "dd MMM yyyy",
                                        dateSelected = fun (args:DateChangedEventArgs) -> dispatch (DateOfSurveyChanged args.NewDate))
                    ; View.Button( text = "Next", command = (fun () -> dispatch HomeNextPressed ))

                    ]))

    let hydroPage (model : Model) (dispatch : Msg -> unit) : ViewElement = 
        View.ContentPage(
            content = 
                View.StackLayout(
                    padding = 20.0, 
                    verticalOptions = LayoutOptions.Center,
                    children = 
                        [ View.Label(text="Model:")
                        ; View.Picker(title="Select Controller Model:", 
                                              selectedIndex=0, 
                                              itemsSource= pickListSource levelControlPickList, 
                                              horizontalOptions=LayoutOptions.Start, 
                                              selectedIndexChanged= fun (i, item) -> dispatch (HydroChanged (snd levelControlPickList.[i])))
                        ; View.Label(text="Serial Number:")
                        ; View.Entry(text= model.LevelControl.SerialNumber, 
                                     textChanged = fun (args:TextChangedEventArgs) -> dispatch (SerialNumberChanged args.NewTextValue))
                        ; View.Label(text="Span:")
                        ; View.Entry(text = optionText (fun a -> a.ToString()) model.LevelControl.Span,
                                     keyboard = Keyboard.Numeric,
                                     textChanged = fun (args:TextChangedEventArgs) -> dispatch (SpanChanged args.NewTextValue))
                        ; View.Label(text="Spill:")
                        ; View.Entry(text = optionText (fun a -> a.ToString()) model.LevelControl.Spill, 
                                     keyboard = Keyboard.Numeric,
                                     textChanged = fun (args:TextChangedEventArgs) -> dispatch (SpillChanged args.NewTextValue))
                        ]))

    /// The view function giving updated content for the page
    let view (model: Model) (dispatch : Msg -> unit) : ViewElement =
        View.NavigationPage(
            pages =
                match model.PageStack with
                | "Home" :: _ -> 
                    let homePage : ViewElement = (startPage model dispatch).HasNavigationBar(true).HasBackButton(false)
                    [homePage]
                | "Hydro" :: _ ->
                    let homePage : ViewElement = (startPage model dispatch).HasNavigationBar(true).HasBackButton(false)
                    let hydroPage : ViewElement = (hydroPage model dispatch).HasNavigationBar(true).HasBackButton(true)
                    [homePage; hydroPage]
                )


    // let program = Program.mkProgram init update view

type App () as app =
    inherit Application ()

    let runner =
        // Program.mkProgram App.init App.update App.view
        Program.mkSimple App.init App.update App.view
        |> Program.withConsoleTrace
        |> Program.runWithDynamicView app


