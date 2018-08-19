// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

namespace HydroApp


open HydroApp.Common

module DataModel = 


    //
    type SurveyInfo = 
        { EngineerName: string 
          DateOfSurvey: System.DateTime
          Site: string }


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
    type LevelControl =
        { LCType: LevelControlType
          TypeIfOther: string
          SerialNumber: string
          Span: option<decimal>
          Spill: option<decimal>
          Relays: Relays }

    

    let levelControlPickList : PickList<LevelControlType> = 
        [| ("Hydroranger",                  Hydroranger)
         ; ("Multiranger",                  Multianger)
         ; ("Hydroranger Plus",             HydroPlus)
         ; ("Hydroranger 200",              Hydro200) 
         ; ("Other, please specify...",     LCOther)
        |]

    

    type Model = 
        { PageStack: string list 
          SurveyInfo: SurveyInfo
          LevelControl: LevelControl }