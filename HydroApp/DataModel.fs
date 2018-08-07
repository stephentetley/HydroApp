// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

namespace HydroApp

module DataModel = 


    type Hydro = 
        | Hydroranger
        | Multianger
        | HydroPlus
        | Hydro200

    type Relay = 
        { RelayName: string 
          RelayStart: option<decimal>
          RelayStop: option<decimal> }

    
    type Relays = Map<int,Relay>

    /// The model from which the view is generated
    type Model =
        { HydroType: Hydro
          SerialNumber: string
          Span: option<decimal>
          Spill: option<decimal>
          Relays: Relays }


    type PickList<'a> = array<string * 'a>

    let pickListSource (items:PickList<'a>) : array<string> = Array.map fst items

    let hydroPickList : PickList<Hydro> = 
        [| ("Hydroranger",          Hydro.Hydroranger)
         ; ("Multiranger",          Hydro.Multianger)
         ; ("Hydroranger Plus",     Hydro.HydroPlus)
         ; ("Hydroranger 200",      Hydro.Hydro200) 
        |]
