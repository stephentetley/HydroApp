// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

namespace HydroApp

open System.Diagnostics
open Elmish.XamarinForms
open Elmish.XamarinForms.DynamicViews
open Xamarin.Forms

module App = 


    type Hydro = 
        | Hydroranger
        | HydroPlus
        | Hydro200
    
    /// The model from which the view is generated
    type Model =
        { HydroType: Hydro
          SerialNumber: string
          Span: option<decimal>
          Spill: option<decimal> }

    let optionText (fn:'a -> string) (source:option<'a>) : string = 
        match source with
        | None -> ""
        | Some a -> fn a

    /// The messages dispatched by the view
    type Msg =
        | HydroChanged of Hydro
        | SerialNumberChanged of string
        | SpanChanged of string
        | SpillChanged of string

    /// Returns the initial state
    let init() : Model = 
        { HydroType = HydroPlus
          SerialNumber = ""
          Span = None
          Spill = None }

    /// The funtion to update the view
    let update (msg:Msg) (model:Model) : Model =
        match msg with
        | HydroChanged t -> { model with HydroType = t }
        | SerialNumberChanged s -> { model with SerialNumber = s}
        | SpanChanged s -> 
            try
                let d = decimal s in { model with Span = Some d}
            with
            | _ -> model
        | SpillChanged s -> 
            try
                let d = decimal s in { model with Spill = Some d}
            with
            | _ -> model
    
    let pickerItems = 
        [| ("Hydroranger",          Hydro.Hydroranger)
         ; ("Hydroranger Plus",     Hydro.HydroPlus)
         ; ("Hydroranger 200",      Hydro.Hydro200) 
        |]


    /// The view function giving updated content for the page
    let view (model: Model) dispatch  =
        // the original code cause object not set errors, but it works once we add
        // it into a contentPage
        View.ContentPage(
          content = View.StackLayout(padding = 20.0, verticalOptions = LayoutOptions.Center,
            children = 
                [ View.Label(text="Hydro:")
                ; View.Picker(title="Select Controller Model:", 
                                      selectedIndex=0, 
                                      itemsSource=(Array.map fst pickerItems), 
                                      horizontalOptions=LayoutOptions.Start, 
                                      selectedIndexChanged= fun (i, item) -> dispatch (HydroChanged (snd pickerItems.[i])))
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

    // let program = Program.mkProgram init update view

type App () as app =
    inherit Application ()

    let runner =
        // Program.mkProgram App.init App.update App.view
        Program.mkSimple App.init App.update App.view
        |> Program.withConsoleTrace
        |> Program.runWithDynamicView app


