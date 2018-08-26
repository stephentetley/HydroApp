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

module SitePage = 
    


    /// The messages dispatched by the view
    type Msg =
        | FieldEngineerChanged of string
        | SiteChanged of string
        | DateOfSurveyChanged of System.DateTime
        | SiteNextTapped

    type ExternalMsg = 
        | NoOp 
        | GoFoward



    /// The funtion to update the view
    let update (msg:Msg) (model:Model) : Model =
        match msg with
        | FieldEngineerChanged s -> { model with SurveyInfo = { model.SurveyInfo with EngineerName = s } }
        

    let view (model : Model) (dispatch : Msg -> unit) : ViewElement = 
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
                    ; View.Button( text = "Next", command = (fun () -> dispatch SiteNextTapped ))

                    ]))




