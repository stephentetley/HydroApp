// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

namespace HydroApp

module Common = 
    
    let optionText (fn:'a -> string) (source:option<'a>) : string = 
        match source with
        | None -> ""
        | Some a -> fn a


    type PickList<'a> = array<string * 'a>

    let pickListSource (items:PickList<'a>) : array<string> = Array.map fst items

    

