# Neon Terminal Lite – free sci-fi GUI theme for Ren'Py 8

The free part of the Neon Terminal GUI kit: a main menu with a neon grid, scanlines and a
glitchy title, a chamfered terminal textbox with a magenta name tag, a choice menu whose
hover frame glows and pulses, and a matching quick menu. Version 1.0.0, tested with
Ren'Py 8.5.3 at 1920×1080 and 1280×720.

## Install

1. Copy the folder `game/` of this ZIP into your project, so that `game/neon_terminal_lite.rpy`
   and `game/gui/neon_terminal_lite/` sit next to your own files.
2. Start the game. Nothing else to change: your gui.rpy, screens.rpy and options.rpy stay as
   they are.

The Lite restyles four screens: the main menu, the say screen (textbox and name), the choice
menu and the quick menu. Every other screen (game menu, save and load, preferences, history,
help, confirm, NVL and so on) stays your project's own. The art set (1080 or 720) is picked
from your game's width.

## Change colours

Colours, fonts and the effect switch are at the top of `neon_terminal_lite.rpy`
(`nel_cyan_color`, `nel_magenta_color`, … and `nel_effects = False` to switch the scanlines,
the title glitch and the hover pulse off).

## The full kit

The full Neon Terminal kit styles every standard screen of Ren'Py in the same look (game
menu, save and load, preferences with sliders and check boxes, history, help, confirm,
input, NVL with NVL menu, notify, skip indicator, about), adds the glitch transition
between menu pages and includes the art generator:
https://juliusschmidt.itch.io/neon-terminal-renpy-gui

**Remove the Lite before installing the full kit:** delete `game/neon_terminal_lite.rpy` and
the folder `game/gui/neon_terminal_lite/`. If both are installed, the game stops with a
message that says so.

## Licence

See LICENSE.txt. Font: IBM Plex Mono, SIL Open Font License 1.1 (text in
`game/gui/neon_terminal_lite/fonts/`). Made with the help of an AI assistant (Claude by
Anthropic); code, art generation and tests were run and checked by the author.
