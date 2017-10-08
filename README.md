# StrEnc (WinForms)


## Overview

    ########
    # TODO #
    ########


## Known Issues

This program is *not* DPI-aware, which means if you run it on a computer whose display is set to something other than 72 dpi, the UI will look blurry. Currently, there are no plans to add hi-DPI support, because of the complexity of doing so in WinForms (which seems to entail complete overhaul of the UI; as I went through some trouble to make sure the current design worked, I'm not particular eager to find my way through the myriad amount of confusing detail and corner cases of the legacy beast that is WinForms). However, the planned WPF version will have high DPI support.
