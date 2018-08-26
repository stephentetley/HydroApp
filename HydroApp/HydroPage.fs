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

module HydroPage = 
    

    /// The messages dispatched by the view
    type Msg =
        | HydroChanged of LevelControlType
        | SerialNumberChanged of string
        | SpanChanged of string
        | SpillChanged of string



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
    let update (msg:Msg) (model:LevelControl) : LevelControl =
        match msg with
        | HydroChanged t -> { model with LCType = t } 
        | SerialNumberChanged s -> { model with SerialNumber = s }
        | SpanChanged s -> 
            try
                let d = decimal s in { model with Span = Some d }
            with
            | _ -> model
        | SpillChanged s -> 
            try
                let d = decimal s in { model with Spill = Some d }
            with
            | _ -> model
    


    let view (model : Model) (dispatch : Msg -> unit) : ViewElement = 
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




