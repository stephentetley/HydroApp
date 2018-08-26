// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

namespace HydroApp

open System.Diagnostics
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms

open HydroApp.Common
open HydroApp.Database

module HydroPage = 
    
    type LevelControlType = 
        | Hydroranger
        | Multianger
        | HydroPlus
        | Hydro200
        | LCOther

    type Relay = 
        { RelayName: string 
          RelayStart: option<decimal>
          RelayStop: option<decimal> }

    
    type Relays = Map<int,Relay>

    /// The model from which the view is generated
    type Model =
        { LCType: LevelControlType
          TypeIfOther: string
          SerialNumber: string
          Span: option<decimal>
          Spill: option<decimal>
          Relays: Relays }

    /// The messages dispatched by the view
    type Msg =
        | HydroChanged of LevelControlType
        | SerialNumberChanged of string
        | SpanChanged of string
        | SpillChanged of string



    let levelControlPickList : PickList<LevelControlType> = 
        [| ("Hydroranger",                  Hydroranger)
         ; ("Multiranger",                  Multianger)
         ; ("Hydroranger Plus",             HydroPlus)
         ; ("Hydroranger 200",              Hydro200) 
         ; ("Other, please specify...",     LCOther)
        |]

    
    /// Returns the initial state
    let init () : Model = 
        {   
            LCType = HydroPlus
            TypeIfOther = ""
            SerialNumber = ""
            Span = None
            Spill = None
            Relays = Map.empty
        }
          

    /// The funtion to update the view
    let update (msg:Msg) (model:Model) : Model =
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
                        ; View.Entry(text= model.SerialNumber, 
                                     textChanged = fun (args:TextChangedEventArgs) -> dispatch (SerialNumberChanged args.NewTextValue))
                        ; View.Label(text="Span:")
                        ; View.Entry(text = optionText (fun a -> a.ToString()) model.Span,
                                     keyboard = Keyboard.Numeric,
                                     textChanged = fun (args:TextChangedEventArgs) -> dispatch (SpanChanged args.NewTextValue))
                        ; View.Label(text="Spill:")
                        ; View.Entry(text = optionText (fun a -> a.ToString()) model.Spill, 
                                     keyboard = Keyboard.Numeric,
                                     textChanged = fun (args:TextChangedEventArgs) -> dispatch (SpillChanged args.NewTextValue))
                        ]))




